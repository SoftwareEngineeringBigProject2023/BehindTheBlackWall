using Google.Api;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.Applications
{
    public static class JwtSecurityTokenHandlerExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Claim GetNameIdentifier(this JwtSecurityTokenHandler handler, Metadata metadata)
        {
            var bearer = metadata.GetValue("Authorization");
            if (bearer is null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Cannot find jwt token."));

            var jwtToken = bearer.Substring(6, bearer.Length - 6).Trim();
            var token = handler.ReadJwtToken(jwtToken);
            var id = token.Claims.FirstOrDefault(item => item.Type == ClaimTypes.NameIdentifier);

            if (id is null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Cannot find name identifier claim."));

            return id;
        }
    }
}
