// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities;

public static class NotationConverter
{
    public static string CompleteParenthesisOfInfix(string infix)
    {
        if (string.IsNullOrWhiteSpace(infix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(infix));
        infix = infix.RemoveWhitespace();
        var postfix = InfixToPostfix(infix);
        var newInfix = PostfixToInfix(postfix);
        if (!newInfix.StartsWith("(") && !newInfix.EndsWith(")"))
        {
            newInfix = $"({newInfix})";
        }
        return newInfix;
    }

    public static string InfixToPostfix(string infix)
    {
        if (string.IsNullOrWhiteSpace(infix))
        {
            throw new ArgumentException($"'{nameof(infix)}' cannot be null or whitespace.", nameof(infix));
        }
        infix = infix.RemoveWhitespace();
        var stk = new Stack<char>();
        var output = "";
        char @out;
        foreach (var ch in infix)
        {
            if (char.IsLetterOrDigit(ch))
            {
                output += ch;
            }
            else
            {
                switch (ch)
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '^':
                        while (stk.Count > 0 && Precedence(ch) <= Precedence(stk.Peek()))
                        {
                            @out = stk.Peek();
                            stk.Pop();
                            output = output + " " + @out;
                        }
                        stk.Push(ch);
                        output += " ";
                        break;

                    case '(':
                        stk.Push(ch);
                        break;

                    case ')':
                        while (stk.Count > 0 && (@out = stk.Peek()) != '(')
                        {
                            stk.Pop();
                            output = output + " " + @out + " ";
                        }
                        if (stk.Count > 0 && stk.Peek() == '(')
                            stk.Pop();
                        break;
                }
            }
        }
        while (stk.Count > 0)
        {
            @out = stk.Peek();
            stk.Pop();
            output = output + @out + " ";
        }
        return output.RemoveWhitespace();
    }

    public static string InfixToPrefix(string infix)
    {
        if (string.IsNullOrWhiteSpace(infix))
        {
            throw new ArgumentException($"'{nameof(infix)}' cannot be null or whitespace.", nameof(infix));
        }
        infix = infix.RemoveWhitespace();
        var postfix = InfixToPostfix(infix.Reverse().MirrorParenthesis());
        var prefix = postfix.Reverse();
        return prefix;
    }

    public static bool IsValidInfix(string infix)
    {
        if (string.IsNullOrWhiteSpace(infix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(infix));

        infix = infix.RemoveWhitespace();
        var condition = IsWellFormatted(infix);
        var prev = ' ';
        foreach (var ch in infix)
        {
            if (ch is '(' or ')') continue;

            var isLetter = char.IsLetter(ch);
            if (prev != ' ')
            {
                if (isLetter == char.IsLetter(prev))
                {
                    return false;
                }
                if (!isLetter == !char.IsLetter(prev))
                {
                    return false;
                }
            }
            prev = ch;
        }
        infix = infix.Replace("(", "").Replace(")", "");
        var condition2 = char.IsLetter(infix[0]) && char.IsLetter(infix[^1]);

        return condition && condition2;
    }

    public static string PostfixToInfix(string postfix)
    {
        if (string.IsNullOrWhiteSpace(postfix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(postfix));
        postfix = postfix.RemoveWhitespace();
        var stackOperands = new Stack<string>();
        foreach (var ch in postfix)
        {
            if (char.IsLetter(ch))
            {
                stackOperands.Push(ch.ToString());
            }
            else
            {
                var operandOld = stackOperands.Pop();
                var operandOlder = stackOperands.Pop();
                var newStr = $"({operandOlder}{ch}{operandOld})";
                stackOperands.Push(newStr);
            }
        }
        return stackOperands.Count == 1 ? stackOperands.Pop() : string.Empty;
    }

    public static string PostFixToPrefix(string postfix)
    {
        if (string.IsNullOrWhiteSpace(postfix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(postfix));
        postfix = postfix.RemoveWhitespace();
        var infix = PostfixToInfix(postfix);
        var prefix = InfixToPrefix(infix);
        return prefix;
    }

    public static string PrefixToInfix(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(prefix));
        prefix = prefix.RemoveWhitespace();
        prefix = prefix.Reverse();
        var stackOperands = new Stack<string>();
        foreach (var ch in prefix)
        {
            if (char.IsLetter(ch))
            {
                stackOperands.Push(ch.ToString());
            }
            else
            {
                var operandOld = stackOperands.Pop();
                var operandOlder = stackOperands.Pop();
                var newStr = $"({operandOlder}{ch}{operandOld})";
                stackOperands.Push(newStr);
            }
        }

        if (stackOperands.Count == 1)
        {
            var infix = stackOperands.Pop().Reverse().MirrorParenthesis();
            return infix;
        }
        return string.Empty;
    }

    public static string PrefixToPostFix(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(prefix));
        prefix = prefix.RemoveWhitespace();

        var infix = PrefixToInfix(prefix);
        var postfix = InfixToPostfix(infix);
        return postfix;
    }

    private static bool IsWellFormatted(string str)
    {
        var lastOpen = new Stack<char>();
        foreach (var c in str)
        {
            switch (c)
            {
                case ')':
                    if (lastOpen.Count == 0 || lastOpen.Pop() != '(') return false;
                    break;
                case ']':
                    if (lastOpen.Count == 0 || lastOpen.Pop() != '[') return false;
                    break;
                case '}':
                    if (lastOpen.Count == 0 || lastOpen.Pop() != '{') return false;
                    break;
                case '(': lastOpen.Push(c); break;
                case '[': lastOpen.Push(c); break;
                case '{': lastOpen.Push(c); break;
            }
        }
        return lastOpen.Count == 0;
    }
    private static string MirrorParenthesis(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(str));

        var newStr = "";
        foreach (var s in str)
        {
            newStr += s switch
            {
                ')' => "(",
                '(' => ")",
                _ => s
            };
        }
        return newStr;
    }

    private static int Precedence(this char op)
    {
        if (op is '*' or '/' or '%')
            return 3;
        if (op is '+' or '-')
            return 2;
        if (op == '^')
            return 1;
        return -1;
    }

    private static string RemoveWhitespace(this string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
    }

    private static string Reverse(this string str)
    {
        var charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}