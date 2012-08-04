﻿using System;
using System.Collections.Generic;

namespace Hawkeye.Logging
{
    /// <summary>
    /// Use this class to obtain an instance of a logging service.
    /// </summary>
    public static partial class LogManager
    {
        private static readonly ILogServiceFactory factory;

        /// <summary>
        /// Initializes the <see cref="LogManager"/> class.
        /// </summary>
        static LogManager()
        {
            //factory = new DebugLogServiceFactory();
            //factory = new log4net.Log4NetServiceFactory();            
            if (This.IsInitialized) factory = This.GetLogServiceFactory();
            else factory = new DebugLogger(null);
        }

        /// <summary>
        /// Gets a value indicating whether a textbox appender can be attached to this <c>LogManager</c>.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <c>LogManager</c> is text box attachable; otherwise, <c>false</c>.
        /// </value>
        public static bool CanAppendLogService
        {
            get { return factory is ILogServiceAppendable; }
        }

        /// <summary>
        /// Gets the default logger.
        /// </summary>
        /// <returns>An instance of an object implementing <see cref="ILogService"/>.</returns>
        public static ILogService GetLogger()
        {
            return GetLogger(null);
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to log.</typeparam>
        /// <param name="additionalData">The optional additional data.</param>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger<T>(IDictionary<string, object> additionalData = null)
        {
            return GetLogger(typeof(T), additionalData);
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <param name="type">The type for which to log.</param>
        /// <param name="additionalData">The optional additional data.</param>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger(Type type, IDictionary<string, object> additionalData = null)
        {
            return factory.GetLogger(type, additionalData);
        }

        /// <summary>
        /// Appends the specified log service to the log services produced by the inner log services factory.
        /// </summary>
        /// <param name="toAppend">To append.</param>
        /// <param name="additionalData">The additional data.</param>
        /// <returns>An implementation of <see cref="ILogLevelThresholdSelector"/> allowing to set a maximum log level to trace.</returns>
        public static ILogLevelThresholdSelector AppendLogService(ILogService toAppend, IDictionary<string, object> additionalData = null)
        {
            if (!CanAppendLogService) throw new NotSupportedException(
                "This function is only supported when using a logging framework providing an implementation of 'ILogServiceAppendable'.");
            if (toAppend == null) throw new ArgumentNullException("toAppend");

            return ((ILogServiceAppendable)factory).AppendLogService(toAppend, additionalData);
        }
    }
}
