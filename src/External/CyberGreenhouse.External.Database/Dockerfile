# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/External/CyberGreenhouse.External.Database/CyberGreenhouse.External.Database.csproj", "src/External/CyberGreenhouse.External.Database/"]
COPY ["src/Core/CyberGreenhouse.Core/CyberGreenhouse.Core.csproj", "src/Core/CyberGreenhouse.Core/"]
RUN dotnet restore "./src/External/CyberGreenhouse.External.Database/CyberGreenhouse.External.Database.csproj"
COPY . .
WORKDIR "/src/src/External/CyberGreenhouse.External.Database"
RUN dotnet build "./CyberGreenhouse.External.Database.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CyberGreenhouse.External.Database.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CyberGreenhouse.External.Database.dll"]