# GEMINI.md

This file provides guidance to Gemini when working with code in this repository.

## Project Overview

This is an ASP.NET Core 8.0 Razor Pages application for a Human Resources Management System (HRMS). The application uses PostgreSQL as the database with a multi-schema architecture (core, attendance, payroll, rules) and follows a layered architecture pattern with Models, Services, and Pages. The application does not use an ORM like Entity Framework, but instead uses a custom `DbAdapter` for raw SQL operations.

## Building and Running

### Build the application
```bash
dotnet build
```

### Run the application
```bash
dotnet run
```
The application will be available at http://localhost:8001 or https://localhost:8002.

### Run with hot reload during development
```bash
dotnet watch run
```

### Database
- **Database:** PostgreSQL
- **Connection String:** The connection string is configured in `appsettings.json`.
- **Database Schema:** The database schema is defined in the `.ddl` files located in the `Models/DDL` directory.

## Development Conventions

### Architecture
- **Models:** Contain business logic and database operations. They use parameterized SQL queries to interact with the database.
- **Services:** Provide an additional business logic layer. Services can use multiple models to perform complex business logic.
- **Pages:** Handle HTTP requests and user interaction. SQL statements are not allowed in the Page Layer.
- **DbAdapter:** A custom class that wraps `NpgsqlConnection` and provides methods for executing queries and commands. It handles transactions, connection management, and parameter binding. All database operations go through this adapter.

### Key Technologies
- ASP.NET Core 8.0 Razor Pages
- PostgreSQL
- Npgsql
- CsvHelper
- Microsoft.Data.Analysis

### Frontend
- Material Dashboard theme
- Razor Pages with a shared layout
- Bootstrap-based responsive design

### Code Patterns
- Manual SQL query construction with parameterized queries.
- Dependency injection is used throughout the application.
- All pages inherit from `BasePageModel`.
- Services follow the `BaseService` pattern.
- Multi-schema database design for logical separation of HR domains.
