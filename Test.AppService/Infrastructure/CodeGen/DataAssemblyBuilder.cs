using System.Reflection;
using System.Reflection.Emit;

namespace Test.AppService.Infrastructure.CodeGen
{
    public static class DataAssemblyBuilder
    {
        private static AssemblyBuilder? asm;
        private static ModuleBuilder? module;

        public static TypeBuilder DefineType(string name)
        {
            asm ??= AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"{typeof(DataAssemblyBuilder).Name}-{Guid.NewGuid()}"), AssemblyBuilderAccess.RunAndCollect);
            module ??= asm.DefineDynamicModule("root");
            return module.DefineType(name, TypeAttributes.Class);
        }

        public static void Call(this ILGenerator gen, MethodInfo method)
        {
            if (method.IsVirtual || (method.DeclaringType != null && method.DeclaringType.IsInterface))
            {
                gen.Emit(OpCodes.Callvirt, method);
                return;
            }

            gen.Emit(OpCodes.Call, method);
        }


        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            if (type.IsInterface)
            {
                return type.GetProperties().Union(type.GetInterfaces().SelectMany(x => x.GetProperties()));
            }

            return type.GetProperties();
        }
    }
}

