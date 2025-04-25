# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CRS.ChannelManager/CRS.ChannelManager.csproj", "CRS.ChannelManager/"]
COPY ["CRS.ChannelManager.Application/CRS.ChannelManager.Application.csproj", "CRS.ChannelManager.Application/"]
COPY ["CRS.ChannelManager.Infrastructure/CRS.ChannelManager.Infrastructure.csproj", "CRS.ChannelManager.Infrastructure/"]
COPY ["CRS.ChannelManager.Domain/CRS.ChannelManager.Domain.csproj", "CRS.ChannelManager.Domain/"]
COPY ["CRS.ChannelManager.Library/CRS.ChannelManager.Library.csproj", "CRS.ChannelManager.Library/"]
RUN dotnet restore "./CRS.ChannelManager/CRS.ChannelManager.csproj"
COPY . .
WORKDIR "/src/CRS.ChannelManager"
RUN dotnet build "./CRS.ChannelManager.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CRS.ChannelManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRS.ChannelManager.dll"]