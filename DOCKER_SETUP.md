# Webshop Docker Setup

## Secrets Management

Dieses Projekt verwendet eine `.env` Datei für die Verwaltung von Secrets und Konfiguration.

### Setup

1. **Kopiere die Beispiel-Datei:**
   ```bash
   cp .env.example .env
   ```

2. **Bearbeite die `.env` Datei** und setze deine eigenen Passwörter:
   ```bash
   # Öffne in einem Editor
   notepad .env
   ```

3. **Wichtige Variablen:**
   - `SA_PASSWORD`: System Administrator Passwort für SQL Server
   - `MSSQL_PID`: SQL Server Edition (z.B. Developer, Express)
   - `DATABASE_NAME`: Name der Datenbank

### Docker Container starten

```bash
# Container bauen und starten
docker compose up -d --build

# Logs anzeigen
docker compose logs -f

# Container stoppen
docker compose down

# Container mit Volumes löschen (ACHTUNG: Löscht Datenbank!)
docker compose down -v
```

### Sicherheitshinweise

?? **WICHTIG:**
- Die `.env` Datei ist in `.gitignore` und wird **nicht** ins Git committed
- Teile die `.env` Datei niemals öffentlich
- Verwende für Production starke, zufällig generierte Passwörter (mindestens 8 Zeichen mit Großbuchstaben, Kleinbuchstaben, Zahlen und Sonderzeichen)
- Die `.env.example` Datei dient nur als Template und enthält keine echten Secrets
- SQL Server erfordert ein **starkes Passwort** für SA_PASSWORD, sonst startet der Container nicht!

### Ports

- **8080** - HTTP (Webshop)
- **8081** - HTTPS (Webshop)
- **1433** - SQL Server (Host-Port für lokale Zugriffe)

### Lokale Entwicklung

Falls du lokal (außerhalb Docker) entwickeln möchtest:

1. Die Datenbank läuft auf `localhost:1433`
2. Connection String: `Server=localhost,1433;Database=WebshopDb;User Id=sa;Password=<dein-password>;Encrypt=false;`
3. Update `appsettings.Development.json` entsprechend

### Umgebungsvariablen überschreiben

Du kannst Variablen auch per Kommandozeile überschreiben:

```bash
HOST_HTTP_PORT=9090 docker compose up -d
```

### SQL Server Besonderheiten

?? **SQL Server Anforderungen:**
- Das `SA_PASSWORD` muss mindestens 8 Zeichen lang sein
- Es muss Großbuchstaben, Kleinbuchstaben, Zahlen und Sonderzeichen enthalten
- Beispiel: `MyP@ssw0rd`

### Für Teammitglieder

Wenn du das Projekt neu klonst:

1. Kopiere `.env.example` nach `.env`
2. Hole dir die echten Passwörter vom Team (z.B. über einen sicheren Kanal)
3. Füge die Passwörter in deine lokale `.env` Datei ein (mindestens 8 Zeichen, sicher!)
4. Starte die Container: `docker compose up -d --build`
