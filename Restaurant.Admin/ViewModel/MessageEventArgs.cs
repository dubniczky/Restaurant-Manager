using System;

namespace Restaurant.Admin.ViewModel
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MessageEventArgs()
        {
            Message = "";
        }
        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
