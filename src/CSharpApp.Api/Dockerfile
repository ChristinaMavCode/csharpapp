# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY CSharpApp.Api/CSharpApp.Api.csproj CSharpApp.Api/
COPY CSharpApp.Application/CSharpApp.Application.csproj CSharpApp.Application/
COPY CSharpApp.Core/CSharpApp.Core.csproj CSharpApp.Core/
COPY CSharpApp.Infrastructure/CSharpApp.Infrastructure.csproj CSharpApp.Infrastructure/
COPY CSharpApp.UnitTests/CSharpApp.UnitTests.csproj CSharpApp.UnitTests/

RUN dotnet restore

COPY . .
WORKDIR /src/CSharpApp.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "CSharpApp.Api.dll"]