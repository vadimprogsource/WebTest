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
        private readonly PropertyInfo property;
        private readonly LambdaExpression   expression;

        public IncludeInfo(PropertyInfo property, LambdaExpression expression)
        {
            this.property   = property;
            this.expression = expression;
        }


        public void AppendTo(DataMappingBuilder<TSource, TDestination> builder)
        {
            builder.AppendSetProperty(property, expression);
        }

    }


    private readonly List<IncludeInfo> includes = new();

    public DataMapper<TSource, TDestination> Include<TValue>(Expression<Func<TDestination, TValue>> property , Expression<Func<TSource,TValue>> expression)
    {
        if (property.Body is MemberExpression member && member.Member is PropertyInfo prop)
        {
            if (prop.CanWrite)
            {
                includes.Add(new IncludeInfo(prop, expression));
            }
        }

        return this;
    }



    public IDataMapper<TSource, TDestination> Compile()
    {

        DataMappingBuilder<TSource, TDestination> builder = new();


        foreach (PropertyInfo dest in typeof(TDestination).GetAllProperties().Where(x=>x.CanWrite))
        {
            if(props.TryGetValue(dest.Name , out PropertyInfo? scr) && scr!=null && dest.PropertyType == scr.PropertyType && scr.CanRead && dest.CanWrite)
            {
                builder.AppendCopyProperty(scr, dest);
            }
        }

        foreach (IncludeInfo include in includes)
        {
           include.AppendTo(builder);
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

