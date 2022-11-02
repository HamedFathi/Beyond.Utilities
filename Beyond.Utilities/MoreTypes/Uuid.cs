// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace Beyond.Utilities.MoreTypes;

public readonly struct Uuid : IEquatable<Uuid>
{
    public Uuid(long mostSignificantBits, long leastSignificantBits)
    {
        MostSignificantBits = mostSignificantBits;
        LeastSignificantBits = leastSignificantBits;
    }

    public Uuid(byte[] b)
    {
        if (b == null)
            throw new ArgumentNullException(nameof(b));
        if (b.Length != 16)
            throw new ArgumentException("Length of the UUID byte array should be 16");
        MostSignificantBits = BitConverter.ToInt64(b, 0);
        LeastSignificantBits = BitConverter.ToInt64(b, 8);
    }

    public long LeastSignificantBits { get; }

    public long MostSignificantBits { get; }

    public static explicit operator Guid(Uuid uuid)
    {
        if (uuid == default) return default;
        var uuidMostSignificantBytes = BitConverter.GetBytes(uuid.MostSignificantBits);
        var uuidLeastSignificantBytes = BitConverter.GetBytes(uuid.LeastSignificantBits);
        byte[] guidBytes =
        {
            uuidMostSignificantBytes[4],
            uuidMostSignificantBytes[5],
            uuidMostSignificantBytes[6],
            uuidMostSignificantBytes[7],
            uuidMostSignificantBytes[2],
            uuidMostSignificantBytes[3],
            uuidMostSignificantBytes[0],
            uuidMostSignificantBytes[1],
            uuidLeastSignificantBytes[7],
            uuidLeastSignificantBytes[6],
            uuidLeastSignificantBytes[5],
            uuidLeastSignificantBytes[4],
            uuidLeastSignificantBytes[3],
            uuidLeastSignificantBytes[2],
            uuidLeastSignificantBytes[1],
            uuidLeastSignificantBytes[0]
        };
        return new Guid(guidBytes);
    }

    public static implicit operator Uuid(Guid value)
    {
        if (value == default) return default;
        var guidBytes = value.ToByteArray();
        byte[] uuidBytes =
        {
            guidBytes[6],
            guidBytes[7],
            guidBytes[4],
            guidBytes[5],
            guidBytes[0],
            guidBytes[1],
            guidBytes[2],
            guidBytes[3],
            guidBytes[15],
            guidBytes[14],
            guidBytes[13],
            guidBytes[12],
            guidBytes[11],
            guidBytes[10],
            guidBytes[9],
            guidBytes[8]
        };
        return new Uuid(BitConverter.ToInt64(uuidBytes, 0), BitConverter.ToInt64(uuidBytes, 8));
    }

    public static Uuid NewUuid()
    {
        return Guid.NewGuid();
    }

    public static bool operator !=(Uuid a, Uuid b)
    {
        return !a.Equals(b);
    }

    public static bool operator ==(Uuid a, Uuid b)
    {
        return a.Equals(b);
    }

    public static Uuid Parse(string input)
    {
        return Guid.Parse(input);
    }

    public static Guid ToGuid(string input)
    {
        return Guid.Parse(input);
    }

    public static Guid ToGuid(Uuid input)
    {
        return (Guid)input;
    }

    public static bool TryParse(string input, out Uuid uuid)
    {
        try
        {
            uuid = new Guid(input.Replace("-", ""));
            return true;
        }
        catch
        {
            uuid = Guid.Empty;
            return false;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Uuid uuid && Equals(uuid);
    }

    public bool Equals(Uuid uuid)
    {
        return MostSignificantBits == uuid.MostSignificantBits && LeastSignificantBits == uuid.LeastSignificantBits;
    }

    public override int GetHashCode()
    {
        return ((Guid)this).GetHashCode();
    }

    public byte[] ToByteArray()
    {
        var uuidMostSignificantBytes = BitConverter.GetBytes(MostSignificantBits);
        var uuidLeastSignificantBytes = BitConverter.GetBytes(LeastSignificantBits);
        byte[] bytes =
        {
            uuidMostSignificantBytes[0],
            uuidMostSignificantBytes[1],
            uuidMostSignificantBytes[2],
            uuidMostSignificantBytes[3],
            uuidMostSignificantBytes[4],
            uuidMostSignificantBytes[5],
            uuidMostSignificantBytes[6],
            uuidMostSignificantBytes[7],
            uuidLeastSignificantBytes[0],
            uuidLeastSignificantBytes[1],
            uuidLeastSignificantBytes[2],
            uuidLeastSignificantBytes[3],
            uuidLeastSignificantBytes[4],
            uuidLeastSignificantBytes[5],
            uuidLeastSignificantBytes[6],
            uuidLeastSignificantBytes[7]
        };
        return bytes;
    }

    public override string ToString()
    {
        return GetDigits(MostSignificantBits >> 32, 8) + "-" +
               GetDigits(MostSignificantBits >> 16, 4) + "-" +
               GetDigits(MostSignificantBits, 4) + "-" +
               GetDigits(LeastSignificantBits >> 48, 4) + "-" +
               GetDigits(LeastSignificantBits, 12);
    }

    private static string GetDigits(long val, int digits)
    {
        var hi = 1L << (digits * 4);
        return $"{hi | (val & (hi - 1)):X}"[1..];
    }
}