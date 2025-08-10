Flights API (.NET 8)
A .NET 8 Web API using EF Core (Code-First) + SQLite, FluentValidation, AutoMapper, and SignalR. 

Prerequisites
.NET 8 SDK
Git
SQLite
Running on vs-code ide with C# extensions installed

clone the repository

EF Core CLI for migrations:

install ef migrations tool:
dotnet tool install --global dotnet-ef

create the db:
dotnet ef database update

run the project:
dotnet watch run

run the tests:
dotnet test "tests/Api.UnitTests/Api.UnitTests.csproj" -v minimal
