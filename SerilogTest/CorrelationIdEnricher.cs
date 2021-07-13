using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogTest
{
    public class CorrelationIdEnricher:ILogEventEnricher
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdEnricher()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory){

            if(logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }

            var httpContext = _httpContextAccessor.HttpContext;

            if(httpContext == null)
            {
                return;
            }

            if(httpContext.Request == null)
            {
                return;
            }

            string correlationId = httpContext.Request.Headers["correlationId"];

            if(string.IsNullOrWhiteSpace(correlationId))
            {
                return;
            }

            var correlationProperty = new LogEventProperty("correlationId", new ScalarValue("correlationId"));
            logEvent.AddPropertyIfAbsent(correlationProperty);
        }
    }
}
