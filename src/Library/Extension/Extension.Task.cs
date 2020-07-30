using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tcs"></param>
        /// <param name="result">返回值</param>
        /// <returns></returns>
        public static bool TrySetResult<T>(this TaskCompletionSource<T> tcs, T result)
        {
            try
            {
                if (tcs == null || tcs.Task.Status != TaskStatus.WaitingForActivation)
                    return false;
                tcs.SetResult(result);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
