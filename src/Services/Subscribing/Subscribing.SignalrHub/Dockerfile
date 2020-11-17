FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "Notifications.sln" "Notifications.sln"

COPY "BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj" "BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj" 
COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj" "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj"
COPY "BuildingBlocks/EventSource/EventSourcing/EventSourcing.csproj" "BuildingBlocks/EventSource/EventSourcing/EventSourcing.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"  
COPY "Services/Publishing/Publishing.API/Publishing.API.csproj" "Services/Publishing/Publishing.API/Publishing.API.csproj"
COPY "Services/Core/Notifications.Core/Notifications.Core.csproj" "Services/Core/Notifications.Core/Notifications.Core.csproj"
COPY "Services/Notifying/Notifying.API/Notifying.API.csproj" "Services/Notifying/Notifying.API/Notifying.API.csproj"
COPY "Services/Notifying/Notifying.BackgroundTasks/Notifying.BackgroundTasks.csproj" "Services/Notifying/Notifying.BackgroundTasks/Notifying.BackgroundTasks.csproj"
COPY "Services/Subscribing/Subscribing.Infrastructure/Subscribing.Infrastructure.csproj" "Services/Subscribing/Subscribing.Infrastructure/Subscribing.Infrastructure.csproj"
COPY "Services/Subscribing/Subscribing.Application/Subscribing.Application.csproj" "Services/Subscribing/Subscribing.Application/Subscribing.Application.csproj"
COPY "Services/Subscribing/Subscribing.Domain/Subscribing.Domain.csproj" "Services/Subscribing/Subscribing.Domain/Subscribing.Domain.csproj"  
COPY "Services/Subscribing/Subscribing.SignalrHub/Subscribing.SignalrHub.csproj" "Services/Subscribing/Subscribing.SignalrHub/Subscribing.SignalrHub.csproj" 
COPY "Web/WebMVC/WebMVC.csproj" "Web/WebMVC/WebMVC.csproj"
COPY "Web/WebStatus/WebStatus.csproj" "Web/WebStatus/WebStatus.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "Notifications.sln"

COPY . .
WORKDIR /src/Services/Subscribing/Subscribing.SignalrHub
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Subscribing.SignalrHub.dll"]
