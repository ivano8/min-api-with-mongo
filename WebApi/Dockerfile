# Build-Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Projektdateien kopieren und Restore
COPY . .
RUN dotnet restore

# Anwendung publishen
RUN dotnet publish -c Release -o /app/publish

# Runtime-Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Veröffentlichte Dateien kopieren
COPY --from=build /app/publish .

# Port 5001 freigeben
EXPOSE 5001

# ASP.NET Core auf Port 5001 konfigurieren
ENV ASPNETCORE_URLS=http://+:5001

# Anwendung starten
ENTRYPOINT ["dotnet", "WebApi.dll"]