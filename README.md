# SpectreConsoleAppTemplate

SpectreConsoleAppTemplate is a .NET 6 console app template that features:

* [Spectre.Console](https://spectreconsole.net/) [GitGub](https://github.com/spectreconsole/spectre.console)
  * library that makes it easier to create beautiful console applications
* Use of .NET's [Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host)
  * Hosts encapsulate the app's resources and lifetime functionality
  * Dependency injection (DI)
  * Logging
  * Configuration and AppSettings
  * App Shutdown
  * `IHostedService` implementations
* Hidden by default `-w`,`--wait-for-debugger` option what waits for Visual Studio's debugger to attach before continuing execution
* `HelloWorldCommand` and `ShowCurrentDateTimeCommand` that demonstrate various capabilities
* comments and helpful links to additional resources within code

## Install Template

1. Clone the code

    ```
    git clone https://github.com/theperiscope/SpectreConsoleAppTemplate.git
    ```

2. `CD` into folder where template code was cloned
3. Install the template

    ```
    dotnet new install .
    ```

## Reinstall Template

1. Clone the code

    ```
    git clone https://github.com/theperiscope/SpectreConsoleAppTemplate.git
    ```

2. `CD` into folder where template code was cloned
3. Install the template

    ```
    dotnet new install . --force
    ```

## Create New Project

```
mkdir newproject
cd newproject
dotnet new spectreConsoleApp
```

## Uninstall Template

1. `CD` into folder where template code was cloned
2. Install the template

    ```
    dotnet new uninstall .
    ```

## Check If Template Is Installed

```
dotnet new list | find "spectre"
```

## Publishing

> https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli

The `/p:CopyOutputSymbolsToPublishDirectory=false` is necessary to prevent .pdb file from being published.

```
dotnet publish -c Release -f net7.0 -r win10-x64 --self-contained true /p:CopyOutputSymbolsToPublishDirectory=false
```