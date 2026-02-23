#!/bin/bash
set -e

# Install sl (steam locomotive) for fun
sudo apt-get update -y
sudo apt-get install sl -y

# Restore .NET project dependencies
dotnet restore src/EmployeeApp/EmployeeApp.csproj
