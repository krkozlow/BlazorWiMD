using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Hub
{
    public class UserConnectionMapping
    {
        public UserConnection User { get; set; }
        public IList<UserConnection> ListeningUsers { get; set; }
    }
}
