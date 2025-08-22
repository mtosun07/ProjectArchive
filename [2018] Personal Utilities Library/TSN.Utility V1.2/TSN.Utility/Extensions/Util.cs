using System;
using System.Collections.Generic;
using System.Threading;
using TSN.Utility.Entities;
using TSN.Utility.Entities.ObjectModels;

namespace TSN.Utility.Extensions
{
    public static class Util
    {
        static Util()
        {
            _locker0 = new object();
        }


        private static readonly object _locker0;



        public static bool IsDerivedFromIEntity(this Type t)
        {
            var gen = typeof(IEntity<>);
            for (var type = t; type != null; type = type.BaseType)
                if (type.IsSubclassOf(gen.MakeGenericType(type)))
                    return true;
            return false;
        }
        public static bool IsDerivedFromEntityBase(this Type t)
        {
            var gen = typeof(EntityBase<>);
            for (var type = t; type != null; type = type.BaseType)
                if (type.IsSubclassOf(gen.MakeGenericType(type)))
                    return true;
            return false;
        }
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            if (ex != null)
            {
                foreach (var exc in GetInnerExceptions(ex.InnerException))
                    yield return exc;
                if (ex is AggregateException aggEx)
                    foreach (var exc in aggEx.InnerExceptions)
                        foreach (var _exc in GetInnerExceptions(exc))
                            yield return _exc;
            }
        }
        public static bool TryToDo(Action work)
        {
            if (work == null)
                throw new ArgumentNullException(nameof(work));
            try
            {
                work();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static void TryCatchFinally(Action @try, Action @catch = null, Action @finally = null)
        {
            if (@try == null)
                throw new ArgumentNullException(nameof(@try));
            if (@catch == null && @finally == null)
                throw new ArgumentNullException(nameof(@catch));
            try
            {
                @try();
            }
            catch
            {
                @catch?.Invoke();
            }
            finally
            {
                @finally?.Invoke();
            }
        }
        public static void ExecuteParallel(params Action[] tasks)
        {
            // Source:  https://stackoverflow.com/a/35913978
            var resetEvents = new ManualResetEvent[tasks.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                resetEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback((object index) =>
                {
                    int taskIndex = (int)index;
                    tasks[taskIndex]();
                    resetEvents[taskIndex].Set();
                }), i);
            }
            WaitHandle.WaitAll(resetEvents);
        }
        public static Wrapper<T> MakeReferenceType<T>(this T obj)
            where T : struct
        {
            return new Wrapper<T>(obj);
        }
    }
}