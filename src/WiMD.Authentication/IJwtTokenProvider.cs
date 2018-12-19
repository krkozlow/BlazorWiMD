using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface IJwtTokenProvider
    {
        string CreateJwtToken(string email);
    }
}
