# DVLD Architecture

## High-Level Architecture
The application follows a strict **3-Tier Architecture** pattern, separating the Presentation (UI), Business Logic, and Data Access layers.

```mermaid
graph TD
    UI[Presentation Tier (WinForms)] --> BL[Business Layer (Logic)]
    BL --> DL[Data Access Layer (ADO.NET)]
    DL --> DB[(SQL Server Database)]
    
    subgraph Layers
    UI
    BL
    DL
    end
```

## detailed Layer Breakdown

### 1. Presentation Tier (`DVLD_PresentationTier`)
*   **Responsibility**: Displaying data to the user and capturing user input. It should contain minimal logic, only related to UI validation and display formatting.
*   **Key Components**:
    *   **Forms (`frm...`)**: Represents screens like `frmLogin`, `frmManagePeople`.
    *   **Controls (`ctrl...`)**: Reusable UI components like `ctrlPersonCard`.
    *   **Program.cs**: Entry point.

### 2. Business Layer (`DVLDBusinessLayer`)
*   **Responsibility**: Core logic, calculations, and rules. It acts as an intermediary between the UI and Data Access layers.
*   **Key Classes**:
    *   `clsPerson`: Manages person data logic.
    *   `clsUser`: Authentication and user management logic.
    *   `clsLicense`: Driving license issuance and validation rules.
*   **Validation**: Ensures data integrity before passing it to the data layer (e.g., checking if a user already exists).

### 3. Data Access Layer (`DVLDDataAccessLayer`)
*   **Responsibility**: Direct interaction with the database. Executes SQL commands and stored procedures.
*   **Key Components**:
    *   `clsDataAccessSettings`: Handles connection strings (via `.env`).
    *   `cls...Data` classes: Static methods for CRUD operations (e.g., `clsPersonData.AddNewPerson`).

## Key Workflows

### User Login
1.  **UI (`frmLogin`)**: User enters username/password.
2.  **Business (`clsUser.FindByUsernameAndPassword`)**: Hashes the password (if implemented) and queries the data layer.
3.  **Data (`clsUserData.GetUserInfoByUsernameAndPassword`)**: Executes SQL query to find matching user.
4.  **Result**: Returns user object to UI if successful.

### Issuing a License
1.  **UI**: Collects application processing fee, class selection.
2.  **Business**:
    *   Verifies applicant age and eligibility.
    *   Checks for passed tests.
    *   Calculates expiration date.
3.  **Data**: Inserts new license record into `Licenses` table.

## Deployment
*   **Database**: SQL Server 2012+
*   **Client**: Windows 10/11 with .NET Framework 4.8 Runtime.
