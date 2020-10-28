# 数据库迁移说明

## 模板

```sh

PM> Add-Migration {迁移标识} -Project {项目名称} -StartupProject {启动项目名称}  -o {迁移代码存放的目标文件夹} 
PM> Remove-Migration -Project {项目名称} （To undo this action, use Remove-Migration）
PM> Update-Database -Project NbSites.Migrations

```

## 开发期间常用命令

```sh

Add-Migration Init -Project NbSites.Migrations -StartupProject NbSites.Migrations
Update-Database -Project NbSites.Migrations -StartupProject NbSites.Migrations
```