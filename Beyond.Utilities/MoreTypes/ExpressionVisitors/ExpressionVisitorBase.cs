// ReSharper disable IdentifierTypo
namespace Beyond.Utilities.MoreTypes.ExpressionVisitors;

[DebuggerStepThrough]
[DebuggerNonUserCode]
public abstract class ExpressionVisitorBase
{
    public virtual Expression Visit(Expression? exp)
    {
        if (exp == null)
        {
            return exp!;
        }
        switch (exp.NodeType)
        {
            case ExpressionType.Negate:
            case ExpressionType.NegateChecked:
            case ExpressionType.Not:
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
            case ExpressionType.ArrayLength:
            case ExpressionType.Quote:
            case ExpressionType.TypeAs:
                return VisitUnary((UnaryExpression)exp);
            case ExpressionType.Add:
            case ExpressionType.AddChecked:
            case ExpressionType.Subtract:
            case ExpressionType.SubtractChecked:
            case ExpressionType.Multiply:
            case ExpressionType.MultiplyChecked:
            case ExpressionType.Divide:
            case ExpressionType.Modulo:
            case ExpressionType.And:
            case ExpressionType.AndAlso:
            case ExpressionType.Or:
            case ExpressionType.OrElse:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.Coalesce:
            case ExpressionType.ArrayIndex:
            case ExpressionType.RightShift:
            case ExpressionType.LeftShift:
            case ExpressionType.ExclusiveOr:
                return VisitBinary((BinaryExpression)exp);
            case ExpressionType.TypeIs:
                return VisitTypeIs((TypeBinaryExpression)exp);
            case ExpressionType.Conditional:
                return VisitConditional((ConditionalExpression)exp);
            case ExpressionType.Constant:
                return VisitConstant((ConstantExpression)exp);
            case ExpressionType.Parameter:
                return VisitParameter((ParameterExpression)exp);
            case ExpressionType.MemberAccess:
                return VisitMemberAccess((MemberExpression)exp);
            case ExpressionType.Call:
                return VisitMethodCall((MethodCallExpression)exp);
            case ExpressionType.Lambda:
                return VisitLambda((LambdaExpression)exp);
            case ExpressionType.New:
                return VisitNew((NewExpression)exp);
            case ExpressionType.NewArrayInit:
            case ExpressionType.NewArrayBounds:
                return VisitNewArray((NewArrayExpression)exp);
            case ExpressionType.Invoke:
                return VisitInvocation((InvocationExpression)exp);
            case ExpressionType.MemberInit:
                return VisitMemberInit((MemberInitExpression)exp);
            case ExpressionType.ListInit:
                return VisitListInit((ListInitExpression)exp);
            case ExpressionType.UnaryPlus:
            case ExpressionType.Power:
            case ExpressionType.Assign:
            case ExpressionType.Block:
            case ExpressionType.DebugInfo:
            case ExpressionType.Decrement:
            case ExpressionType.Dynamic:
            case ExpressionType.Default:
            case ExpressionType.Extension:
            case ExpressionType.Goto:
            case ExpressionType.Increment:
            case ExpressionType.Index:
            case ExpressionType.Label:
            case ExpressionType.RuntimeVariables:
            case ExpressionType.Loop:
            case ExpressionType.Switch:
            case ExpressionType.Throw:
            case ExpressionType.Try:
            case ExpressionType.Unbox:
            case ExpressionType.AddAssign:
            case ExpressionType.AndAssign:
            case ExpressionType.DivideAssign:
            case ExpressionType.ExclusiveOrAssign:
            case ExpressionType.LeftShiftAssign:
            case ExpressionType.ModuloAssign:
            case ExpressionType.MultiplyAssign:
            case ExpressionType.OrAssign:
            case ExpressionType.PowerAssign:
            case ExpressionType.RightShiftAssign:
            case ExpressionType.SubtractAssign:
            case ExpressionType.AddAssignChecked:
            case ExpressionType.MultiplyAssignChecked:
            case ExpressionType.SubtractAssignChecked:
            case ExpressionType.PreIncrementAssign:
            case ExpressionType.PreDecrementAssign:
            case ExpressionType.PostIncrementAssign:
            case ExpressionType.PostDecrementAssign:
            case ExpressionType.TypeEqual:
            case ExpressionType.OnesComplement:
            case ExpressionType.IsTrue:
            case ExpressionType.IsFalse:
            default:
                throw new NotSupportedException($"Unhandled expression type: '{exp.NodeType}'");
        }
    }
    protected virtual MemberBinding VisitBinding(MemberBinding binding)
    {
        switch (binding.BindingType)
        {
            case MemberBindingType.Assignment:
                return VisitMemberAssignment((MemberAssignment)binding);
            case MemberBindingType.MemberBinding:
                return VisitMemberMemberBinding((MemberMemberBinding)binding);
            case MemberBindingType.ListBinding:
                return VisitMemberListBinding((MemberListBinding)binding);
            default:
                throw new NotSupportedException($"Unhandled binding type '{binding.BindingType}'");
        }
    }
    protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
    {
        var arguments = VisitList(initializer.Arguments);
        if (arguments != initializer.Arguments)
        {
            return Expression.ElementInit(initializer.AddMethod, arguments);
        }
        return initializer;
    }
    protected virtual Expression VisitUnary(UnaryExpression u)
    {
        var operand = Visit(u.Operand);
        if (operand != u.Operand)
        {
            return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
        }
        return u;
    }
    protected virtual Expression VisitBinary(BinaryExpression b)
    {
        var left = Visit(b.Left);
        var right = Visit(b.Right);
        var conversion = Visit(b.Conversion);
        if (left != b.Left || right != b.Right || conversion != b.Conversion)
        {
            if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
            {
                return Expression.Coalesce(left, right, conversion as LambdaExpression);
            }
            return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
        }
        return b;
    }
    protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
    {
        var expr = Visit(b.Expression);
        if (expr != b.Expression)
        {
            return Expression.TypeIs(expr, b.TypeOperand);
        }
        return b;
    }
    protected virtual Expression VisitConstant(ConstantExpression c)
    {
        return c;
    }
    protected virtual Expression VisitConditional(ConditionalExpression c)
    {
        var test = Visit(c.Test);
        var ifTrue = Visit(c.IfTrue);
        var ifFalse = Visit(c.IfFalse);
        if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
        {
            return Expression.Condition(test, ifTrue, ifFalse);
        }
        return c;
    }
    protected virtual Expression VisitParameter(ParameterExpression p)
    {
        return p;
    }
    protected virtual Expression VisitMemberAccess(MemberExpression m)
    {
        var exp = Visit(m.Expression);
        return !Equals(exp, m.Expression) ? Expression.MakeMemberAccess(exp, m.Member) : m;
    }
    protected virtual Expression VisitMethodCall(MethodCallExpression m)
    {
        var obj = Visit(m.Object);
        IEnumerable<Expression> args = VisitList(m.Arguments);
        if (!Equals(obj, m.Object) || !Equals(args, m.Arguments))
        {
            return Expression.Call(obj, m.Method, args);
        }
        return m;
    }
    protected virtual ReadOnlyCollection<Expression> VisitList(ReadOnlyCollection<Expression> original)
    {
        List<Expression>? list = null;
        for (int i = 0, n = original.Count; i < n; i++)
        {
            var p = Visit(original[i]);
            if (list != null)
            {
                list.Add(p);
            }
            else if (p != original[i])
            {
                list = new List<Expression>(n);
                for (var j = 0; j < i; j++)
                {
                    list.Add(original[j]);
                }
                list.Add(p);
            }
        }
        return list != null ? list.AsReadOnly() : original;
    }
    protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
    {
        var e = Visit(assignment.Expression);
        if (e != assignment.Expression)
        {
            return Expression.Bind(assignment.Member, e);
        }
        return assignment;
    }
    protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
        var bindings = VisitBindingList(binding.Bindings);
        if (!Equals(bindings, binding.Bindings))
        {
            return Expression.MemberBind(binding.Member, bindings);
        }
        return binding;
    }
    protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
    {
        var initializers = VisitElementInitializerList(binding.Initializers);
        return !Equals(initializers, binding.Initializers) ? Expression.ListBind(binding.Member, initializers) : binding;
    }
    protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
    {
        List<MemberBinding>? list = null;
        for (int i = 0, n = original.Count; i < n; i++)
        {
            var b = VisitBinding(original[i]);
            if (list != null)
            {
                list.Add(b);
            }
            else if (b != original[i])
            {
                list = new List<MemberBinding>(n);
                for (var j = 0; j < i; j++)
                {
                    list.Add(original[j]);
                }
                list.Add(b);
            }
        }
        if (list != null)
        {
            return list;
        }
        return original;
    }
    protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
    {
        List<ElementInit>? list = null;
        for (int i = 0, n = original.Count; i < n; i++)
        {
            var init = VisitElementInitializer(original[i]);
            if (list != null)
            {
                list.Add(init);
            }
            else if (init != original[i])
            {
                list = new List<ElementInit>(n);
                for (var j = 0; j < i; j++)
                {
                    list.Add(original[j]);
                }
                list.Add(init);
            }
        }
        if (list != null)
        {
            return list;
        }
        return original;
    }
    protected virtual Expression VisitLambda(LambdaExpression lambda)
    {
        var body = Visit(lambda.Body);
        if (body != lambda.Body)
        {
            return Expression.Lambda(lambda.Type, body, lambda.Parameters);
        }
        return lambda;
    }
    protected virtual NewExpression VisitNew(NewExpression nex)
    {
        IEnumerable<Expression> args = VisitList(nex.Arguments);
        if (Equals(args, nex.Arguments)) return nex;
        return nex.Members != null ? Expression.New(nex.Constructor!, args, nex.Members) : Expression.New(nex.Constructor!, args);
    }
    protected virtual Expression VisitMemberInit(MemberInitExpression init)
    {
        var n = VisitNew(init.NewExpression);
        var bindings = VisitBindingList(init.Bindings);
        if (n != init.NewExpression || !Equals(bindings, init.Bindings))
        {
            return Expression.MemberInit(n, bindings);
        }
        return init;
    }
    protected virtual Expression VisitListInit(ListInitExpression init)
    {
        var n = VisitNew(init.NewExpression);
        var initializers = VisitElementInitializerList(init.Initializers);
        if (!Equals(n, init.NewExpression) || !Equals(initializers, init.Initializers))
        {
            return Expression.ListInit(n, initializers);
        }
        return init;
    }
    protected virtual Expression VisitNewArray(NewArrayExpression na)
    {
        IEnumerable<Expression> exprs = VisitList(na.Expressions);
        if (Equals(exprs, na.Expressions)) return na;
        return na.NodeType == ExpressionType.NewArrayInit ? Expression.NewArrayInit(na.Type.GetElementType()!, exprs) : Expression.NewArrayBounds(na.Type.GetElementType()!, exprs);
    }
    protected virtual Expression VisitInvocation(InvocationExpression iv)
    {
        IEnumerable<Expression> args = VisitList(iv.Arguments);
        var expr = Visit(iv.Expression);
        if (!Equals(args, iv.Arguments) || expr != iv.Expression)
        {
            return Expression.Invoke(expr, args);
        }
        return iv;
    }
}