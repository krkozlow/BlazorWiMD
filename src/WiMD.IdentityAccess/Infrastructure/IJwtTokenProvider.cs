using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.IdentityAccess.Infrastructure
{
    public interface IJwtTokenProvider
    {
        string CreateJwtToken(string email);
    }
}
