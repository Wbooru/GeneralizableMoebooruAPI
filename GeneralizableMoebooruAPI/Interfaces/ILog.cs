using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralizableMoebooruAPI.Interfaces
{
    public interface ILog
    {
        void Error(string message, [CallerMemberName]string method = "unknown method");
        void Warn(string message, [CallerMemberName]string method = "unknown method");
        void Info(string message, [CallerMemberName]string method = "unknown method");
        void Debug(string message, [CallerMemberName]string method = "unknown method");
    }

    public class LogDefaultImplement : ILog
    {
        public static ILog Default { get; } = new LogDefaultImplement();

        private void Output(string message,string method,string log_type)
        {
            message = $"[{DateTime.Now.ToString()} {log_type}]{method}: {message} {Environment.NewLine}";

            Console.WriteLine(message);
        }

        public void Debug(string message, [CallerMemberName] string method = "unknown method") => Output(message, method, "DEBUG");
        
        public void Error(string message, [CallerMemberName] string method = "unknown method") => Output(message, method, "ERROR");

        public void Info(string message, [CallerMemberName] string method = "unknown method") => Output(message, method, "INFO");

        public void Warn(string message, [CallerMemberName] string method = "unknown method") => Output(message, method, "WARN");
    }
}
