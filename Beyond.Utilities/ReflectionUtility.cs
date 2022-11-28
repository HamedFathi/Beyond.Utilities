// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local☺

using Beyond.Utilities.MoreTypes.ExpressionVisitors;

namespace Beyond.Utilities;

public static class ReflectionUtility
{
    public static TProperty? GetProperty<TClass, TProperty>(TClass instanceType, string propertyName)
        where TClass : class
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        object? obj = null;
        var type = instanceType.GetType();
        var info = type.GetTypeInfo().GetProperty(propertyName);
        if (info != null)
            obj = info.GetValue(instanceType, null);
        return (TProperty?)obj;
    }

    public static object? GetProperty(Type instanceType, string propertyName)
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        object? obj = null;
        var info = instanceType.GetTypeInfo().GetProperty(propertyName);
        if (info != null)
            obj = info.GetValue(instanceType, null);
        return obj;
    }

    public static void SetProperty<TClass>(TClass instanceType, string propertyName, object propertyValue)
        where TClass : class
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        var type = instanceType.GetType();
        var info = type.GetTypeInfo().GetProperty(propertyName);

        if (info != null)
            info.SetValue(instanceType, Convert.ChangeType(propertyValue, info.PropertyType), null);
    }

    public static void SetProperty(Type instanceType, string propertyName, object propertyValue)
    {
        if (propertyName == null || string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        var info = instanceType.GetTypeInfo().GetProperty(propertyName);

        if (info != null)
            info.SetValue(instanceType, Convert.ChangeType(propertyValue, info.PropertyType), null);
    }
    public static bool MeetsSpecialGenericConstraints(Type genericArgType, Type proposedSpecificType)
    {
        var genericArgTypeInfo = genericArgType.GetTypeInfo();
        var proposedSpecificTypeInfo = proposedSpecificType.GetTypeInfo();
        var gpa = genericArgTypeInfo.GenericParameterAttributes;
        var constraints = gpa & GenericParameterAttributes.SpecialConstraintMask;
        if (constraints == GenericParameterAttributes.None)
        {
            return true;
        }
        if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0
            && proposedSpecificTypeInfo.IsValueType)
        {
            return false;
        }
        if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0
            && !proposedSpecificTypeInfo.IsValueType)
        {
            return false;
        }
        if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0
            && proposedSpecificType.GetConstructor(Type.EmptyTypes) == null)
        {
            return false;
        }
        return true;
    }
    public static PropertyInfo GetProperty<TModel>(Expression<Func<TModel, object>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return (PropertyInfo)memberExpression.Member;
    }
    public static PropertyInfo GetProperty<TModel, T>(Expression<Func<TModel, T>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return (PropertyInfo)memberExpression.Member;
    }
    public static PropertyInfo? GetProperty(LambdaExpression expression)
    {
        var memberExpression = GetMemberExpression(expression, true);
        return (PropertyInfo?)memberExpression?.Member;
    }
    private static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression)
    {
        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }
        if (memberExpression == null)
        {
            throw new ArgumentException("Not a member access", nameof(expression));
        }
        return memberExpression;
    }
    public static MemberExpression? GetMemberExpression(this LambdaExpression expression, bool enforceMemberExpression)
    {
        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }
        if (enforceMemberExpression && memberExpression == null)
        {
            throw new ArgumentException("Not a member access", nameof(expression));
        }
        return memberExpression;
    }
    public static bool IsMemberExpression<T>(Expression<Func<T, object>> expression)
    {
        return IsMemberExpression<T, object>(expression);
    }
    public static bool IsMemberExpression<T, TU>(Expression<Func<T, TU>> expression)
    {
        return GetMemberExpression(expression, false) != null;
    }
    private static bool TryEvaluateExpression(Expression? operation, out object? value)
    {
        if (operation == null)
        {
            value = null;
            return true;
        }
        switch (operation.NodeType)
        {
            case ExpressionType.Constant:
                value = ((ConstantExpression)operation).Value;
                return true;
            case ExpressionType.MemberAccess:
                var me = (MemberExpression)operation;
                if (TryEvaluateExpression(me.Expression, out var target))
                {
                    FieldInfo? fieldInfo;
                    if (null != (fieldInfo = me.Member as FieldInfo))
                    {
                        value = fieldInfo.GetValue(target);
                        return true;
                    }
                    PropertyInfo? propertyInfo;
                    if (null != (propertyInfo = me.Member as PropertyInfo))
                    {
                        value = propertyInfo.GetValue(target, null);
                        return true;
                    }
                }
                break;
        }
        value = null;
        return false;
    }
    public static MethodInfo? GetMethod<T>(Expression<Func<T, object>> expression)
    {
        return new FindMethodVisitor(expression).Method;
    }
    public static MethodInfo? GetMethod(Expression<Func<object>> expression)
    {
        return GetMethod<Func<object>>(expression);
    }
    public static MethodInfo? GetMethod(Expression expression)
    {
        return new FindMethodVisitor(expression).Method;
    }
    public static MethodInfo? GetMethod<TDelegate>(Expression<TDelegate> expression)
    {
        return new FindMethodVisitor(expression).Method;
    }
    public static MethodInfo? GetMethod<T, TU>(Expression<Func<T, TU>> expression)
    {
        return new FindMethodVisitor(expression).Method;
    }
    public static MethodInfo? GetMethod<T, TU, TV>(Expression<Func<T, TU, TV>> expression)
    {
        return new FindMethodVisitor(expression).Method;
    }
    public static MethodInfo? GetMethod<T>(Expression<Action<T>> expression)
    {
        return new FindMethodVisitor(expression).Method;
    }
}