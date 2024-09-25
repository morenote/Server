using Microsoft.AspNetCore.Builder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morenote.Framework.Middleware.DigitalEnvelopeMd
{
    public static class DigitalEnvelopeMiddlewareExtensions
    {
        public static IApplicationBuilder UseDigitalEnvelope( this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DigitalEnvelopeMiddleware>();
        }
    }
    
}
