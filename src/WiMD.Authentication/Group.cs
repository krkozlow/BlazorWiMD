using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}
