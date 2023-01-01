// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes.ExpressionVisitors;

public class ExpressionStringVisitor : ExpressionVisitor
{
    private readonly StringBuilder _sb = new();

    public static string ConvertToString(Expression expression)
    {
        var visitor = new ExpressionStringVisitor();
        visitor.Visit(expression);
        return visitor._sb.ToString();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sb.Append("(");
        Visit(node.Left);
        _sb.Append(GetOperator(node.NodeType));
        Visit(node.Right);
        _sb.Append(")");
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _sb.Append(node.Value);
        return node;
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        _sb.Append(node.Parameters[0].Name);
        _sb.Append(" => ");
        Visit(node.Body);
        return node;
    }

    private static string GetOperator(ExpressionType nodeType)
    {
        switch (nodeType)
        {
            case ExpressionType.Add:
                return " + ";

            case ExpressionType.AndAlso:
                return " && ";

            case ExpressionType.Divide:
                return " / ";

            case ExpressionType.Equal:
                return " == ";

            case ExpressionType.GreaterThan:
                return " > ";

            case ExpressionType.GreaterThanOrEqual:
                return " >= ";

            case ExpressionType.LessThan:
                return " < ";

            case ExpressionType.LessThanOrEqual:
                return " <= ";

            case ExpressionType.Multiply:
                return " * ";

            case ExpressionType.NotEqual:
                return " != ";

            case ExpressionType.OrElse:
                return " || ";

            case ExpressionType.Subtract:
                return " - ";

            default:
                throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
        }
    }
}