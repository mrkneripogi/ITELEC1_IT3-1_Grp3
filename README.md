# ITELEC1_IT3-1_Grp3

Car Wash Management System

A Windows Forms–based Automotive Service Management System developed using C# (.NET) and MySQL, designed to streamline daily operations such as customer management, services, payments, employee control, data protection, and audit logging with role-based access control.

📌 Project Overview

The Car Wash Management System is built to help car wash businesses manage customers, employees, services, and transactions efficiently while ensuring data security and accountability.

The system implements encryption, manager override (swipe simulation), and audit logs to protect sensitive customer information and track user actions.

🎯 Objectives

-Automate customer, employee, and service management
-Secure sensitive customer data using encryption
-Implement role-based access control (Admin, Supervisor, Worker)
-Allow temporary access via Manager Override (Swipe)
-Track all critical actions using System Audit Logs
-Improve operational efficiency and data integrity

🧩 System Features
🔐 Data Protection & Security
Sensitive customer fields (Phone, Car Plate Number, Address) are AES-encrypted in the database
Automatic decryption for authorized users (Admin/Supervisor)
Workers see encrypted data unless Manager Override is granted
Protection against unauthorized data exposure

👥 User Roles & Access Control
Role	Permissions
Admin	Full access to all modules and decrypted data
Supervisor	Same as Admin
Worker	Limited access, encrypted data only
Manager Override	Temporary access to sensitive data
🪪 Manager Override (Swipe Simulation)

Simulates card swipe authentication

Temporarily grants sensitive data access

Automatically revoked when navigating to restricted modules

Logged in the audit trail

📋 Audit Logging

Logs user actions such as:

Login / Logout

Manager Override usage

Automatic logout (session timeout)

CRUD operations on customers and employees

Stored in the database

Ready for UI integration (Audit Log Viewer)

⏱ Session Timeout & Auto Logout

Detects user inactivity (1 minute)

Prompts user confirmation

Automatically logs out after 30 seconds of no response

Records auto logout in audit logs

🧾 Core Modules

Login & Authentication

Customer Management

Employee Management

Service Management

Cash & Transactions

Reports

Audit Logs (Backend Ready)

🛠 Technologies Used
Technology	Description
C# (.NET Framework)	Application logic
Windows Forms	Desktop UI
MySQL	Database
AES Encryption	Data security
MySql.Data	Database connector
🗄 Database Design (Key Tables)

tbcustomer

tbemployee

tbservice

tbcash

tbauditlog

tbvehicletype

Sensitive fields are stored encrypted at rest.

🔐 Encryption Strategy

AES encryption with a fixed secret key

Data encrypted before saving to the database

Decryption performed only when:

User is Admin or Supervisor

Manager Override is active

Base64 validation prevents decryption errors

🧠 Access Control Logic
IsAdminOrSupervisor()
CanViewCustomerSensitive()
CanAlterCustomer()
CanAlterEmployer()


These methods ensure centralized permission handling across all modules.

📁 Project Structure
carWashManagement/
│
├── Main.cs
├── Login.cs
├── Customer.cs
├── Employer.cs
├── Service.cs
├── Cash.cs
│
├── Helpers/
│   ├── CryptoHelper.cs
│   ├── AccessControl.cs
│   ├── AuditLogger.cs
│
├── Database/
│   └── dbConnect.cs
│
└── README.md

🚀 How to Run the Project

Clone the repository

git clone https://github.com/yourusername/car-wash-management-system.git


Open the solution in Visual Studio

Import the MySQL database

Create database: carwashmanagement

Import provided .sql file (if available)

Update database credentials in dbConnect.cs

Run the application

⚠️ Important Notes

Ensure all sensitive fields are encrypted consistently

Never decrypt values without Base64 validation

Manager Override access is temporary by design

Audit logs are mandatory for compliance and monitoring

📌 Future Enhancements

Audit Log UI viewer

Role management UI

Password hashing with salt

Backup and disaster recovery

Web-based version

👨‍💻 Author

Mark Joven Neri
BSIT Student
Polytechnic University of the Philippines

📜 License

This project is for academic and educational purposes.
Commercial use requires proper authorization.
