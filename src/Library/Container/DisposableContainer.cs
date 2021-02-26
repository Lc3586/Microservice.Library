using Microservice.Library.Collection;
using System;

namespace Microservice.Library.Container
{
    public class DisposableContainer : IDisposableContainer, IDisposable
    {
        SynchronizedCollection<IDisposable> _objList
            = new SynchronizedCollection<IDisposable>();

        public void AddDisposableObj(IDisposable disposableObj)
        {
            if (!_objList.Contains(disposableObj))
                _objList.Add(disposableObj);
        }

        public void Dispose()
        {
            foreach (var x in _objList)
            {
                try
                {
                    x.Dispose();
                }
                catch
                {

                }
            }
            _objList.Dispose();
            _objList = null;
        }
    }
}
