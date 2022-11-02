// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class ThreadUtility
{
    public static Thread Repeat(TimeSpan interval, Action action, bool isBackground = false, string? name = null)
    {
        return new Thread(() =>
        {
            // ReSharper disable once UnusedVariable
            var timer = new Timer(_ => action(), null, TimeSpan.Zero, interval);
        })
        {
            IsBackground = isBackground,
            Name = name
        };
    }

    public static Thread Repeat(TimeSpan interval, Action action, Func<bool> isStopped, bool isBackground = false,
        string? name = null)
    {
        return new Thread(() =>
        {
            var timer = new Timer(_ => { action(); }, null, TimeSpan.Zero, interval);
            if (isStopped()) timer.Dispose();
        })
        {
            IsBackground = isBackground,
            Name = name
        };
    }
}