# DrzewaAPI - Instrukcje dla developera

## 🚀 Szybki start

### 1. Uruchomienie lokalne
```bash
dotnet run --environment Development
```

### 2. Test aplikacji
- **URL:** http://localhost:5000
- **API:** http://localhost:5000/api/species

## 🔐 Dane dostępowe

### Użytkownik testowy
- **Email:** `user@example.com`
- **Hasło:** `string`

### Baza danych Azure SQL
- **Serwer:** `drzewaapi-sql-server-2025.database.windows.net`
- **Baza danych:** `DrzewaDB`
- **Użytkownik:** `drzewaadmin`
- **Hasło:** `DrzewaAPI2024!StrongPassword`

### Azure Storage
- **Konto:** `drzewaapistorage2024`
- **Kontener:** `uploads`

## 📁 Struktura projektu

- `Controllers/` - Kontrolery API
- `Services/` - Logika biznesowa
- `Models/` - Modele danych
- `Dtos/` - Data Transfer Objects
- `Data/` - Kontekst bazy danych
- `Migrations/` - Migracje EF Core

## 🔧 Konfiguracja

- `appsettings.json` - Konfiguracja podstawowa
- `appsettings.Development.json` - Konfiguracja lokalna
- `appsettings.Production.json` - Konfiguracja produkcyjna

## 🌐 Endpointy API

### Autoryzacja
- `POST /api/auth/login` - Logowanie
- `POST /api/auth/register` - Rejestracja

### Drzewa
- `GET /api/trees` - Lista drzew
- `GET /api/trees/{id}` - Pojedyncze drzewo
- `POST /api/trees` - Dodaj drzewo (wymaga autoryzacji)

### Gatunki
- `GET /api/species` - Lista gatunków

### Aplikacje
- `GET /api/applications` - Lista aplikacji użytkownika
- `POST /api/applications/{id}/submit` - Prześlij aplikację

## 🚀 Deployment na Azure

```bash
dotnet publish -c Release -o ./publish
az webapp deploy --resource-group DrzewaAPI-RG --name drzewaapi-app-2024 --src-path ./publish.zip
```

## 📝 Uwagi

- Aplikacja używa Azure SQL Database i Azure Blob Storage
- Wszystkie obrazy są przechowywane w Azure Storage
- PDF-y generowane lokalnie są zapisywane w Azure Storage
- Aplikacja automatycznie seeduje dane testowe przy starcie
