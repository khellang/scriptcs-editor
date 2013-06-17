using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ScriptCsPad.Extensions
{
    public static class ReflectionExtensions
    {
        public static MethodInfo GetMethodInfo<T>(this T value, Expression<Action<T>> expression)
        {
            return GetMethodInfo(expression);
        }

        private static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new InvalidOperationException("Expression must be a method call.");

            return methodCallExpression.Method;
        }
    }
}