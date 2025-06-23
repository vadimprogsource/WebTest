using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Test.Api;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure.CodeGen;
using Test.Entity;

namespace Test.AppService.Infrastructure;

public class DataMapper<TSource,TDestination>
{
    private readonly Dictionary<string, PropertyInfo> props;


    public DataMapper()
    {
        props = typeof(TSource).GetAllProperties().Where(x => x.CanRead).ToDictionary(x => x.Name);
    }

    public DataMapper<TSource, TDestination> Exclude<TValue>(Expression<Func<TSource, TValue>> property)
    {
        if (property.Body is MemberExpression m)
        {
            props.Remove(m.Member.Name);
        }

        return this;
    }


    private readonly struct IncludeInfo
    {
        public readonly PropertyInfo      Property;
        public readonly LambdaExpression  Expression;

        public IncludeInfo(LambdaExpression property, LambdaExpression expression)
        {

            if (property.Body is MemberExpression m && m.Member is PropertyInfo p && p.CanWrite)
            {
                Property = p;
                Expression = expression;
                return;
            }

            throw new NotSupportedException();
        }


    }


    private readonly List<IncludeInfo> includes = new();

    public DataMapper<TSource, TDestination> Include<TValue>(Expression<Func<TDestination, TValue>> property , Expression<Func<TSource,TValue>> expression)
    {
        includes.Add(new IncludeInfo(property, expression));
        return this;
    }



    public IDataMapper<TSource, TDestination> Compile()
    {

        DataMappingBuilder<TSource, TDestination> builder = new();

        foreach (PropertyInfo targetProp in typeof(TDestination).GetAllProperties().Where(x=>x.CanWrite))
        {
            if(props.TryGetValue(targetProp.Name , out PropertyInfo? sourceProp) && sourceProp != null && targetProp.PropertyType == sourceProp.PropertyType && sourceProp.CanRead && targetProp.CanWrite)
            {
                builder.AppendCopyProperty(sourceProp, targetProp);
            }
        }

        foreach (IncludeInfo i in includes)
        {
            builder.AppendSetProperty(i.Property, i.Expression);
        }

        return builder.Build();

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

