FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GrandDiscordBot/GrandDiscordBot.csproj", "GrandDiscordBot/"]
RUN dotnet restore "./GrandDiscordBot/./GrandDiscordBot.csproj"
COPY . .
WORKDIR "/src/GrandDiscordBot"
RUN dotnet build "./GrandDiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GrandDiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrandDiscordBot.dll"]