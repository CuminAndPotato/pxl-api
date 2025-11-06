# Requirements

- dotnet 8 SDK
- CS-Script VSCode extension


# CS-Script VSCode extension integration

The extension requires CS-Script tools to function properly.
This is the most reliable way of managing their updates independently from the extension releases (e.g. update with new .NET version).

## Installation

1. Install/Update tools
    - Script engine: `dotnet tool install --global cs-script.cli`
    - Syntaxer: `dotnet tool install --global cs-syntaxer`

2. Configure tools
    Execute extension command "CS-Script: Detect and integrate CS-Script"

Note: you need to have .NET SDK installed for using CS-Script (see https://dotnet.microsoft.com/en-us/download)
