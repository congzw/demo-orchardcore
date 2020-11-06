using System;

namespace NbSites.VersionInfos
{
    internal static class VersionInfoRegistry
    {
        internal static void Init(VersionInfo instance)
        {
            //��ʼ���汾��0.1.* ~ 0.9.*��
            instance.AppendChangeLog("������ܴ", "", "0.1.0", new DateTime(2020, 10, 1))
                .With("���Ӱ汾��Ϣ��VersionInfo")
                .With("���ӽӿ��ĵ���ApiDoc")
                .With("����ģ�黯��Orchard")
                .With("�����Զ�ע�룺AutoInject")
                .With("�����Զ�����AutoTasks");

            instance.AppendChangeLog("���ݿ��л�", "", "0.2.0", new DateTime(2020, 11, 1))
                .With("�������ݿ�֧�֣�MySql")
                .With("�������ݿ�֧�֣�SqlServer");

            instance.AppendChangeLog("��̨����", "", "0.3.0", new DateTime(2020, 11, 5))
                .With("���Ӻ�̨����hangfire")
                .With("�������ݿ��ֶγ��ȵ��Զ�ӳ��");
        }
    }
}