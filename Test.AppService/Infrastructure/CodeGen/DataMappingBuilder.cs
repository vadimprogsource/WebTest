using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure.CodeGen;




public class DataMappingBuilder<TSource, TDestination>
{
    private readonly TypeBuilder type;
    private readonly ILGenerator generator;
    private readonly List<FieldDelegateBuilder> getters = new();

    public DataMappingBuilder()
    {
        type = DataAssemblyBuilder.DefineType($"DataMapper<{typeof(TSource).Name},{typeof(TDestination).Name}>");
        type.AddInterfaceImplementation(typeof(IDataMapper<TSource, TDestination>));

        MethodBuilder map = type.DefineMethod("Map", MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.Public,CallingConventions.HasThis, typeof(TDestination), new[] {typeof(TSource) , typeof(TDestination) });
        generator = map.GetILGenerator();


        MethodBuilder @new = type.DefineMethod("New", MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.Public,CallingConventions.HasThis, typeof(TDestination), new[] { typeof(TSource) });
        ILGenerator gen = @new.GetILGenerator();
        gen.Emit(OpCodes.Ldarg_0);
        gen.Emit(OpCodes.Ldarg_1);
        gen.Emit(OpCodes.Newobj, typeof(TDestination).GetConstructor(Array.Empty<Type>()) ?? throw new NotSupportedException());
        gen.Call(map);
        gen.Emit(OpCodes.Ret);
    }


    public DataMappingBuilder<TSource, TDestination> AppendCopyProperty(PropertyInfo fromProperty, PropertyInfo toProperty)
    {
        generator.Emit(OpCodes.Ldarg_2);
        generator.Emit(OpCodes.Ldarg_1);
        generator.Call(fromProperty.GetGetMethod(true)?? throw new NullReferenceException($"getter property {fromProperty.Name} is null!"));
        generator.Call(toProperty.GetSetMethod(true) ?? throw new NullReferenceException($"setter property  {toProperty.Name} is null!"));
        return this;
    }


    public DataMappingBuilder<TSource, TDestination> AppendSetProperty(PropertyInfo property, LambdaExpression expression)
    {

        FieldDelegateBuilder builder = new (type, expression, getters.Count);
        getters.Add(builder);

        generator.Emit(OpCodes.Ldarg_2);
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Ldfld, builder.Field);
        generator.Emit(OpCodes.Ldarg_1);
        generator.Call(builder.Invoke);
        generator.Call(property.GetSetMethod(true) ?? throw new NotSupportedException($"Property  {property.Name} is readonly!"));

        return this;
    }

    public IDataMapper<TSource, TDestination> Build()
    {
        generator.Emit(OpCodes.Ldarg_2);
        generator.Emit(OpCodes.Ret);


        ConstructorBuilder ctor;
        ILGenerator gen;

        if (getters.Count > 0)
        {
            ctor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, getters.Select(x => x.Field.FieldType).ToArray());
            gen = ctor.GetILGenerator();

            int i = 0;
            foreach (FieldDelegateBuilder builder in getters)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg, ++i);
                gen.Emit(OpCodes.Stfld, builder.Field);
            }

            gen.Emit(OpCodes.Ret);

            if (Activator.CreateInstance(type.CreateType(), getters.Select(x => x.Func).ToArray<object>()) is IDataMapper<TSource, TDestination> m)
            {
                return m;
            }

        }
        else
        {
            ctor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard,Array.Empty<Type>());
            gen = ctor.GetILGenerator();
            gen.Emit(OpCodes.Ret);

            if (Activator.CreateInstance(type.CreateType()) is IDataMapper<TSource, TDestination> _m)
            {
                return _m;
            }

        }


        throw new TypeAccessException();
    }
}

