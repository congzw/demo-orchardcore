using System;

namespace NbSites.VersionInfos
{
    internal static class VersionInfoRegistry
    {
        internal static void Init(VersionInfo instance)
        {
            //初始化版本（0.1.* ~ 0.9.*）
            instance.AppendChangeLog("基本框架搭建", "", "0.1.0", new DateTime(2020, 10, 1))
                .With("增加版本信息：VersionInfo")
                .With("增加接口文档：ApiDoc")
                .With("增加模块化：Orchard")
                .With("增加自动注入：AutoInject")
                .With("增加自动任务：AutoTasks");

            instance.AppendChangeLog("数据库切换", "", "0.2.0", new DateTime(2020, 11, 1))
                .With("增加数据库支持：MySql")
                .With("增加数据库支持：SqlServer");

            instance.AppendChangeLog("后台任务", "", "0.3.0", new DateTime(2020, 11, 5))
                .With("增加后台任务：hangfire")
                .With("增加数据库字段长度的自动映射");
        }
    }
}