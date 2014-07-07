namespace Zabavnov.MVVM
{
    public delegate bool TryFunc<TResult>(out TResult result);
    public delegate bool TryFunc<in T, TResult>(T arg, out TResult result);
    public delegate bool TryFunc<in T1, in T2, TResult>(T1 arg, T2 arg2, out TResult result);
    public delegate bool TryFunc<in T1, in T2, in T3, TResult>(T1 arg, T2 arg2, T3 arg3, out TResult result);
    public delegate bool TryFunc<in T1, in T2, in T3, in T4, TResult>(T1 arg, T2 arg2, T3 arg3, T4 arg4, out TResult result);
}
