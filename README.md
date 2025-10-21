# 🌳 DrzewaAPI

A comprehensive .NET Web API for managing tree registrations, monument applications, and commune administration in Poland. It enables users to document and manage trees as well as submit monument applications to local communes.

## 📑 Table of Contents

- [Features](#-getting-started)
- [Features](#-features)
- [API Endpoints](#-api-endpoints)
- [Technology Stack](#️-technology-stack)
- [Security](#-security)
- [License](#-license)

## 🌱 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later
- SQL Server (LocalDB, Express, or Full)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Verthie/DrzewaAPI.git
   cd drzewa-api
   ```

2. **Configure the database**
   
   Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DrzewaDB;Trusted_Connection=True;"
     }
   }
   ```

3. **Configure application settings**
   
   Set up required configurations in `appsettings.json`:
   ```json
   {
     "Jwt": {
       "Key": "your-secret-key-here",
       "Issuer": "DrzewaAPI",
       "Audience": "DrzewaAPI",
       "ExpiryMinutes": 60
     },
     "Email": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": 587,
       "FromEmail": "your-email@example.com",
       "Password": "your-app-password"
     },
     "Gemini": {
       "ApiKey": "your-gemini-api-key"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the API**
   
   The API will be available at:
   - HTTPS: `https://localhost:7001`
   - HTTP: `http://localhost:5001`

### Using Docker (Optional)

```bash
# Build the image
docker build -t drzewa-api .

# Run the container
docker run -p 5001:80 -e ConnectionStrings__DefaultConnection="your-connection-string" drzewa-api
```

## 🚀 Features

- **Tree Management** - Register, update, and track trees with detailed metadata
- **Species Database** - Comprehensive tree species information with images
- **Monument Applications** - Dynamic form system for tree monument nominations and easy pdf file export from provided data
- **User Authentication** - Secure JWT-based authentication with email verification
- **Commune Administration** - Multi-commune support with custom templates
- **AI Integration** - Gemini AI for application justification generation

## 📡 API Endpoints

### 🔐 Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Auth/register` | Register new user |
| POST | `/api/Auth/login` | User login |
| POST | `/api/Auth/refresh-token` | Refresh JWT token |

### 📧 Email Verification
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/EmailVerification/verify` | Verify email address |
| POST | `/api/EmailVerification/resend` | Resend verification email |
| POST | `/api/EmailVerification/forgot-password` | Request password reset |
| POST | `/api/EmailVerification/reset-password` | Reset password |

### 🌲 Trees
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Trees` | Get all trees |
| GET | `/api/Trees/user` | Get current user's trees |
| GET | `/api/Trees/{id}` | Get specific tree |
| POST | `/api/Trees` | Create new tree entry |
| PUT | `/api/Trees/{id}` | Update tree |
| DELETE | `/api/Trees/{id}` | Delete tree |
| PUT | `/api/Trees/{id}/approve` | Approve tree submission |
| PUT | `/api/Trees/{id}/vote` | Vote for tree |
| DELETE | `/api/Trees/{id}/vote` | Remove vote |

### 🍃 Species
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Species` | Get all species |
| GET | `/api/Species/{id}` | Get specific species |
| POST | `/api/Species` | Add new species |
| DELETE | `/api/Species/{id}` | Delete species |

### 📝 Applications
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Applications` | Get all applications |
| GET | `/api/Applications/{id}` | Get specific application |
| POST | `/api/Applications` | Create application |
| PUT | `/api/Applications/{id}` | Update application |
| DELETE | `/api/Applications/{id}` | Delete application |
| GET | `/api/Applications/{id}/form-schema` | Get form schema |
| POST | `/api/Applications/{id}/submit` | Submit application |
| POST | `/api/Applications/{id}/generate-pdf` | Generate PDF |

### 📋 Application Templates
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/ApplicationTemplates` | Get all templates |
| GET | `/api/ApplicationTemplates/active` | Get active templates |
| GET | `/api/ApplicationTemplates/{id}` | Get specific template |
| GET | `/api/ApplicationTemplates/commune/{communeId}` | Get commune templates |
| POST | `/api/ApplicationTemplates` | Create template |
| PUT | `/api/ApplicationTemplates/{id}` | Update template |
| DELETE | `/api/ApplicationTemplates/{id}` | Delete template |

### 🏛️ Communes
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Communes` | Get all communes |
| GET | `/api/Communes/{id}` | Get specific commune |
| POST | `/api/Communes` | Create commune |
| PUT | `/api/Communes/{id}` | Update commune |
| DELETE | `/api/Communes/{id}` | Delete commune |

### 👤 Users
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Users` | Get all users |
| GET | `/api/Users/current` | Get current user |
| GET | `/api/Users/{id}` | Get specific user |
| PUT | `/api/Users/data/{userId}` | Update user data |
| PUT | `/api/Users/password` | Update password |
| DELETE | `/api/Users/{userId}` | Delete user |

### 🤖 AI Features
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Gemini/application/{id}/justification` | Generate AI justification |

> [!IMPORTANT]
> All endpoints from Authentication, Registration and a couple endpoints from other controllers do not require user authentication.

## 🛠️ Technology Stack

- **.NET** - Web API framework
- **OpenAPI 3.0** - API documentation
- **JWT** - Authentication
- **MaIN.NET + Gemini** - AI-powered features
- **Azure Container + Azure Storage** - API and file hosting
- **Supabase** - Database Hosting
- **Brevo** - SMTP Service Provider
- **itext7 + pupeetersharp** - Pdf file generation from html templates
- **Nominatim API** - Provides tree address based on its cooridinates
- **Geoportal API** - Provides polish plot number, district and other data necessary for sending a valid application

## 🔒 Security

- JWT Bearer token authentication
- Email verification
- Password reset flow
- Refresh token mechanism
- Role-based access control

## 📝 License

This project is licensed under the MIT License.
Made with 🌳 for preserving Poland's natural heritage.
