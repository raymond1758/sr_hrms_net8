# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is an ASP.NET Core 8.0 Razor Pages application for Human Resources Management System (HRMS). The application uses PostgreSQL as the database with a multi-schema architecture (core, attendance, payroll, rules) and follows a layered architecture pattern with Models, Services, and Pages.

## Development Commands

### Build and Run
- `dotnet build` - Build the application
- `dotnet run` - Run the application (available at http://localhost:8001 or https://localhost:8002)
- `dotnet watch run` - Run with hot reload during development

### Database
- Database: PostgreSQL (port 5632, database: hrms)
- Connection string configured in appsettings.json: `Host=localhost;Port=5632;Database=hrms;Username=postgres;Password=ddgpemhf;Timeout=3`
- No Entity Framework - uses custom DbAdapter for raw SQL operations
- DbAdapter provides centralized database access with transaction support and proper dependency injection

## Architecture

### Core Components

**DbAdapter** (`Models/DbAdapter.cs`)
- Central database access layer wrapping NpgsqlConnection
- Provides methods for ExecuteQuery, ExecuteReader, ExecuteCommand, ExecuteScalar, ExecuteScalar<T>
- Handles transactions, connection management, and parameter binding with full IDisposable pattern
- All database operations go through this adapter
- Includes comprehensive error handling and logging
- Registered as scoped service in Program.cs for proper dependency injection

**Base Classes**
- `BaseModel` (`Models/BaseModel.cs`) - Abstract base for all models, injects DbAdapter dependency
- `BaseService` (`Services/BaseService.cs`) - Abstract base for all services, injects DbAdapter dependency  
- `BasePageModel` (`Pages/Shared/BasePageModel.cs`) - Base class for Razor Pages, injects DbAdapter dependency

### Data Layer Pattern
- Models contain business logic and database operations (Use parametered SQL to perform SQL operation). Mainly focus on RDBMS CRUD of single table
- Services provide additional business logic layer (Use parametered SQL to perform SQL operation). Service Layer can use multiple models to perform complex business logic
- Pages handle HTTP requests and user interaction (SQL statements are not allowed in Page Layer)
- All database access uses parameterized queries through DbAdapter with proper parameter binding

### Database Schema

**Core Schema** (`Models/DDL/schema_core.ddl`)
- `core.employee` - Employee master data with comprehensive HR information
- `core.department` - Department hierarchy and management structure
- `core.company` - Company information and registration details
- `core.account` - User account management with authentication
- `core.calendar` - Work calendar and holiday management
- `core.job_title` - Job titles and position classifications

**Attendance Schema** (`Models/DDL/schema_attendance.ddl`)
- `attendance.leave_type` - Leave type definitions
- `attendance.leave_records` - Employee leave records and applications
- `attendance.punch_records` - Time tracking and attendance data

**Payroll Schema** (`Models/DDL/schema_payroll.ddl`)
- `payroll.payroll_batch` - Payroll processing batch management
- `payroll.payroll_admin_rewards` - Administrative rewards and bonuses
- Additional payroll-related tables for salary calculations

**Rules Schema** (`Models/DDL/schema_rules.ddl`)
- Business rules and configuration tables

### Key Models
- `Employee` (`Models/Employee.cs`) - Employee management with comprehensive HR data
- `Department` (`Models/Department.cs`) - Department management with hierarchy support
- `PunchRecord` (`Models/PunchRecord.cs`) - Time tracking and punch records
- `PayrollBatch` (`Models/PayrollBatch.cs`) - Payroll batch processing management
- `Calendar` (`Models/Calendar.cs`) - Calendar and work day management
- `Company` (`Models/Company.cs`) - Company information and settings
- `Account` (`Models/Account.cs`) - User account and authentication

### Services Layer
- `EmployeeService` (`Services/EmployeeService.cs`) - Business logic for employee operations
- `DepartmentService` (`Services/DepartmentService.cs`) - Department-related business logic
- `PunchRecordService` (`Services/PunchRecordService.cs`) - Time tracking and attendance management
- `PayrollBatchService` (`Services/PayrollBatchService.cs`) - Payroll batch processing logic
- `CalendarService` (`Services/CalendarService.cs`) - Calendar operations and work day management

### Frontend
- Uses Material Dashboard theme (located in wwwroot/assets/)
- Razor Pages with shared layout (_Layout.cshtml)
- Bootstrap-based responsive design with Material Design components
- Custom navigation components (_Sidenav.cshtml, _Topnav.cshtml)
- Main application pages:
  - `Index.cshtml` - Dashboard/home page with system overview
  - `employees.cshtml` - Employee management interface with CRUD operations
  - `department.cshtml` - Department management interface
  - `punchRecords.cshtml` - Time tracking interface with filtering and pagination
  - `payrollBatches.cshtml` - Payroll batch management interface
  - `calendar.cshtml` - Calendar and scheduling interface

### Utilities
- `CsvHelper` (`Utilities/CsvHelper.cs`) - CSV import/export functionality for data processing

## Important Notes

### Database Connection
- Primary connection string in appsettings.json: `Host=localhost;Port=5632;Database=hrms;Username=postgres;Password=ddgpemhf;Timeout=3`
- Application URLs configured in appsettings.json: `http://localhost:8001;https://localhost:8002`
- Uses PostgreSQL with Npgsql driver (v9.0.3)
- DbAdapter registered as scoped service for proper dependency injection lifecycle

### Dependencies
- CsvHelper (v33.1.0) - for CSV import/export functionality
- Microsoft.Data.Analysis (v0.22.2) - for data analysis features
- Npgsql (v9.0.3) - PostgreSQL .NET driver

### Development Environment
- Target Framework: .NET 8.0
- Nullable reference types enabled
- Implicit usings enabled
- Development environment uses HTTPS redirect and static files
- Comprehensive error handling with Chinese error messages for database operations
- Console logging for database errors during development

### Code Patterns
- Manual SQL query construction with parameterized queries (no ORM)
- Dependency injection through constructor parameters using built-in DI container
- Exception handling with localized Chinese error messages in DbAdapter
- Console logging for database errors and SQL debugging during development
- All pages inherit from BasePageModel for consistent DbAdapter access
- Services follow the BaseService pattern for standardized database operations
- Multi-schema database design for logical separation of HR domains

### Multi-Schema Database Design
The application uses a well-organized multi-schema approach:
- **core**: Employee, department, company, and fundamental HR data
- **attendance**: Leave management and time tracking
- **payroll**: Salary processing and payroll batch management
- **rules**: Business rules and system configurations

This schema separation provides clear domain boundaries and supports complex HRMS business requirements.

## Current Development Status
Based on recent modifications, the following areas have been enhanced:
- Payroll batch management functionality (PayrollBatch model, service, and page)
- Time tracking with advanced filtering and pagination (PunchRecord enhancements)
- Navigation improvements in shared components
- Database schema documentation with comprehensive DDL files
- Dependency injection pattern implementation in Program.cs

# Important Instructions for Development

Do what has been asked; nothing more, nothing less.
NEVER create files unless they're absolutely necessary for achieving your goal.
ALWAYS prefer editing an existing file to creating a new one.
NEVER proactively create documentation files (*.md) or README files. Only create documentation files if explicitly requested by the User.