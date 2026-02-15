# Drivers and Vehicles Licenses Department (DVLD)

## Overview
DVLD is a comprehensive desktop application designed to manage the operations of a Department of Motor Vehicles (DMV) or similar licensing authority. It handles the issuance, renewal, and management of driving licenses, as well as the management of drivers, tests, and detained licenses.

The application is built using the **Microsoft .NET Framework** (Windows Forms) and follows a **3-Tier Architecture** to ensure separation of concerns and maintainability.

## Features

### 👥 People Management
*   **Manage People**: Add, update, delete, and list people in the system.
*   **Search**: Filter and search for people by various criteria (National No, Name, etc.).
*   **Details**: View detailed personal information including photos and addresses.

### 👤 User Management
*   **System Users**: Manage administrative users who can access the system.
*   **Permissions**: (Implicit in design) Control access to the system.
*   **Security**: Change password functionality and secure login.

### 🚗 Drivers & Licenses
*   **Drivers**: List and manage consolidated driver information.
*   **Local Licenses**:
    *   Issue new local driving licenses.
    *   Renew existing licenses.
    *   Replace lost or damaged licenses.
*   **International Licenses**:
    *   Issue international driving licenses based on valid local licenses.
    *   Manage international license applications.

### 📝 Tests & Appointments
*   **Test Types**: specific test types (Vision, Written, Street).
*   **Appointments**: Schedule tests for applicants.
*   **Test Results**: Record and manage the results of driving tests.
*   **Retake Tests**: Handle procedures for retaking failed tests.

### 👮 Detained Licenses
*   **Detain License**: Suspend or detain a driver's license (e.g., for traffic violations).
*   **Release License**: Process the release of detained licenses after fines or periods are served.
*   **Management**: View and manage all currently detained licenses.

### ⚙️ Application Management
*   **Application Types**: Manage different types of services/applications offered by the department and their fees.

## Architecture

The project is structured using a strict **3-Tier Architecture**:

1.  **Presentation Tier (`DVLD_PresentationTier`)**:
    *   Built with Windows Forms (.NET Framework 4.8).
    *   Handles all user interactions, input validation, and UI rendering.
    *   Communicates *only* with the Business Layer.

2.  **Business Layer (`DVLDBusinessLayer`)**:
    *   Contains the core business logic and rules.
    *   Entities include `clsPerson`, `clsDriver`, `clsLicense`, etc.
    *   Validates data before sending it to the Data Access Layer.

3.  **Data Access Layer (`DVLDDataAccessLayer`)**:
    *   Handles all communication with the SQL Server database.
    *   Executes stored procedures or SQL queries.
    *   Returns raw data (DataTables/DTOs) to the Business Layer.

*   **Domain Layer (`DVLD_Domain`)**: (Optional/Helper) Contains shared data models or DTOs used across layers.

## Technology Stack
*   **Language**: C#
*   **Framework**: .NET Framework 4.8
*   **UI**: Windows Forms (WinForms)
*   **Database**: Microsoft SQL Server
*   **Configuration**: DotNetEnv (for environment variables)

## Setup & Installation

### Prerequisites
*   Visual Studio 2019 or later (with .NET Desktop Development workload).
*   Microsoft SQL Server.

### Database Setup
1.  Open SQL Server Management Studio (SSMS).
2.  Restore the database from the backup file located at:  
    `Drivers-and-Vehicles-Licenses-Department/Database/DVLD.bak`
3.  Ensure the restored database is named `DVLD` (or update your configuration to match).

### Configuration
The application uses a `.env` file for database connection configuration.

1.  Create a `.env` file in the `DVLD_PresentationTier` directory (or check the code in `Program.cs` for the specific path expected).
2.  Add the following variables to the `.env` file:
    ```env
    DB_SERVER=YourServerName
    DB_NAME=DVLD
    DB_USER=YourUsername
    DB_PASSWORD=YourPassword
    ```
    *(Note: Check strictly `Program.cs` configuration logic if the file is not found).*

### Running the Application
1.  Open `DVLD_PresentationTier.sln` is Visual Studio (or open the project file).
2.  Set `DVLD_PresentationTier` as the **StartUp Project**.
3.  Build the solution (Ctrl+Shift+B).
4.  Run the application (F5).

## Contributing
1.  Fork the repository.
2.  Create a feature branch (`git checkout -b feature/AmazingFeature`).
3.  Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4.  Push to the branch (`git push origin feature/AmazingFeature`).
5.  Open a Pull Request.
