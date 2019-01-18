using System;
using System.Collections.Generic;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.GeolocationContext.Domain.Model
{
    public class UserConnection : ValueObject
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
}
