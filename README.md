# ASP.NET Core Starter Template

This is my starter template for ASP.Net core.
It should contain everything you need to start creating a web API.

## Contents
This Template should cover the majority of what you need for a Web API
project. Included is:

- Code First Database Context via Entity Framework
- Basic MVC controller structure
- Custom auth middleware starter
- Health checks middleware
- Email sending service with Razor templating
- Health service for graceful shutdown and startup
- Cryptography utils required for password management
- Razor pages
- General manageable folder structure

## Getting Started

Pull the repo and with the dotnet SDK installed run:
```
dotnet new -i <location of the repo folder here>
```

The Template will then be installed locally on your PC.
To then use the template select it in your IDE or run `dotnet new aspCoreStarter`
in the terminal in the location you want to create the project.
Please note that is will create the solution and project files in
the same directory as where you call the command, so create
your project folder first.
