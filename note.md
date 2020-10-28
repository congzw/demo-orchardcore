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

# order from small to big
Before -> Base -> App -> After

```
