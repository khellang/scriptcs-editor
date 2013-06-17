using System;

using Common.Logging;

namespace ScriptCsPad
{
    public class NullLogger : ILog
    {
        public void Trace(object message) { }

        public void Trace(object message, Exception exception) { }

        public void TraceFormat(string format, params object[] args) { }

        public void TraceFormat(string format, Exception exception, params object[] args) { }

        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args) { }

        public void TraceFormat(
            IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Trace(Action<FormatMessageHandler> formatMessageCallback) { }

        public void Trace(Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { }

        public void Trace(
            IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Debug(object message) { }

        public void Debug(object message, Exception exception) { }

        public void DebugFormat(string format, params object[] args) { }

        public void DebugFormat(string format, Exception exception, params object[] args) { }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args) { }

        public void DebugFormat(
            IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Debug(Action<FormatMessageHandler> formatMessageCallback) { }

        public void Debug(Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { }

        public void Debug(
            IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Info(object message) { }

        public void Info(object message, Exception exception) { }

        public void InfoFormat(string format, params object[] args) { }

        public void InfoFormat(string format, Exception exception, params object[] args) { }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args) { }

        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Info(Action<FormatMessageHandler> formatMessageCallback) { }

        public void Info(Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { }

        public void Info(
            IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Warn(object message) { }

        public void Warn(object message, Exception exception) { }

        public void WarnFormat(string format, params object[] args) { }

        public void WarnFormat(string format, Exception exception, params object[] args) { }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args) { }

        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Warn(Action<FormatMessageHandler> formatMessageCallback) { }

        public void Warn(Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { }

        public void Warn(
            IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Error(object message) { }

        public void Error(object message, Exception exception) { }

        public void ErrorFormat(string format, params object[] args) { }

        public void ErrorFormat(string format, Exception exception, params object[] args) { }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args) { }

        public void ErrorFormat(
            IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Error(Action<FormatMessageHandler> formatMessageCallback) { }

        public void Error(Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { }

        public void Error(
            IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Fatal(object message) { }

        public void Fatal(object message, Exception exception) { }

        public void FatalFormat(string format, params object[] args) { }

        public void FatalFormat(string format, Exception exception, params object[] args) { }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args) { }

        public void FatalFormat(
            IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Fatal(Action<FormatMessageHandler> formatMessageCallback) { }

        public void Fatal(Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { }

        public void Fatal(
            IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { }

        public bool IsTraceEnabled { get; private set; }

        public bool IsDebugEnabled { get; private set; }

        public bool IsErrorEnabled { get; private set; }

        public bool IsFatalEnabled { get; private set; }

        public bool IsInfoEnabled { get; private set; }

        public bool IsWarnEnabled { get; private set; }
    }
}