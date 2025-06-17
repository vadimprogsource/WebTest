using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;

namespace Test.AppService.Infrastructure;

public class DataMapper<TSource,TDestination>
{
    private readonly Dictionary<string, PropertyInfo> props;

    public DataMapper()
    {
        props = typeof(TSource).GetProperties().Where(x=>x.CanRead).ToDictionary(x => x.Name);
    }

    public DataMapper<TSource, TDestination> Exclude<TValue>(Expression<Func<TSource, TValue>> property)
    {
        if (property.Body is MemberExpression m)
        {
            props.Remove(m.Member.Name);
        }

        return this;
    }

    private static void CallMethod(ILGenerator gen, MethodInfo method)
    {
        if (method.IsVirtual || (method.DeclaringType != null && method.DeclaringType.IsInterface))
        {
            gen.Emit(OpCodes.Callvirt, method);
            return;
        }

        gen.Emit(OpCodes.Call, method);

    }

    private static void CallCopyProperty(ILGenerator gen, MethodInfo getter, MethodInfo setter)
    {
        gen.Emit(OpCodes.Dup);
        gen.Emit(OpCodes.Ldarg_0);
        CallMethod(gen, getter);
        CallMethod(gen, setter);
    }


    private readonly struct Executor : IDataMapper<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> ctor;
        private readonly Func<TSource, TDestination,TDestination> map;

        public Executor(DynamicMethod ctor , DynamicMethod map)
        {
            this.ctor = ctor.CreateDelegate<Func<TSource, TDestination>>();
            this.map  = map.CreateDelegate<Func<TSource, TDestination, TDestination>>();
        }

        public TDestination New(TSource source) => ctor(source);
        public TDestination Map(TSource source, TDestination desctination) => map(source, desctination);
    }



    public IDataMapper<TSource, TDestination> Compile()
    {

        DynamicMethod @new = new("new", typeof(TDestination), new[] { typeof(TSource)});
        ILGenerator new_gen = @new.GetILGenerator();
        new_gen.Emit(OpCodes.Newobj, typeof(TDestination).GetConstructor(Array.Empty<Type>()) ?? throw new NotSupportedException());


        DynamicMethod map = new("map", typeof(TDestination), new[] { typeof(TSource), typeof(TDestination) });
        ILGenerator map_gen = map.GetILGenerator();
        map_gen.Emit(OpCodes.Ldarg_1);

        foreach (PropertyInfo dest in typeof(TDestination).GetProperties().Where(x=>x.CanWrite))
        {
            if(props.TryGetValue(dest.Name , out PropertyInfo? scr) && scr!=null && dest.PropertyType == scr.PropertyType)
            {

                MethodInfo? getter = scr.GetGetMethod() , setter = dest.GetSetMethod();

                if (getter == null || setter == null) continue;

                CallCopyProperty(new_gen, getter, setter);
                CallCopyProperty(map_gen, getter,setter);
            }
        }

        new_gen.Emit(OpCodes.Ret);
        map_gen.Emit(OpCodes.Ret);

        return new Executor(@new, map);

    }


}

public abstract class EntityDataMapper<TInterface, TEntity> : DataMapper<TInterface, TEntity> where TInterface : IEntity where TEntity : EntityBase, TInterface
{
    public EntityDataMapper()
    {
        Exclude(x => x.Guid);
        Exclude(x => x.CreatedAt);
        Exclude(x => x.IsValid);
    }
}

