# 🌳 DrzewaAPI
Backend do projektu mapy drzew.

## Spis treści

1. [Funkcje](#funkcje)
2. [Dostępne endpointy](#dostępne-endpointy)
   - [AuthController](#authcontroller)
   - [UsersController](#userscontroller)
3. [Technologie](#technologie)
4. [Uruchamianie aplikacji](#uruchamianie-aplikacji)
5. [Licencja](#licencja)

## Funkcje
- Rejestracja nowych użytkowników.
- Logowanie istniejących użytkowników i generowanie tokenów uwierzytelniających.
- Pobieranie listy wszystkich użytkowników.
- Pobieranie danych pojedynczego użytkownika na podstawie identyfikatora.
- Aktualizacja profilu użytkownika z uwzględnieniem autoryzacji i ról.

## Dostępne Endpointy

### AuthController
Ścieżka bazowa: `api/auth`
- **POST** `api/auth/register` - Rejestruje nowego użytkownika
- **POST** `api/auth/login` - Loguje użytkownika i zwraca token JWT

### UsersController
Ścieżka bazowa: `api/users` (wymagana autoryzacja)
- **GET** `api/users` - Pobiera listę wszystkich użytkowników
- **GET** `api/users/{id}` - Pobiera szczegółowe informacje o użytkowniku na podstawie identyfikatora (GUID)
- **PUT** `api/users/{id}` - Aktualizuje dane użytkownika

## Technologie
- **.NET 9 / ASP.NET Core** – framework do tworzenia aplikacji webowych i API
- **C#** – język programowania
- **Entity Framework Core (opcjonalnie w dalszym rozwoju)** – do komunikacji z bazą danych
- **JWT (JSON Web Token)** – do uwierzytelniania i autoryzacji użytkowników

## Uruchamianie aplikacji
1. Sklonuj repozytorium:
   `git clone https://github.com/3-Flora/DrzewaAPI.git`
2. Wejdź do folderu projektu:
   `cd DrzewaAPI`
3. Przywróć zależności:
   `dotnet restore`
4. Uruchom aplikację:
   `dotnet run`
5. Aplikacja będzie dostępna pod adresem:
   `https://localhost:7274` lub `http://localhost:5174`

## Licencja
Projekt udostępniany jest na licencji MIT.
