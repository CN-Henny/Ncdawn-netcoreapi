using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ncdawn.Core.Common
{
    public class Help
    {
        //0.Nuget下載包
        //.Net Core 将默认DI改为Autofac
        //Install-Package Autofac.Configuration
        //Install-Package Autofac.Extensions.DependencyInjection
        //DBFirst开发步骤
        //Microsoft.EntityFrameworkCore
        //Microsoft.EntityFrameworkCore.Tools
        //Microsoft.EntityFrameworkCore.Design
        //Microsoft.EntityFrameworkCore.SqlServer
        //Microsoft.EntityFrameworkCore.SqlServer.Design


        //1.通过数据库将生成Model，调出【程序包管理控制台】，选择项目，输入以下命令 将数据库中所有表生成Model
        //Scaffold-DbContext "Server=120.27.16.5; Database=AppsDBDCE;Persist Security Info=True;User ID = sa; password=1qazXSW@;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
        //命令解析： 
        //Scaffold-DbContext “数据库连接字符串” Microsoft.EntityFrameworkCore.SqlServer -OutputDir 输出的文件夹, 如无此参将生成到项目根目录
        //如果只想生成某些表将使用Tables参数 如: -Tables(“table1”,”table2”,”table3”,”table4”)
        //其他参数可输入 Scaffold-DbContext -? 查看帮助.

        //postgresql数据库
        //1、安装包
        //Npgsql
        //Npgsql.EntityFrameworkCore.PostgreSQL
        //Npgsql.EntityFrameworkCore.PostgreSQL.Design
        //2、执行语句
        //PostGreSQL得时候：Scaffold-DbContext "Server=localhost;Database=postgresqlBase;User ID=postgres;Password=1qazXSW@;" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir PgSqlModels

    }
}
