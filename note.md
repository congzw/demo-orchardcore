# some notes

## modules orders

```sh

# module layers
AfterAllModulesLoad: 10000
------------------
App: 1001 ~ 2000
------------------
Base: 1 ~ 1000
------------------
BeforeAllModulesLoad: -10000

# ordery demo
NbSites.ApiDoc
NbSites.App.Portal
NbSites.App.Setup
NbSites.Base
NbSites.Jobs -1
NbSites.Core -2

# order from small to big
Before -> Basic -> App -> After

```

�����ԣ�

- Manifest.cs��DependenciesӰ����ǹ��캯��Ctor()
- Startup.cs��OrderӰ�����ConfigureServices()
- Startup.cs��ConfigureOrderӰ�����Configure()

## build scripts

�����ֶ��ű��Ĳ��������ÿ���������ã������쳣ҳ��ȣ��� /p:EnvironmentName=Development

