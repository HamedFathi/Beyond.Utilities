// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities.MoreTypes;

public class Blockchain<T>
{
    private readonly object _lockObject = new ();
    public class Block
    {
        internal Block(T? data)
        {
            Index = 0;
            TimeStampUtc = DateTimeOffset.UtcNow;
            PreviousHash = null;
            Data = data;
            Hash = CalculateHash();
        }

        internal T? Data { get; set; }
        internal string Hash { get; set; }
        internal int Index { get; set; }
        internal string? PreviousHash { get; set; }
        internal DateTimeOffset TimeStampUtc { get; set; }

        internal string CalculateHash()
        {
            var data = JsonSerializer.Serialize(Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes($"{TimeStampUtc}-{PreviousHash ?? ""}-{data}");
            var outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
    }
    public Blockchain()
    {
        Chain = new List<Block>
        {
            new(default)
        };
    }

    public IList<Block> Chain { get; }

    public void AddBlock(T data)
    {
        lock (_lockObject)
        {
            var block = new Block(data);
            var latestBlock = Chain[^1];
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
        }
    }
}