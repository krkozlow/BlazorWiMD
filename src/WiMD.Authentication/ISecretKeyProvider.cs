using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface ISecretKeyProvider
    {
        byte[] GetSecretKey();
    }
}
