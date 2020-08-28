using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Ncdawn.Core.Utils.ConfigUtils;
using System.IO;

namespace Ncdawn.Core.Config
{
    public static class CacheConfigurationExtensionsstatic
    {
        public static IConfigurationBuilder AddConfigFile(this IConfigurationBuilder builder, string path)
        {
            return AddConfigFile(builder, provider: null, path: path, basePath: null, optional: false, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddConfigFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddConfigFile(builder, provider: null, path: path, basePath: null, optional: optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddConfigFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {

            return AddConfigFile(builder, provider: null, path: path, basePath: null, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddConfigFile(this IConfigurationBuilder builder, string path, string basePath, bool optional, bool reloadOnChange)
        {
            return AddConfigFile(builder, provider: null, path: path, basePath: basePath, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddConfigFile(this IConfigurationBuilder builder, IFileProvider provider, string path, string basePath, bool optional, bool reloadOnChange)
        {
            Check.NotNull(builder, "builder");
            Check.CheckCondition(() => string.IsNullOrEmpty(path), "path");
            path = EnvironmentHelper.GetEnvironmentVariable(path);
            if (File.Exists(path))
            {
                if (provider == null && Path.IsPathRooted(path))
                {
                    provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
                    path = Path.GetFileName(path);
                }
                var source = new CPlatformConfigurationSource
                {
                    FileProvider = provider,
                    Path = path,
                    Optional = optional,
                    ReloadOnChange = reloadOnChange
                };
                builder.Add(source);
                if (!string.IsNullOrEmpty(basePath))
                    builder.SetBasePath(basePath);
                AppConfig.Configuration = builder.Build();
            }
            return builder;
        }
    }
}