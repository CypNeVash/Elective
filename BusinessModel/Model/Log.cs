using System;

namespace BusinessModel
{
    public enum LogStatus { info, error, warning }

    /// <summary>
    /// Entity that stores changes in the application
    /// </summary>
    public class Log : BaseEntity
    {
        public Log()
        {
        }

        public Log(string controller, string action, LogStatus status, string message, string name)
        {
            Controller = controller;
            Action = action;
            Status = status;
            Message = message;
            Name = name;
        }

        public string Controller { get; set; }

        public string Action { get; set; }

        public LogStatus Status { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Name { get; set; }
    }
}
