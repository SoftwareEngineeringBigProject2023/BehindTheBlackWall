#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/SEServer.Auth/SEServer.Auth/SEServer.Auth.csproj", "src/services/SEServer.Auth/SEServer.Auth/"]
COPY ["src/modules/ModuleDistributor.Dapr.Configuration/ModuleDistributor.Dapr.Configuration.csproj", "src/modules/ModuleDistributor.Dapr.Configuration/"]
COPY ["src/modules/ModuleDistributor.Dapr/ModuleDistributor.Dapr.csproj", "src/modules/ModuleDistributor.Dapr/"]
COPY ["src/modules/ModuleDistributor/ModuleDistributor.csproj", "src/modules/ModuleDistributor/"]
COPY ["src/modules/ModuleDistributor.EntityFrameworkCore/ModuleDistributor.EntityFrameworkCore.csproj", "src/modules/ModuleDistributor.EntityFrameworkCore/"]
COPY ["src/modules/ModuleDistributor.GrpcWebSocketBridge/ModuleDistributor.GrpcWebSocketBridge.csproj", "src/modules/ModuleDistributor.GrpcWebSocketBridge/"]
COPY ["src/modules/ModuleDistributor.Grpc/ModuleDistributor.Grpc.csproj", "src/modules/ModuleDistributor.Grpc/"]
COPY ["src/modules/ModuleDistributor.Logging/ModuleDistributor.Logging.csproj", "src/modules/ModuleDistributor.Logging/"]
COPY ["src/modules/ModuleDistributor.Serilog/ModuleDistributor.Serilog.csproj", "src/modules/ModuleDistributor.Serilog/"]
COPY ["src/services/SEServer.Auth/SEServer.Auth.Protos/SEServer.Auth.Protos.csproj", "src/services/SEServer.Auth/SEServer.Auth.Protos/"]
RUN dotnet restore "src/services/SEServer.Auth/SEServer.Auth/SEServer.Auth.csproj"
COPY . .
WORKDIR "/src/src/services/SEServer.Auth/SEServer.Auth"
RUN dotnet build "SEServer.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SEServer.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SEServer.Auth.dll"]