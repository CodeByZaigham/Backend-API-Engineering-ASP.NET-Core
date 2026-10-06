# Backend-API-Engineering-ASP.NET-Core
Production-oriented ASP.NET Core Web API implementing scalable backend architecture with Entity Framework Core, SQL Server, authentication, middleware, dependency injection, caching, validation, and RESTful API design.

under development

## Prerequisites
- .NET SDK 10.0.4
- SQL Server running locally

## Setup
1. git clone <repo-url> && cd "shopforge backend"
2. dotnet restore
2. dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your connection string>"
3. dotnet tool install --global dotnet-ef
4. dotnet ef database update
5. dotnet watch run
