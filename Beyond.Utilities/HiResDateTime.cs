// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Beyond.Utilities;

public class HiResDateTime
{
    private static long lastTimeStamp = DateTime.Now.Ticks;
    private static long lastUtcTimeStamp = DateTime.UtcNow.Ticks;

    public static long NowTicks
    {
        get
        {
            long original, newValue;
            do
            {
                original = lastTimeStamp;
                long now = DateTime.UtcNow.Ticks;
                newValue = Math.Max(now, original + 1);
            } while (Interlocked.CompareExchange
                         (ref lastTimeStamp, newValue, original) != original);

            return newValue;
        }
    }

    public static long UtcNowTicks
    {
        get
        {
            long original, newValue;
            do
            {
                original = lastUtcTimeStamp;
                long now = DateTime.UtcNow.Ticks;
                newValue = Math.Max(now, original + 1);
            } while (Interlocked.CompareExchange
                         (ref lastUtcTimeStamp, newValue, original) != original);

            return newValue;
        }
    }
}