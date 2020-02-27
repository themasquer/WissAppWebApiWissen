using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WissAppWebApi.Models
{
    public class UsersMessagesModel
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int? ReceiverId { get; set; }

        public int MessageId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }
    }
}