using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeHandler.Notifications
{
    public delegate void OfficeHandlerEventHandler(object sender, OfficeHandlerEventArgs e);

    public class OfficeHandlerEventArgs : EventArgs
    {
        public OfficeHandlerEventArgs(string s)
        {
            msg = s;
        }
        private string msg;
        public string Message
        {
            get { return msg; }
            set { msg = value; }
        }
    }
}
