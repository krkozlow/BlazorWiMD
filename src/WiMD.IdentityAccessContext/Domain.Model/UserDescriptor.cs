using System;
using System.Collections.Generic;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.IdentityAccess.Domain.Model
{
    public class UserDescriptor : ValueObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
