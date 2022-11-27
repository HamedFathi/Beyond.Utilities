namespace Beyond.Utilities.MoreTypes;

public struct ParenthesesPair
{
    public ParenthesesPair(int id, int startIndex, int endIndex, int depth, string value)
    {
        if (startIndex > endIndex)
            throw new ArgumentException("startIndex must be less than endIndex");

        StartIndex = startIndex;
        EndIndex = endIndex;
        Depth = depth;
        Value = value;
        Id = id;
    }

    public int Depth { get; }
    public int EndIndex { get; }
    public int Id { get; }
    public int StartIndex { get; }
    public string Value { get; }
}