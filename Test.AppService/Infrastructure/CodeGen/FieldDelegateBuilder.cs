using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Test.AppService.Infrastructure.CodeGen;

public readonly struct FieldDelegateBuilder
{

    public readonly Delegate     Func;
    public readonly FieldBuilder Field;

    public MethodInfo Invoke => Field.FieldType.GetMethod("Invoke") ?? throw new NotSupportedException($"{Field.FieldType.Name} dont support invoke!");

    public FieldDelegateBuilder(TypeBuilder typeBuilder, LambdaExpression expession, int fieldIndex)
    {
        Func = expession.Compile();
        Field = typeBuilder.DefineField($"func_{fieldIndex}", Func.GetType(), FieldAttributes.Private);
    }
}

