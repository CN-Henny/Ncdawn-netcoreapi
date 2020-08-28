using System;

namespace Ncdawn.Core.SqlSugor
{
    public class ConfigMethodAttr : Attribute
    {
        public string Name { set; get; }
        public ConfigMethodAttr(string name)
        {
            Name = name;
        }
    }
    public class ConfigIOAttr : Attribute
    {
        public string Name { set; get; }
        public ConfigIOAttr(string name)
        {
            Name = name;
        }
    }

    public interface IConfigIO
    {
        void ReadConfig(string text);
        void WriteLog();
    }
}
