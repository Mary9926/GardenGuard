FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GardenGuardFrontend/Server/GardenGuardFrontend.Server.csproj", "GardenGuardFrontend/Server/"]
COPY ["GardenGuardFrontend/Client/GardenGuardFrontend.Client.csproj", "GardenGuardFrontend/Client/"]
RUN dotnet restore "GardenGuardFrontend/Server/GardenGuardFrontend.Server.csproj"
COPY . .
WORKDIR "/src/GardenGuardFrontend/Server"
RUN dotnet build "GardenGuardFrontend.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GardenGuardFrontend.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GardenGuardFrontend.Server.dll"]