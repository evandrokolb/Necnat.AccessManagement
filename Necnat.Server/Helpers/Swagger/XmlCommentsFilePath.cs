using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Reflection;

namespace Necnat.Server.Helpers.Swagger
{
    public static class XmlCommentsFilePath
    {
        public static string Get(Type startupType)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = startupType.GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
