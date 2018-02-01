using System;

namespace BusinessModel
{
    public enum MessageState { Read, NotRead }

    public class Message : BaseEntity
    {
        public Message()
        {
        }

        public Message(Account from, Account to, MessageState status, string theme, string message, DateTime sendDate)
        {
            From = from;
            To = to;
            Status = status;
            Theme = theme;
            Text = message;
            SendDate = sendDate;
        }

        public Guid? From_id { get; set; }
        public Guid? To_id { get; set; }
        public Account From { get; set; }
        public Account To { get; set; }
        public MessageState Status { get; set; }
        public string Theme { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public string Description{ get { return Text.Length > 60 ? Text.Substring(0, 60) : Text; } }
    }
}
