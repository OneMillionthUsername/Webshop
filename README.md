# Webshop

Ein ASP.NET Core MVC-Webshop-Projekt mit Repository Pattern, Service Layer und vollständiger Unit-Test-Abdeckung.

## Technologie-Stack

| Bereich | Technologie |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Datenbank | SQL Server / MSSQL **oder** MySQL / MariaDB |
| ORM | Entity Framework Core |
| Dependency Injection | ASP.NET Core built-in DI |
| Unit Tests | xUnit + Moq |
| Object Mapping | AutoMapper |

## Projektstruktur

```
WebshopWorkspace/
├── Webshop/                        # Hauptprojekt
│   ├── Controllers/                # MVC Controller
│   ├── Models/                     # Domänen-Modelle
│   ├── Dtos/                       # Data Transfer Objects
│   ├── Repositories/               # Data Access Layer
│   ├── Services/                   # Business Logic Layer
│   ├── Data/                       # DbContext & Migrations
│   └── docs/                       # Architekturdokumentation
└── Tests_BL_Backend/               # Unit-Test-Projekt
    ├── Controllers/
    ├── Repositories/
    └── Services/
```

## Voraussetzungen

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server **oder** MySQL / MariaDB

## Einrichtung

### 1. Repository klonen

```bash
git clone https://github.com/OneMillionthUsername/Webshop.git
cd Webshop
```

### 2. Datenbank konfigurieren

Passe `Webshop/appsettings.json` an:

**SQL Server / MSSQL** (`DatabaseProvider`: `SqlServer` oder `MSSQL`):
```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "SqlServerConnection": "Server=localhost;Database=Webshop;Trusted_Connection=True;"
  }
}
```

**MySQL / MariaDB** (`DatabaseProvider`: `MySQL` oder `MariaDB`):
```json
{
  "DatabaseProvider": "MySQL",
  "ConnectionStrings": {
    "MySqlConnection": "Server=localhost;Database=Webshop;User=root;Password=deinPasswort;"
  }
}
```

### 3. Migrationen anwenden

```bash
cd Webshop
dotnet ef database update
```

### 4. Anwendung starten

```bash
dotnet run
```

## Tests ausführen

```bash
dotnet test
```

## Architektur

Das Projekt folgt einem strikten Schichtenmodell:

```
Controller → Service (Interface) → Repository (Interface) → DbContext
```

- **Repository Layer** – kapselt alle Datenbankzugriffe hinter Interfaces
- **Service Layer** – enthält die Geschäftslogik, arbeitet ausschließlich mit Repository-Interfaces
- **Controller Layer** – nimmt HTTP-Anfragen entgegen, delegiert an Services, gibt DTOs zurück

## Domänenmodell

| Modell | Beschreibung |
|---|---|
| `Product` | Produkt mit Basispreis und Kategorie |
| `ProductVariant` | Varianten eines Produkts (z. B. Größe, Farbe) |
| `Category` | Produktkategorie |
| `Order` | Bestellung eines Kunden |
| `OrderItem` | Einzelposition einer Bestellung |
| `Customer` | Kundenstammdaten |
| `Payment` | Zahlungsinformationen zu einer Bestellung |
| `Discount` | Rabattdefinitionen |
