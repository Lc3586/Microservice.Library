using System;

namespace Library.Container
{
    public interface IDisposableContainer : IDisposable
    {
        void AddDisposableObj(IDisposable disposableObj);
    }
}
