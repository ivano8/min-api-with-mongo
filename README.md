## README – Minimal API mit MongoDB (Docker)

---

## Ziel

* ASP.NET Core Minimal API erstellen
* MongoDB anbinden
* Containerisierung mit Docker
* Orchestrierung mit docker-compose
* Konfiguration via Options Pattern

---

## Projektstruktur

```text
min-api-with-mongo/
│
├── docker-compose.yml
└── WebApi/
    ├── Dockerfile
    ├── Program.cs
    ├── WebApi.csproj
    ├── DatabaseSettings.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    └── Properties/
        └── launchSettings.json
```

---

## Voraussetzungen

* .NET 8 SDK
* Docker + Docker Compose

---

## Anwendung starten

```bash
docker compose up --build
```

Aufruf:

```text
http://localhost:5001
http://localhost:5001/check
```

---

## Komponentenübersicht

### 1. Minimal API (`Program.cs`)

* Definiert Endpunkte:

  * `/` → Testmeldung
  * `/check` → prüft MongoDB-Verbindung
* Verwendet `MongoDB.Driver`
* Zugriff auf Konfiguration via `IOptions`

---

### 2. Konfiguration (`appsettings.json`)

```json
"DatabaseSettings": {
  "ConnectionString": "mongodb://gbs:geheim@localhost:27017"
}
```

* Basis-Konfiguration
* Wird in Docker via Environment überschrieben

---

### 3. Options Pattern

**Klasse:**

```csharp
public class DatabaseSettings
{
    public string ConnectionString { get; set; } = "";
}
```

**Registrierung:**

```csharp
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));
```

**Verwendung:**

```csharp
(IOptions<DatabaseSettings> options)
```

---

### 4. Dockerfile (Multistage)

* Stage 1: Build (`sdk:8.0`)
* Stage 2: Runtime (`aspnet:8.0`)

Wesentliche Punkte:

* `dotnet restore`
* `dotnet publish`
* Port: `5001`
* Start via `dotnet WebApi.dll`

---

### 5. docker-compose.yml

```yaml
services:
  webapi:
    build:
      context: ./WebApi
    ports:
      - "5001:5001"
    environment:
      DatabaseSettings__ConnectionString: "mongodb://gbs:geheim@mongodb:27017"
    depends_on:
      - mongodb

  mongodb:
    image: mongo:latest
    volumes:
      - mongo-data:/data/db
    environment:
      MONGO_INITDB_ROOT_USERNAME: gbs
      MONGO_INITDB_ROOT_PASSWORD: geheim

volumes:
  mongo-data:
```

---

## Wichtige Konzepte

### Container-Netzwerk

* Services kommunizieren über Namen:

  * `mongodb` statt `localhost`

---

### Environment Overrides

* `:` → `__` in Environment Variables

```text
DatabaseSettings:ConnectionString
→ DatabaseSettings__ConnectionString
```

---

### Persistenz

* MongoDB speichert Daten in Volume:

```yaml
mongo-data:/data/db
```

---

## Häufige Fehler

### Port bereits belegt

```text
port is already allocated
```

**Lösung:**

* bestehenden Container stoppen
* oder Port-Mapping entfernen

---

### MongoDB nicht erreichbar

**Ursache:**

* falscher Host (`localhost` im Container)

**Lösung:**

```text
mongodb://gbs:geheim@mongodb:27017
```

---

### Build-Fehler bei `publish`

**Lösung:**

* `--no-restore` entfernen oder Cache neu bauen:

```bash
docker compose build --no-cache
```

---

## Testergebnis

Aufruf:

```text
http://localhost:5001/check
```

Erwartung:

```text
Zugriff auf MongoDB ok. Vorhandene DBs: admin,config,local
```

---

## Kernaussage

* Minimal API + MongoDB integriert
* Konfiguration entkoppelt (Options Pattern)
* Vollständig containerisiert
* Start via `docker compose`
* Kommunikation über interne Docker-Namen statt `localhost`
