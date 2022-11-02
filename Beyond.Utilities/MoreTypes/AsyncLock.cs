// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities.MoreTypes;

public class AsyncLock : IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public void Dispose()
    {
        if (_semaphoreSlim.CurrentCount == 0) _semaphoreSlim.Release();

        GC.SuppressFinalize(this);
    }

    // ReSharper disable once UnusedMember.Global
    public async Task<AsyncLock> LockAsync()
    {
        await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
        return this;
    }
}