using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WissAppMvc.Models
{
    public class MessagesModel
    {
        public string Message { get; set; }
        public string Date { get; set; }
        public bool Sent { get; set; }
        public string User { get; set; }
    }
}