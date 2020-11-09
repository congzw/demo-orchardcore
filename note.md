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

经测试：

- Manifest.cs的Dependencies影响的是构造函数Ctor()
- Startup.cs的Order影响的是ConfigureServices()
- Startup.cs的ConfigureOrder影响的是Configure()

## build scripts

增加手动脚本的参数，启用开发相关设置（例如异常页面等）： /p:EnvironmentName=Development

