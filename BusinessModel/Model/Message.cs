using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessModel
{
    public enum MessageState { Read, NotRead }

    public class Message : BaseEntity
    {
        public Message()
        {
        }

        public Message(Account from, Account to, string theme, string message)
        {
            From = from;
            To = to;
            Theme = theme;
            Text = message;
        }

        public Account From { get; set; }
        public Account To { get; set; }
        public MessageState Status { get; set; } = MessageState.NotRead;
        public string Theme { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        public string Description{ get { return Text.Length > 60 ? Text.Substring(0, 60) : Text; } }
    }
}
