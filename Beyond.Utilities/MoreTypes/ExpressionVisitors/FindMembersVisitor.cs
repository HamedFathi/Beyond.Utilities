// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes.ExpressionVisitors;

public class FindMembersVisitor : ExpressionVisitor
{
    internal static readonly PropertyInfo ArrayLength = typeof(Array).GetProperty(nameof(Array.Length))!;
    public static MemberInfo? Member<T>(Expression<Func<T, object>> expression)
    {
        var finder = new FindMembersVisitor();
        finder.Visit(expression);
        return finder.Members.LastOrDefault();
    }
    public readonly IList<MemberInfo> Members = new List<MemberInfo>();
    protected override Expression VisitMember(MemberExpression node)
    {
        Members.Insert(0, node.Member);
        return base.VisitMember(node);
    }
    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.Name == "Count" && node.Method.ReturnType == typeof(int))
        {
            Members.Insert(0, ArrayLength);
        }
        return base.VisitMethodCall(node);
    }
    protected sealed override Expression VisitUnary(UnaryExpression node)
    {
        if (node.NodeType == ExpressionType.ArrayLength)
        {
            Members.Insert(0, ArrayLength);
        }
        return base.VisitUnary(node);
    }
    public static MemberInfo[] Determine(Expression expression)
    {
        var visitor = new FindMembersVisitor();
        visitor.Visit(expression);
        return visitor.Members.ToArray();
    }
}