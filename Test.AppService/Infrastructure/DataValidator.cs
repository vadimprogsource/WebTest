using System;
using System.Linq.Expressions;
using System.Reflection;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure
{
    public abstract class DataValidator<TEntity> : IDataValidator<TEntity>
    {
        private readonly struct Error : IError
        {
            private readonly MemberInfo memberInfo;

    

            public string Code { get; }

            public string Reason { get; }

            public string Source => memberInfo.Name;

            public Error(Expression expression , string code , string reason)
            {
                Code = code;
                Reason = reason;

                if (expression.NodeType == ExpressionType.MemberAccess && expression is MemberExpression m)
                {
                    memberInfo = m.Member;
                    return;
                }
                throw new NotSupportedException();
            }
        }

        public List<IError> errors = new ();

        protected void Throw<TValue>(Expression<Func<TEntity, TValue>> property, string code = "", string reason = "")
        {
            errors.Add(new Error(property.Body, code, reason));
        }

        protected abstract void OnValidate(TEntity entity);
        

        public bool Validate(TEntity entity)
        {
            OnValidate(entity);
            return errors.Count<1;
            throw new NotImplementedException();
        }

        public IEnumerable<IError> Errors => errors;
    }
}

