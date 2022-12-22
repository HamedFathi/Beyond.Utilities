// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class Notation2Converter
{
    public static string CompleteParenthesisOfInfix(string infix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)", Func<string, int>? precedence = null)
    {
        if (string.IsNullOrWhiteSpace(infix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(infix));
        infix = infix.RemoveWhitespaces();
        var postfix = InfixToPostfix(infix, operandPattern, operatorsAndParenthesesPattern, precedence);
        var newInfix = PostfixToInfix(postfix, operandPattern, operatorsAndParenthesesPattern);
        return $"({newInfix})";
    }

    public static string InfixToPostfix(string infix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)", Func<string, int>? precedence = null)
    {
        if (string.IsNullOrWhiteSpace(infix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(infix));
        if (string.IsNullOrWhiteSpace(operandPattern))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(operandPattern));
        if (!operandPattern.IsValidRegex())
            throw new ArgumentException("Value is not valid for a regex pattern.", nameof(operandPattern));
        if (string.IsNullOrWhiteSpace(operatorsAndParenthesesPattern))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(operatorsAndParenthesesPattern));
        if (!operatorsAndParenthesesPattern.IsValidRegex())
            throw new ArgumentException("Value is not valid for a regex pattern.",
                nameof(operatorsAndParenthesesPattern));

        var parentheses = new[] { "(", ")" };
        precedence ??= Precedence;
        var result = string.Empty;
        string @out;
        var stack = new Stack<string>();
        infix = infix.RemoveWhitespaces();
        var matches = Regex.Matches(infix, $"{operandPattern}|{operatorsAndParenthesesPattern}");
        foreach (Match match in matches)
        {
            var val = match.Value;
            if (!parentheses.Contains(val) && !Regex.IsMatch(val, operatorsAndParenthesesPattern))
            {
                result += val;
            }
            else
            {
                if (!parentheses.Contains(val) && Regex.IsMatch(val, operatorsAndParenthesesPattern))
                {
                    while (stack.Count > 0 && precedence(val) <= precedence(stack.Peek()))
                    {
                        @out = stack.Peek();
                        stack.Pop();
                        result = result + " " + @out;
                    }

                    stack.Push(val);
                    result += " ";
                }
                else if (val == "(")
                {
                    stack.Push(val);
                }
                else if (val == ")")
                {
                    while (stack.Count > 0 && (@out = stack.Peek()) != "(")
                    {
                        stack.Pop();
                        result = result + " " + @out + " ";
                    }

                    if (stack.Count > 0 && stack.Peek() == "(")
                        stack.Pop();
                }
            }
        }

        while (stack.Count > 0)
        {
            @out = stack.Peek();
            stack.Pop();
            result = result + @out + " ";
        }

        return result.RemoveWhitespaces();
    }

    public static string InfixToPrefix(string infix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)", Func<string, int>? precedence = null)
    {
        if (string.IsNullOrWhiteSpace(infix))
        {
            throw new ArgumentException($"'{nameof(infix)}' cannot be null or whitespace.", nameof(infix));
        }

        infix = infix.RemoveWhitespaces();
        var newInput = Regex.Matches(infix, $"{operandPattern}|{operatorsAndParenthesesPattern}")
            .Select(x => x.Value).Reverse().Aggregate((a, b) => a + b);
        var postfix = InfixToPostfix(newInput.MirrorParenthesis(), operandPattern, operatorsAndParenthesesPattern,
            precedence);
        var prefix = Regex.Matches(postfix, $"{operandPattern}|{operatorsAndParenthesesPattern}")
            .Select(x => x.Value).Reverse().Aggregate((a, b) => a + b);
        return prefix;
    }

    public static bool IsValidInfix(string infix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)")
    {
        if (string.IsNullOrWhiteSpace(infix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(infix));

        infix = infix.RemoveWhitespaces();
        var condition = IsWellFormattedParentheses(infix);
        var prev = string.Empty;
        var matches = Regex.Matches(infix, $"{operandPattern}|{operatorsAndParenthesesPattern}");

        foreach (Match match in matches)
        {
            var val = match.Value;

            if (val is "(" or ")") continue;

            if (prev != string.Empty)
            {
                var prevOperand = Regex.IsMatch(prev, operandPattern);
                var valOperand = Regex.IsMatch(val, operandPattern);
                if (prevOperand == valOperand)
                {
                    return false;
                }

                var prevOperator = Regex.IsMatch(prev, operatorsAndParenthesesPattern);
                var valOperator = Regex.IsMatch(val, operatorsAndParenthesesPattern);
                if (prevOperator == valOperator)
                {
                    return false;
                }
            }

            prev = val;
        }

        infix = infix.Replace("(", "").Replace(")", "");
        var condition2 = !Regex.IsMatch(infix[0].ToString(), operatorsAndParenthesesPattern) &&
                         !Regex.IsMatch(infix[^1].ToString(), operatorsAndParenthesesPattern);

        return condition && condition2;
    }

    public static string PostfixToInfix(string postfix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)")
    {
        if (string.IsNullOrWhiteSpace(postfix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(postfix));
        if (string.IsNullOrWhiteSpace(operandPattern))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(operandPattern));
        if (string.IsNullOrWhiteSpace(operatorsAndParenthesesPattern))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(operatorsAndParenthesesPattern));

        if (!operandPattern.IsValidRegex())
            throw new ArgumentException("Value is not valid for a regex pattern.", nameof(operandPattern));
        if (!operatorsAndParenthesesPattern.IsValidRegex())
            throw new ArgumentException("Value is not valid for a regex pattern.",
                nameof(operatorsAndParenthesesPattern));

        var stackOperands = new Stack<string>();
        postfix = postfix.RemoveWhitespaces();
        var matches = Regex.Matches(postfix, $"{operandPattern}|{operatorsAndParenthesesPattern}");
        foreach (Match match in matches)
        {
            var val = match.Value;
            if (!Regex.IsMatch(val, operatorsAndParenthesesPattern))
            {
                stackOperands.Push(val);
            }
            else
            {
                var operandOld = stackOperands.Pop();
                var operandOlder = stackOperands.Pop();
                var newStr = $"({operandOlder}{val}{operandOld})";
                stackOperands.Push(newStr);
            }
        }

        return stackOperands.Count == 1 ? stackOperands.Pop() : string.Empty;
    }

    public static string PostFixToPrefix(string postfix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)", Func<string, int>? precedence = null)
    {
        if (string.IsNullOrWhiteSpace(postfix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(postfix));
        postfix = postfix.RemoveWhitespaces();
        var infix = PostfixToInfix(postfix, operandPattern, operatorsAndParenthesesPattern);
        var prefix = InfixToPrefix(infix, operandPattern, operatorsAndParenthesesPattern, precedence);
        return prefix;
    }

    public static string PrefixToInfix(string prefix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)")
    {
        if (prefix == null) throw new ArgumentNullException(nameof(prefix));
        if (operandPattern == null) throw new ArgumentNullException(nameof(operandPattern));
        if (operatorsAndParenthesesPattern == null)
            throw new ArgumentNullException(nameof(operatorsAndParenthesesPattern));
        if (!operandPattern.IsValidRegex())
            throw new ArgumentException("Value is not valid for a regex pattern.", nameof(operandPattern));
        if (!operatorsAndParenthesesPattern.IsValidRegex())
            throw new ArgumentException("Value is not valid for a regex pattern.",
                nameof(operatorsAndParenthesesPattern));

        prefix = prefix.RemoveWhitespaces();
        var stackOperands = new Stack<string>();

        prefix = Regex.Matches(prefix, $"{operandPattern}|{operatorsAndParenthesesPattern}")
            .Select(x => x.Value).Reverse().Aggregate((a, b) => a + b);

        var matches = Regex.Matches(prefix, $"{operandPattern}|{operatorsAndParenthesesPattern}");
        foreach (Match match in matches)
        {
            var val = match.Value;
            if (Regex.IsMatch(val, operandPattern))
            {
                stackOperands.Push(val);
            }
            else
            {
                var operandOld = stackOperands.Pop();
                var operandOlder = stackOperands.Pop();
                var newStr = $"({operandOlder}{val}{operandOld})";
                stackOperands.Push(newStr);
            }
        }

        if (stackOperands.Count == 1)
        {
            var infix = Regex.Matches(stackOperands.Pop(), $"{operandPattern}|{operatorsAndParenthesesPattern}")
                .Select(x => x.Value).Reverse().Aggregate((a, b) => a + b);
            infix = infix.MirrorParenthesis();
            return infix;
        }

        return string.Empty;
    }

    public static string PrefixToPostFix(string prefix, string operandPattern = "[a-zA-Z]",
        string operatorsAndParenthesesPattern = @"\+|-|\*|/|\^|\||&|\(|\)", Func<string, int>? precedence = null)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(prefix));
        prefix = prefix.RemoveWhitespaces();

        var infix = PrefixToInfix(prefix, operandPattern, operatorsAndParenthesesPattern);
        var postfix = InfixToPostfix(infix, operandPattern, operatorsAndParenthesesPattern, precedence);
        return postfix;
    }

    private static bool IsValidRegex(this string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern)) return false;
        try
        {
            var _ = Regex.Match("", pattern);
        }
        catch (ArgumentException)
        {
            return false;
        }

        return true;
    }

    private static bool IsWellFormattedParentheses(string str)
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

                case '(':
                    lastOpen.Push(c);
                    break;

                case '[':
                    lastOpen.Push(c);
                    break;

                case '{':
                    lastOpen.Push(c);
                    break;
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

    private static int Precedence(this string op)
    {
        if (op is "*" or "/" or "%")
            return 3;
        if (op is "+" or "-")
            return 2;
        if (op == "^")
            return 1;
        return -1;
    }

    private static string RemoveWhitespaces(this string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
    }
}