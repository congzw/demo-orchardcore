using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "NbSites.Base",
    Author = "congzw",
    Website = "https://github.com/congzw/demo-orchardcore",
    Version = "0.1.0",
    Description = "NbSites.Base",
    Category = "Demo",
    Dependencies = new string[] { "NbSites.Core" }
)]