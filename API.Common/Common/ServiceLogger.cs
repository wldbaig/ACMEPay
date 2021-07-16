using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Threading.Tasks;

namespace API.Common
{
    public interface IServiceLogger<T> where T : class
    {
        void Warning(string message);
        void Error(string message, Exception ex);
        void ErrorWriteToFile(string message, Exception ex);
        Task WriteToFileAsync(string message);
        Task FlushLogFileAsync();
    }

    public class ServiceLogger<T> : IServiceLogger<T> where T : class
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHostEnvironment env;
        public ServiceLogger(IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.env = env;
        }

        public void Warning(string message)
        {
            var logger = GetLoggerInstance();
            logger.Warning(message);
        }

        public void Error(string message, Exception ex)
        {
            var logger = GetLoggerInstance();
            logger.ForContext("InnerException", ex?.InnerException).Error(ex, message);
        }

        public void ErrorWriteToFile(string message, Exception ex)
        {
            string path = env.ContentRootFileProvider.GetFileInfo("wwwroot/private/log/log.txt")?.PhysicalPath;
            var log = new LoggerConfiguration().WriteTo.File(path: path, rollingInterval: RollingInterval.Day).CreateLogger();
            log.Error(ex, message);
        }

        public async Task WriteToFileAsync(string message)
        {
            var request = httpContextAccessor.HttpContext.Request;
            var username = request.HttpContext.User.Identity.IsAuthenticated ? request.HttpContext.User.Identity.Name : "Anonymous";
            string path = env.ContentRootFileProvider.GetFileInfo("wwwroot/private/log/log.txt")?.PhysicalPath;
            var finalMessage = string.Empty;

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                finalMessage += "DateTime :" + DateTime.UtcNow + Environment.NewLine;
                finalMessage += "Username :" + username + Environment.NewLine;
                finalMessage += message + Environment.NewLine;
                finalMessage += "--------------------------------------------------------------------------------------" + Environment.NewLine;

                await writer.WriteLineAsync(finalMessage);
            }
        }

        public async Task FlushLogFileAsync()
        {
            string path = env.ContentRootFileProvider.GetFileInfo("wwwroot/private/log/log.txt")?.PhysicalPath;

            await File.WriteAllTextAsync(path, string.Empty);
        }

        private ILogger GetLoggerInstance()
        {
            var request = httpContextAccessor.HttpContext.Request;

            var headerKeys = request.Headers?.Keys?.Select(k => k).ToList();
            List<string> headerKeyValueStrings = new List<string>();

            if (headerKeys != null && headerKeys.Count > 0)
            {
                foreach (var key in headerKeys)
                {
                    headerKeyValueStrings.Add($"{key}={request.Headers[key]}");
                }
            }

            // Get calling method name
            //var methodName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            var sourceContext = typeof(T).FullName;

            return Log
                    .ForContext("Date", DateTime.UtcNow)
                    .ForContext("Logger", sourceContext)
                    .ForContext("IPAddress", request?.HttpContext?.Connection?.RemoteIpAddress?.ToString())
                    .ForContext("UserName", request.HttpContext.User.Identity.IsAuthenticated ? request.HttpContext.User.Identity.Name : "Anonymous")
                    .ForContext("HttpHeader", string.Join("&", headerKeyValueStrings))
                    .ForContext("RequestMethod", request.Method.ToString())
                    .ForContext("Url", $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}")
                    .ForContext("Source", Enumeration.ErrorLogSource.API);
        }
    }
}
