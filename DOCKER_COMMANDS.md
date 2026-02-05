# Docker Befehls-Referenz - Webshop Projekt

Schnelle Referenz für die wichtigsten Docker-Befehle für dieses Projekt.

## ?? Docker Compose Befehle

### Container starten/stoppen

```bash
# Container im Hintergrund starten
docker compose up -d

# Container starten und neu bauen
docker compose up -d --build

# Nur bestimmten Service starten
docker compose up -d webshop
docker compose up -d shop-db

# Container stoppen
docker compose stop

# Container stoppen und entfernen
docker compose down

# Container, Volumes und Images löschen (ACHTUNG: Löscht alle Daten!)
docker compose down -v --rmi all
```

### Container neu starten

```bash
# Alle Container neu starten
docker compose restart

# Nur einen Service neu starten
docker compose restart webshop
docker compose restart shop-db

# Container stoppen, neu bauen und starten
docker compose down && docker compose up -d --build
```

### Status und Informationen

```bash
# Status aller Container anzeigen
docker compose ps

# Konfiguration validieren und anzeigen
docker compose config

# Verwendete Images anzeigen
docker compose images

# Prozesse in Containern anzeigen
docker compose top
```

## ?? Logs und Debugging

### Logs anzeigen

```bash
# Logs aller Services anzeigen
docker compose logs

# Logs eines bestimmten Services
docker compose logs webshop
docker compose logs shop-db

# Logs live verfolgen (follow)
docker compose logs -f

# Logs eines Services live verfolgen
docker compose logs -f webshop

# Nur die letzten 50 Zeilen
docker compose logs --tail=50 webshop

# Logs mit Zeitstempel
docker compose logs -t webshop
```

### In Container eintreten

```bash
# Bash-Shell im Webshop-Container öffnen
docker exec -it webshop-app /bin/bash

# Shell im Datenbank-Container öffnen
docker exec -it webshop-mariadb /bin/bash

# MySQL/MariaDB CLI direkt öffnen
docker exec -it webshop-mariadb mysql -u webshop_user -p

# Als Root-User ins Datenbank
docker exec -it webshop-mariadb mysql -u root -p

# Einzelnen Befehl im Container ausführen
docker exec webshop-app ls -la
docker exec webshop-app dotnet --version
```

## ??? Datenbank Management

### Datenbank-Zugriff

```bash
# In die Datenbank einloggen
docker exec -it webshop-mariadb mysql -u webshop_user -p
# Passwort eingeben: webshop_password (oder aus .env)

# Als Root
docker exec -it webshop-mariadb mysql -u root -p

# SQL-Befehle direkt ausführen
docker exec -it webshop-mariadb mysql -u root -p -e "SHOW DATABASES;"
docker exec -it webshop-mariadb mysql -u root -p -e "USE WebshopDb; SHOW TABLES;"
```

### Datenbank Backup und Restore

```bash
# Datenbank-Backup erstellen
docker exec webshop-mariadb mysqldump -u root -prootpassword123 WebshopDb > backup_$(date +%Y%m%d_%H%M%S).sql

# Backup wiederherstellen
docker exec -i webshop-mariadb mysql -u root -prootpassword123 WebshopDb < backup_20250205_120000.sql

# Alle Datenbanken sichern
docker exec webshop-mariadb mysqldump -u root -prootpassword123 --all-databases > full_backup.sql
```

### EF Core Migrationen

```bash
# Migration wird automatisch beim Start ausgeführt!
# Falls manuell nötig (lokal außerhalb Docker):

# Neue Migration erstellen
dotnet ef migrations add MigrationName

# Migration anwenden
dotnet ef database update

# Letzte Migration rückgängig machen
dotnet ef migrations remove

# Alle Migrationen auflisten
dotnet ef migrations list

# SQL-Script generieren
dotnet ef migrations script
```

## ?? Docker Container Befehle (direkt)

### Container Management

```bash
# Alle laufenden Container anzeigen
docker ps

# Alle Container anzeigen (auch gestoppte)
docker ps -a

# Container stoppen
docker stop webshop-app
docker stop webshop-mariadb

# Container starten
docker start webshop-app
docker start webshop-mariadb

# Container neu starten
docker restart webshop-app

# Container löschen
docker rm webshop-app
docker rm -f webshop-app  # Erzwingen, auch wenn läuft

# Alle gestoppten Container löschen
docker container prune
```

### Container Informationen

```bash
# Detaillierte Container-Infos
docker inspect webshop-app

# Ressourcen-Nutzung anzeigen (CPU, RAM)
docker stats

# Einzelnen Container überwachen
docker stats webshop-app

# Container-Prozesse anzeigen
docker top webshop-app
```

## ??? Docker Image Befehle

### Images verwalten

```bash
# Alle Images anzeigen
docker images

# Image löschen
docker rmi webshop-webshop

# Ungenutzte Images löschen
docker image prune

# Alle ungenutzten Images löschen (nicht von Containern verwendet)
docker image prune -a

# Image neu bauen
docker build -t webshop:latest .

# Build mit spezifischem Tag
docker build -t webshop:v1.0 .
```

### Image Informationen

```bash
# Image-Details anzeigen
docker inspect webshop-webshop

# Image-Historie anzeigen
docker history webshop-webshop

# Größe der Images
docker images --format "table {{.Repository}}\t{{.Tag}}\t{{.Size}}"
```

## ?? Netzwerk und Volumes

### Netzwerk

```bash
# Alle Netzwerke anzeigen
docker network ls

# Netzwerk-Details
docker network inspect webshop_webshop-network

# Container im Netzwerk anzeigen
docker network inspect webshop_webshop-network --format='{{range .Containers}}{{.Name}} {{end}}'
```

### Volumes

```bash
# Alle Volumes anzeigen
docker volume ls

# Volume-Details
docker volume inspect webshop_shop-data

# Ungenutztes Volume löschen
docker volume rm webshop_shop-data

# Alle ungenutzten Volumes löschen
docker volume prune
```

## ?? System aufräumen

```bash
# Alle gestoppten Container, ungenutzte Netzwerke und hängende Images löschen
docker system prune

# Alles löschen inkl. Volumes (ACHTUNG: Datenverlust!)
docker system prune -a --volumes

# Speicherplatz-Übersicht
docker system df

# Detaillierte Speicherplatz-Übersicht
docker system df -v
```

## ?? Troubleshooting

### Häufige Probleme

```bash
# Port bereits belegt?
# Prüfe welcher Prozess Port 8080 verwendet
netstat -ano | findstr :8080

# Container startet nicht?
# Prüfe die Logs
docker compose logs webshop

# Datenbank-Verbindung fehlgeschlagen?
# Prüfe ob Container läuft und healthy ist
docker compose ps
docker inspect webshop-mariadb --format='{{.State.Health.Status}}'

# Container neu bauen ohne Cache
docker compose build --no-cache
docker compose up -d --force-recreate

# Netzwerk-Probleme?
# Netzwerk neu erstellen
docker compose down
docker network prune
docker compose up -d
```

### Container Diagnose

```bash
# CPU und Memory Limits setzen
docker update --cpus="1.5" --memory="1g" webshop-app

# Container-Events live anzeigen
docker events

# Nur Events eines Containers
docker events --filter container=webshop-app
```

## ?? Produktions-Tipps

```bash
# Container im Production-Modus starten
# Setze in .env: ASPNETCORE_ENVIRONMENT=Production
docker compose up -d

# Health-Check manuell prüfen
docker inspect webshop-mariadb --format='{{json .State.Health}}'

# Container-Restart-Policy ändern
docker update --restart=unless-stopped webshop-app

# Logs rotieren (limitieren)
docker compose logs --tail=1000 > logs.txt
```

## ?? Nützliche Kombinationen

```bash
# Kompletter Neustart mit frischen Daten
docker compose down -v && docker compose up -d --build

# Schneller Update-Zyklus
docker compose down && docker compose build && docker compose up -d

# Nur Webshop neu deployen (Datenbank läuft weiter)
docker compose up -d --build webshop

# Alle Container-IDs anzeigen
docker ps -aq

# Alle Container stoppen
docker stop $(docker ps -aq)

# Logs aller Container in eine Datei
docker compose logs > all-logs.txt

# Container-Status überwachen (aktualisiert alle 2 Sek)
watch -n 2 docker compose ps
```

## ?? Weitere Ressourcen

- [Docker Compose Dokumentation](https://docs.docker.com/compose/)
- [Docker CLI Referenz](https://docs.docker.com/engine/reference/commandline/cli/)
- [Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

## ? Quick Reference - Die wichtigsten Befehle

| Befehl | Beschreibung |
|--------|--------------|
| `docker compose up -d --build` | Container neu bauen und starten |
| `docker compose down` | Container stoppen und entfernen |
| `docker compose logs -f webshop` | Logs live verfolgen |
| `docker compose ps` | Container-Status anzeigen |
| `docker compose restart webshop` | Service neu starten |
| `docker exec -it webshop-app /bin/bash` | In Container eintreten |
| `docker stats` | Ressourcen-Nutzung anzeigen |
| `docker system prune` | System aufräumen |

---

**Tipp:** Füge diese Befehle zu deinen Bookmarks hinzu oder drucke sie aus! ??
