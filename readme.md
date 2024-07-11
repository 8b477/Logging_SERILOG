# Setup de Serilog en Quelques Minutes ??

Ce guide vous expliquera comment configurer **Serilog** dans votre projet ASP.NET Core en quelques �tapes simples. Avec Serilog, vous pouvez facilement g�rer et personnaliser vos journaux de log.

## ?? Packages � Installer

Commencez par installer les trois packages suivants :

```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console
```


# ?? Configuration de appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log-.txt",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true,
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
```

# ?? Configuration de Program.cs
Mettez en place le setup de Serilog dans votre fichier Program.cs :

```c#
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging(); // <-----

try
{
    // Journalisez un message d'info
    Log.Information("Starting web application");

    // Ex�cutez l'application
    app.Run();
}
catch (Exception ex)
{
    // Journalisez un message "Fatal" avec l'exception
    Log.Fatal(ex, "Application � planter");
}
finally
{
    // Fermez et videz le logger SERILOG pour s'assurer que tous les logs
    Log.CloseAndFlush();
}
```

# ?? Cr�ation d'Endpoints Minimal API pour la D�mo
Ajoutez deux endpoints dans votre application pour tester la journalisation :

```c#
app.MapGet("/", () => "Hello World!");
app.MapGet("/error", () => throw new Exception("Simulated exception"));
```

# ?? D�sactivation du Lancement Automatique du Navigateur
Pour d�sactiver le lancement automatique du navigateur, modifiez le fichier launchSettings.json :
Switch cette ligne sur false : `"launchBrowser": false,`

Code complet : 

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:12707",
      "sslPort": 0
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "launchUrl": "weatherforecast",
      "applicationUrl": "http://localhost:5091",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": false,
      "launchUrl": "weatherforecast",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

# ?? Voil� !
Vous avez maintenant configur� Serilog dans votre projet ASP.NET Core.
Vous pouvez tester la journalisation en acc�dant aux endpoints d�finis ici `Logging_SERILOG.http` et v�rifier les fichiers de log g�n�r�s dans le r�pertoire Logs.


