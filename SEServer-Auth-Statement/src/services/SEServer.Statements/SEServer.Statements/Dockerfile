#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/SEServer.Statements/SEServer.Statements/SEServer.Statements.csproj", "src/services/SEServer.Statements/SEServer.Statements/"]
COPY ["src/modules/ModuleDistributor.Dapr.Configuration/ModuleDistributor.Dapr.Configuration.csproj", "src/modules/ModuleDistributor.Dapr.Configuration/"]
COPY ["src/modules/ModuleDistributor.Dapr/ModuleDistributor.Dapr.csproj", "src/modules/ModuleDistributor.Dapr/"]
COPY ["src/modules/ModuleDistributor/ModuleDistributor.csproj", "src/modules/ModuleDistributor/"]
COPY ["src/modules/ModuleDistributor.HealthCheck.Dapr/ModuleDistributor.HealthCheck.Dapr.csproj", "src/modules/ModuleDistributor.HealthCheck.Dapr/"]
COPY ["src/modules/ModuleDistributor.HealthCheck/ModuleDistributor.HealthCheck.csproj", "src/modules/ModuleDistributor.HealthCheck/"]
COPY ["src/modules/ModuleDistributor.Logging/ModuleDistributor.Logging.csproj", "src/modules/ModuleDistributor.Logging/"]
COPY ["src/modules/ModuleDistributor.Serilog/ModuleDistributor.Serilog.csproj", "src/modules/ModuleDistributor.Serilog/"]
COPY ["src/services/SEServer.Statements/SEServer.Statements.GrpcServices/SEServer.Statements.GrpcServices.csproj", "src/services/SEServer.Statements/SEServer.Statements.GrpcServices/"]
COPY ["src/modules/ModuleDistributor.GrpcWebSocketBridge/ModuleDistributor.GrpcWebSocketBridge.csproj", "src/modules/ModuleDistributor.GrpcWebSocketBridge/"]
COPY ["src/modules/ModuleDistributor.Grpc/ModuleDistributor.Grpc.csproj", "src/modules/ModuleDistributor.Grpc/"]
COPY ["src/services/SEServer.Statements/SEServer.Statements.Applications/SEServer.Statements.Applications.csproj", "src/services/SEServer.Statements/SEServer.Statements.Applications/"]
COPY ["src/modules/ModuleDistributor.DependencyInjection/ModuleDistributor.DependencyInjection.csproj", "src/modules/ModuleDistributor.DependencyInjection/"]
COPY ["src/services/SEServer.Statements/SEServer.Statements.Applications.Abstracts/SEServer.Statements.Applications.Abstracts.csproj", "src/services/SEServer.Statements/SEServer.Statements.Applications.Abstracts/"]
COPY ["src/modules/ModuleDistributor.Dapr.Actors/ModuleDistributor.Dapr.Actors.csproj", "src/modules/ModuleDistributor.Dapr.Actors/"]
COPY ["src/services/SEServer.Statements/SEServer.Statements.Domain/SEServer.Statements.Domain.csproj", "src/services/SEServer.Statements/SEServer.Statements.Domain/"]
COPY ["src/services/SEServer.Statements/SEServer.Statements.Domain.Shared/SEServer.Statements.Domain.Shared.csproj", "src/services/SEServer.Statements/SEServer.Statements.Domain.Shared/"]
COPY ["src/services/SEServer.Statements/SEServer.Statements.EntityFrameworkCore/SEServer.Statements.EntityFrameworkCore.csproj", "src/services/SEServer.Statements/SEServer.Statements.EntityFrameworkCore/"]
COPY ["src/modules/ModuleDistributor.EntityFrameworkCore/ModuleDistributor.EntityFrameworkCore.csproj", "src/modules/ModuleDistributor.EntityFrameworkCore/"]
RUN dotnet restore "src/services/SEServer.Statements/SEServer.Statements/SEServer.Statements.csproj"
COPY . .
WORKDIR "/src/src/services/SEServer.Statements/SEServer.Statements"
RUN dotnet build "SEServer.Statements.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SEServer.Statements.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SEServer.Statements.dll"]