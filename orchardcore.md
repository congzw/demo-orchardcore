# OrchardCore的路径


[入门步骤](https://docs.orchardcore.net/en/dev/docs/guides/)


```sh

#Installing the Orchard CMS templates
dotnet new -i OrchardCore.ProjectTemplates::1.0.0-rc2-*
#To use the development branch of the template
dotnet new -i OrchardCore.ProjectTemplates::1.0.0-rc2-* --nuget-source https://nuget.cloudsmith.io/orchardcore/preview/v3/index.json  

# Generate an Orchard Cms Web Application¶ 
dotnet new occms  

#Generate a modular ASP.NET MVC Core Web Application
dotnet new ocmvc

```

This will allow for the Razor Pages to be reloaded without the need to recompile them.
``` xml

<PropertyGroup>
  <TargetFramework>netcoreapp3.1</TargetFramework>
  <PreserveCompilationReferences>true</PreserveCompilationReferences>
</PropertyGroup>

```

## concepts of Orchard Core CMS

- Content Type: 内容Class
- Content Item: 内容Instance 
- Content Part：LEGO组件
- Content Field: 


