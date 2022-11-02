// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public class AsyncLazy<T> : Lazy<Task<T>>
{
    public AsyncLazy(Func<T> valueFactory) :
        base(() => Task.Factory.StartNew(valueFactory))
    {
    }

    public AsyncLazy(Func<Task<T>> taskFactory) :
        base(() => Task.Factory.StartNew(taskFactory).Unwrap())
    {
    }
}