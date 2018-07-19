using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public class TaskManage: ITaskMange
    {
        SynchronizationContext _context = null;
        /// <summary>
        /// 主线程（UI）同步上下文
        /// </summary>
        public SynchronizationContext Context { get => _context; }

        TaskFactory _factory= null;

        /// <summary>
        /// 任务管理器
        /// </summary>
        /// <param name="context"></param>
        public TaskManage()
        {
            _factory = new TaskFactory();
        }

        /// <summary>
        /// 初始化任务管理器
        /// 必须在UI线程执行
        /// </summary>
        /// <param name="context">UI线程同步上下文</param>
        /// <param name="exceptionHandler">异常处理</param>
        public void Init(SynchronizationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        public void RunTask<T>(Action<T> method, T param, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        {
            _factory.StartNew(()=> {
                try
                {
                    method(param);
                }
                catch (Exception ex)
                {
                    TaskLog.LogExcepion("执行异步任务出错", ex);
                }
            }, options);
        }

        /// <summary>
        /// 同步到UI线程执行
        /// 不等待执行完成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        public void InvokeUIByPost<T>(Action<T> method, T param)
        where T : class
        {
            _context.Post(new SendOrPostCallback(p =>
            {
                try
                {
                    T t1 = p as T;
                    method(t1);
                }
                catch (Exception ex)
                {
                    TaskLog.LogExcepion("任务异步委托到UI线程出错", ex);
                }
            }), param);
        }

        /// <summary>
        /// 同步到UI线程执行
        /// 等待执行完成后返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        public void InvokeUIBySend<T>(Action<T> method, T param)
        where T : class
        {
            _context.Send(new SendOrPostCallback(p =>
            {
                try
                {
                    T t1 = p as T;
                    method(t1);
                }
                catch (Exception ex)
                {
                    TaskLog.LogExcepion("任务同步委托到UI线程出错", ex);
                }
            }), param);
        }

        /// <summary>
        /// 执行一个异步操作，并在该异步操作执行结束后，将结果回调到UI函数处理。
        /// 注意：在执行同步任务时，需要先判断入参是否为空！
        /// </summary>
        /// <typeparam name="ParamIn">参数1类型</typeparam>
        /// <typeparam name="ParamOut">参数2类型</typeparam>
        /// <param name="asyncMethod">异步操作</param>
        /// <param name="callbackMethod">UI回调函数</param>
        /// <param name="paramIn">参数1</param>
        /// <param name="options"></param>
        public void AsyncRunWithCallBack<ParamIn, ParamOut>(Func<ParamIn, ParamOut> asyncMethod, Action<ParamOut> callbackMethod, ParamIn paramIn, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        where ParamOut : class
        {
            _factory.StartNew<ParamOut>(() =>
            {
                try
                {
                    return asyncMethod(paramIn);
                }
                catch (Exception ex)
                {
                    TaskLog.LogExcepion("在执行异步回调任务时，执行异步任务出错", ex);
                    throw ex;
                }
            })
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    try
                    {
                        _context.Send(new SendOrPostCallback(p =>
                        {
                            ParamOut result = p as ParamOut;
                            callbackMethod(result);
                        }), task.Result);
                    }
                    catch (Exception ex)
                    {
                        TaskLog.LogExcepion("在执行异步回调任务时，执行回调任务出错", ex);
                    }
                }
            });
        }

        ///// <summary>
        ///// 执行一个异步操作，并在该异步操作执行结束后，将结果回调到UI函数处理。
        ///// 注意：在执行同步任务时，需要先判断入参是否为空！
        ///// </summary>
        ///// <typeparam name="ParamIn">参数1类型</typeparam>
        ///// <typeparam name="ParamOut">参数2类型</typeparam>
        ///// <param name="asyncMethod">异步操作</param>
        ///// <param name="callbackMethod">UI回调函数</param>
        ///// <param name="paramIn">参数1</param>
        ///// <param name="paramOut">参数2</param>
        ///// <param name="options"></param>
        //public void AsyncRunWithCallBack<Param>(Action<Param> asyncMethod, Action callbackMethod, Param paramIn, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        //{
        //    _factory.StartNew(() =>
        //    {
        //        try
        //        {
        //            asyncMethod(paramIn);
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskLog.LogExcepion("在执行异步回调任务时，执行异步任务出错", ex);
        //            throw ex;
        //        }
        //    })
        //    .ContinueWith(task =>
        //    {
        //        if (task.IsCompleted)
        //        {
        //            try
        //            {
        //                callbackMethod();
        //            }
        //            catch (Exception ex)
        //            {
        //                TaskLog.LogExcepion("在执行异步回调任务时，执行回调任务出错", ex);
        //            }
        //        }
        //    });
        //}

        ///// <summary>
        ///// 执行一个异步操作，并在该异步操作执行结束后，将结果回调到UI函数处理。
        ///// 注意：在执行同步任务时，需要先判断入参是否为空！
        ///// </summary>
        ///// <typeparam name="ParamIn">参数1类型</typeparam>
        ///// <typeparam name="ParamOut">参数2类型</typeparam>
        ///// <param name="asyncMethod">异步操作</param>
        ///// <param name="callbackMethod">UI回调函数</param>
        ///// <param name="paramIn">参数1</param>
        ///// <param name="paramOut">参数2</param>
        ///// <param name="options"></param>
        //public void AsyncRunWithCallBack<Param>(Action asyncMethod, Action callbackMethod, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        //{
        //    _factory.StartNew(() =>
        //    {
        //        try
        //        {
        //            asyncMethod();
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskLog.LogExcepion("在执行异步回调任务时，执行异步任务出错", ex);
        //            throw ex;
        //        }
        //    })
        //    .ContinueWith(task =>
        //    {
        //        if (task.IsCompleted)
        //        {
        //            try
        //            {
        //                callbackMethod();
        //            }
        //            catch (Exception ex)
        //            {
        //                TaskLog.LogExcepion("在执行异步回调任务时，执行回调任务出错", ex);
        //            }
        //        }
        //    });
        //}
    }
}
