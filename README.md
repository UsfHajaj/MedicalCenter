# 🏥 Medical Center Management System - ASP.NET Core Web API 9.0

A powerful and fully featured **Medical Center Management System API** built with **ASP.NET Core 9.0**.  
This project provides full backend support for managing users (patients, doctors, admins), appointments, reviews, and medical centers.

## 🚀 Features

### ✅ Authentication & Account Management
- User/Admin/Doctor registration
- Login with JWT
- Email confirmation
- Profile update
- Reset/Change password
- Account deletion

### 🩺 Appointments
- Create, view, update, and delete appointments
- Filter by patient, date, and status
- View today’s/upcoming/last 30 days appointments
- Total earnings calculation

### 👨‍⚕️ Doctors
- CRUD operations for doctors
- Doctor bookings by status/date
- View reviews, qualifications, rating, and specializations
- Manage appointments

### 🏥 Medical Centers
- CRUD for medical centers
- Manage doctor availabilities in each center

### 👨‍👩‍👧‍👦 Patients
- Patient profile
- Appointment history & date range filters
- Review linking and appointment cancellation

### ⭐ Reviews & Specializations
- Add/Edit/Delete patient reviews
- Unique patient reviewers
- Full CRUD for medical specializations

---

## 📂 Project Structure

MedicalCenter/
├── Controllers/                       # API Controllers
├── Data/
│   └── DTOs/                         # Data Transfer Objects
├── Migrations/                       # EF Core Migrations
├── Model/                            # Entity Models
├── Properties/                       # Project settings
├── Services/                         # Business logic & services
├── wwwroot/
│   └── EmailTemplates/              # Email confirmation templates
├── Program.cs                        # Application entry point
├── SeedRoles.cs                      # Seeding roles on startup
├── MedicalCenter.csproj              # C# project file
├── MedicalCenter.sln                 # Visual Studio solution
├── MedicalCenter.http                # Sample HTTP requests
├── appsettings.json                  # App configuration
├── appsettings.Development.json      # Dev-specific config
├── .gitignore                        # Ignored files for Git
└── .gitattributes                    # Git settings

---

## 🛠️ Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core
- RESTful API architecture
- SQL Server (or SQLite)
- Swagger (Swashbuckle)
- JWT Authentication

---

## 🧑‍💻 Getting Started

Follow these steps to run the project locally:

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/medical-center-api.git
cd medical-center-api

dotnet ef database update

dotnet run

https://localhost:44350/swagger/index.html
```



