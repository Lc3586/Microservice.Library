using System;

namespace Microservice.Library.Container
{
    /// <summary>
    /// 
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class DisposableContainer : IDisposableContainer, IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        SynchronizedCollection<IDisposable> _objList
            = new SynchronizedCollection<IDisposable>();

        public void AddDisposableObj(IDisposable disposableObj)
        {
            if (!_objList.Contains(disposableObj))
                _objList.Add(disposableObj);
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            foreach (var x in _objList)
            {
                try
                {
                    x.Dispose();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
                {

                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
            _objList.Dispose();
            _objList = null;
        }
    }
}
