using System;
using System.Collections.Generic;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.IdentityAccess.Domain.Model
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}