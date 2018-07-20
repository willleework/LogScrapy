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
    public class TaskManage : ITaskMange
    {
        SynchronizationContext _context = null;
        /// <summary>
        /// 主线程（UI）同步上下文
        /// </summary>
        public SynchronizationContext Context { get => _context; }

        TaskFactory _factory = null;

        /// <summary>
        /// 任务调度分组字典，以时间间隔分组
        /// </summary>
        private Dictionary<int, List<Action>> _scheduleTasks = new Dictionary<int, List<Action>>();
        private List<Timer> _timers = new List<Timer>();
        private Dictionary<int, Timer> _timerDic = new Dictionary<int, Timer>();

        ReaderWriterLockSlim _sheduleLock = new ReaderWriterLockSlim();

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
        /// 如果是耗时操作，请指定option参数为LongRunning
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

        /// <summary>
        /// 执行一个异步操作，并在该异步操作执行结束后，将结果回调到UI函数处理。
        /// 注意：在执行同步任务时，需要先判断入参是否为空！
        /// </summary>
        /// <typeparam name="ParamIn">参数1类型</typeparam>
        /// <typeparam name="ParamOut">参数2类型</typeparam>
        /// <param name="asyncMethod">异步操作</param>
        /// <param name="callbackMethod">UI回调函数</param>
        /// <param name="paramIn">参数1</param>
        /// <param name="paramOut">参数2</param>
        /// <param name="options"></param>
        public void AsyncRunWithCallBack<Param>(Action<Param> asyncMethod, Action callbackMethod, Param paramIn, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        {
            _factory.StartNew(() =>
            {
                try
                {
                    asyncMethod(paramIn);
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
                            callbackMethod();
                        }), task);
                    }
                    catch (Exception ex)
                    {
                        TaskLog.LogExcepion("在执行异步回调任务时，执行回调任务出错", ex);
                    }
                }
            });
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
        /// <param name="paramOut">参数2</param>
        /// <param name="options"></param>
        public void AsyncRunWithCallBack<Param>(Action asyncMethod, Action callbackMethod, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        {
            _factory.StartNew(() =>
            {
                try
                {
                    asyncMethod();
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
                            callbackMethod();
                        }), task);
                    }
                    catch (Exception ex)
                    {
                        TaskLog.LogExcepion("在执行异步回调任务时，执行回调任务出错", ex);
                    }
                }
            });
        }

        /// <summary>
        /// 任务调度（待测试方法）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">任务</param>
        /// <param name="durations">调度间隔</param>
        /// <param name="options">调度参数</param>
        public void RegisterScheduleTask(Action method, int durations, TaskCreationOptions options = TaskCreationOptions.PreferFairness)
        {
            try
            {
                bool addNewSchedule = false;
                _sheduleLock.EnterUpgradeableReadLock();
                if (!_scheduleTasks.ContainsKey(durations))
                {
                    _sheduleLock.EnterWriteLock();
                    if (!_timerDic.ContainsKey(durations))
                    {
                        Timer timer = new Timer(p => { RunScheduleTasks((int)p); }, durations, Timeout.Infinite, durations);
                        _timerDic[durations] = timer;
                        addNewSchedule = true;
                    }
                    _scheduleTasks.Add(durations, new List<Action>());
                }
                else
                {
                    _sheduleLock.EnterWriteLock();
                }
                _scheduleTasks[durations].Add(method);
                //启动计时器
                if (addNewSchedule)
                {
                    _timerDic[durations].Change(0, durations);
                }
            }
            catch (Exception ex)
            {
                TaskLog.LogExcepion("任务管理器注册调度任务失败", ex);
            }
            finally
            {
                _sheduleLock.ExitWriteLock();
                _sheduleLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 注销任务调度（待测试方法）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="durations"></param>
        public void UnRegisterScheduleTask(Action method, int durations)
        {
            _sheduleLock.EnterUpgradeableReadLock();
            try
            {
                if (_scheduleTasks.ContainsKey(durations) && _scheduleTasks[durations].Count > 0)
                {
                    _sheduleLock.EnterWriteLock();
                    _scheduleTasks[durations].Remove(method);//TODO: 此处如果抛出异常，会导致锁退出异常
                    _sheduleLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                TaskLog.LogExcepion("任务管理器注销调度任务失败", ex);
            }
            finally
            {
                _sheduleLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 执行任务队列
        /// </summary>
        /// <param name="dutations"></param>
        private void RunScheduleTasks(int dutations)
        {
            _sheduleLock.EnterUpgradeableReadLock();
            if (!_scheduleTasks.ContainsKey(dutations) || _scheduleTasks[dutations].Count <= 0)
            {
                CheckTaskGroupValid(dutations);
                TaskLog.LogInfo(string.Format("任务管理器没有发现调度频度【{0}ms】的任务队列，本次执行直接退出", dutations));
            }
            try
            {
                List<Action> tasks = new List<Action>();
                tasks.AddRange(_scheduleTasks[dutations]);
                foreach (Action task in tasks)
                {
                    try
                    {
                        //TODO：此处将任务推送到线程池队列中，如果是耗时操作，会导致线程池资源耗尽
                        //应该将调度任务封装，区分耗时操作和简单操作（外部属性和内部检测），UI线程和工作者线程
                        _factory.StartNew(() => { task(); });
                    }
                    catch (Exception ex)
                    {
                        TaskLog.LogExcepion(string.Format("任务调度器执行任务失败，任务名称【{0}】，分组【{1}】", task.Method.Name, dutations), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                TaskLog.LogExcepion(string.Format("执行任务队列出错，分组【{0}】", dutations), ex);
            }
            finally
            {
                _sheduleLock.EnterUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 检查任务组有效性
        /// </summary>
        /// <param name="duration"></param>
        private void CheckTaskGroupValid(int duration)
        {
            _sheduleLock.EnterWriteLock();
            try
            {
                if (_scheduleTasks.ContainsKey(duration) || _scheduleTasks[duration].Count <= 0)
                {
                    if (_timerDic.ContainsKey(duration))
                    {
                        //关闭计时器
                        Timer timer = _timerDic[duration];

                        timer.Change(Timeout.Infinite, duration);
                        timer.Dispose();
                        _timerDic.Remove(duration);
                        _timers.Remove(timer);
                    }
                }
            }
            catch (Exception ex)
            {
                TaskLog.LogExcepion("检查任务组有效性", ex);
            }
            finally
            {
                _sheduleLock.ExitReadLock();
            }
        }
    }
}
