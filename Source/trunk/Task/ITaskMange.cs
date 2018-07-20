﻿using System;
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
    public interface ITaskMange
    {
        /// <summary>
        /// 初始化任务管理器
        /// 必须在UI线程执行
        /// </summary>
        /// <param name="context">UI线程同步上下文</param>
        /// <param name="exceptionHandler">异常处理</param>
        void Init(SynchronizationContext context);

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        void RunTask<T>(Action<T> method, T param, TaskCreationOptions options = TaskCreationOptions.PreferFairness);

        /// <summary>
        /// 同步到UI线程执行
        /// 不等待执行完成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        void InvokeUIByPost<T>(Action<T> method, T param) where T : class;

        /// <summary>
        /// 同步到UI线程执行
        /// 等待执行完成后返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        void InvokeUIBySend<T>(Action<T> method, T param) where T : class;

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
        void AsyncRunWithCallBack<ParamIn, ParamOut>(Func<ParamIn, ParamOut> asyncMethod, Action<ParamOut> callbackMethod, ParamIn paramIn, TaskCreationOptions options = TaskCreationOptions.PreferFairness) where ParamOut : class;

        /// <summary>
        /// 执行一个异步操作，并在该异步操作执行结束后，将结果回调到UI函数处理。
        /// 注意：在执行同步任务时，需要先判断入参是否为空！
        /// </summary>
        /// <typeparam name="Param">参数1类型</typeparam>
        /// <param name="asyncMethod">异步操作</param>
        /// <param name="callbackMethod">UI回调函数</param>
        /// <param name="paramIn">参数1</param>
        /// <param name="options"></param>
        void AsyncRunWithCallBack<Param>(Action<Param> asyncMethod, Action callbackMethod, Param paramIn, TaskCreationOptions options = TaskCreationOptions.PreferFairness);

        /// <summary>
        /// 执行一个异步操作，并在该异步操作执行结束后，将结果回调到UI函数处理。
        /// 注意：在执行同步任务时，需要先判断入参是否为空！
        /// </summary>
        /// <typeparam name="Param">参数1类型</typeparam>
        /// <param name="asyncMethod">异步操作</param>
        /// <param name="callbackMethod">UI回调函数</param>
        /// <param name="options"></param>
        void AsyncRunWithCallBack<Param>(Action asyncMethod, Action callbackMethod, TaskCreationOptions options = TaskCreationOptions.PreferFairness);

        /// <summary>
        /// 注册定时任务
        /// </summary>
        /// <param name="method">定时任务</param>
        /// <param name="durations">执行间隔时间，作为内部分组依据</param>
        /// <param name="options">任务参数，耗时操作需在此参数指明，否则有可能造成任务调度被耗时操作阻塞</param>
        void RegisterScheduleTask(Action method, int durations, TaskCreationOptions options = TaskCreationOptions.PreferFairness);

        /// <summary>
        /// 取消注册任务
        /// </summary>
        /// <param name="method">取消的任务</param>
        /// <param name="durations">定时时间，作为内部分组依据</param>
        void UnRegisterScheduleTask(Action method, int durations);
    }
}
