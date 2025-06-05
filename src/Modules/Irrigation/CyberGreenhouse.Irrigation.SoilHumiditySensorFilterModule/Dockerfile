# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Modules/Irrigation/CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule/CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule.csproj", "src/Modules/Irrigation/CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule/"]
COPY ["src/Core/CyberGreenhouse.Core/CyberGreenhouse.Core.csproj", "src/Core/CyberGreenhouse.Core/"]
COPY ["src/Core/CyberGreenhouse.MessageBus.Contracts/CyberGreenhouse.MessageBus.Contracts.csproj", "src/Core/CyberGreenhouse.MessageBus.Contracts/"]
COPY ["src/Core/CyberGreenhouse.MessageBus/CyberGreenhouse.MessageBus.csproj", "src/Core/CyberGreenhouse.MessageBus/"]
COPY ["src/Core/CyberGreenhouse.MessageBus.RabbitMQ/CyberGreenhouse.MessageBus.RabbitMQ.csproj", "src/Core/CyberGreenhouse.MessageBus.RabbitMQ/"]
RUN dotnet restore "./src/Modules/Irrigation/CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule/CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule.csproj"
COPY . .
WORKDIR "/src/src/Modules/Irrigation/CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule"
RUN dotnet build "./CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule.dll"]