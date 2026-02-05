# User Secrets Setup - Webshop Projekt

## ?? Was sind User Secrets?

User Secrets ist ein .NET Feature, das Secrets **lokal auf Ihrem Entwicklungsrechner** speichert, außerhalb des Projekt-Verzeichnisses. Diese Secrets werden **niemals** ins Git committed.

## ?? Setup für lokale Entwicklung

### 1. User Secrets initialisieren (bereits konfiguriert)

Das Projekt hat bereits eine User Secrets ID in der `.csproj`:
```xml
<UserSecretsId>504c0d33-7bec-40bf-b4f6-244b75bdcc57</UserSecretsId>
```

### 2. Connection String als User Secret speichern

**Öffnen Sie PowerShell im Projekt-Verzeichnis** und führen Sie aus:

```powershell
# Connection String als User Secret hinzufügen
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=webshop_password;"
```

? Das Passwort ist jetzt sicher auf Ihrem Rechner gespeichert (in: `%APPDATA%\Microsoft\UserSecrets\504c0d33-7bec-40bf-b4f6-244b75bdcc57\secrets.json`)

### 3. Überprüfen

```powershell
# Alle User Secrets anzeigen
dotnet user-secrets list

# Sollte ausgeben:
# ConnectionStrings:DefaultConnection = Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=webshop_password;
```

### 4. User Secret bearbeiten (falls nötig)

```powershell
# User Secrets Datei direkt öffnen
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=NEUES_PASSWORT;"

# Oder alle Secrets löschen und neu beginnen
dotnet user-secrets clear
```

## ?? Für Docker (bereits konfiguriert)

In Docker werden die Secrets aus der `.env` Datei geladen - **keine Änderung nötig!**

```yaml
# compose.yaml verwendet bereits:
environment:
  - ConnectionStrings__DefaultConnection=Server=${DB_SERVER};Port=${DB_PORT};Database=${MYSQL_DATABASE};User=${MYSQL_USER};Password=${MYSQL_PASSWORD};
```

## ?? Konfigurationshierarchie

.NET lädt Konfiguration in dieser Reihenfolge (spätere überschreiben frühere):

1. `appsettings.json` (Basis-Konfiguration, ohne Secrets)
2. `appsettings.Development.json` (Entwicklungsumgebung)
3. **User Secrets** (Lokale Secrets, nur Development)
4. **Umgebungsvariablen** (Docker, Production)
5. **Kommandozeilen-Argumente**

## ?? Zusammenfassung

### Lokale Entwicklung (ohne Docker)
```powershell
# 1. User Secrets setzen
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=webshop_password;"

# 2. Anwendung starten
dotnet run

# 3. Migrationen ausführen
dotnet ef database update
```

### Docker Entwicklung
```powershell
# Verwendet automatisch .env Datei
docker compose up -d --build
```

### Neues Teammitglied?

**Sende ihnen diese Anleitung** und den Befehl:
```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=DAS_ECHTE_PASSWORT;"
```

?? **Sende das Passwort über einen sicheren Kanal** (z.B. verschlüsselte E-Mail, Teams Private Chat, Password Manager)

## ?? Troubleshooting

### Problem: "Connection string not found"

**Lösung 1:** User Secrets prüfen
```powershell
dotnet user-secrets list
```

**Lösung 2:** User Secrets neu setzen
```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=webshop_password;"
```

### Problem: Migration schlägt fehl

**Lösung:** Stelle sicher, dass die Datenbank läuft:
```powershell
# In Docker
docker compose up -d shop-db

# Oder lokal auf Port 3307 prüfen
Test-NetConnection -ComputerName localhost -Port 3307
```

### Problem: Falsches Passwort in User Secrets

**Lösung:** Überschreiben
```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=WebshopDb;User=webshop_user;Password=KORREKTES_PASSWORT;"
```

## ?? Weitere Informationen

- [.NET User Secrets Dokumentation](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Environment Variables in .NET](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)

---

## ? Checkliste für neue Entwickler

- [ ] Repository geklont
- [ ] `.env` Datei erstellt (von `.env.example` kopiert)
- [ ] Passwörter in `.env` aktualisiert
- [ ] User Secrets gesetzt: `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..."`
- [ ] Docker Container gestartet: `docker compose up -d --build`
- [ ] Anwendung getestet: http://localhost:8080

?? Fertig! Sie können jetzt sicher entwickeln ohne Secrets ins Git zu committen.
