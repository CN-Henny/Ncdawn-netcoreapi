using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ncdawn.Core.Config
{
    public class CPlatformConfigurationSource : FileConfigurationSource
    {
        public string ConfigurationKeyPrefix { get; set; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new CPlatformConfigurationProvider(this);
        }
    }

    public class CPlatformConfigurationProvider : FileConfigurationProvider
    {

        public CPlatformConfigurationProvider(CPlatformConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new JsonConfigurationParser();
            this.Data = parser.Parse(stream, null);
        }
    }


}