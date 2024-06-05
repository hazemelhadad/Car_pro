# Vehicles & Employees Management System

This project is a comprehensive system designed to manage vehicles and employees within a company. It provides functionalities for registering new employees, assigning roles, managing vehicles, and tracking vehicle usage.

## Technologies Used

- **ASP.NET Core Web API**: The backend of the system is developed using ASP.NET Core Web API, providing RESTful endpoints for communication with the frontend.
  
- **Entity Framework**: Entity Framework is used as the ORM (Object-Relational Mapper) for interacting with the database, making database operations easier and more efficient.

- **Angular 17**: The frontend of the system is built using Angular 17, a modern and powerful framework for building single-page web applications.

- **Microsoft SQL Server**: The data storage for the system is implemented using Microsoft SQL Server, a robust relational database management system.

## Functionality

### Account Management

- **Registration**: New employees can register for an account by providing their employee ID and password.
  
- **Login**: Registered employees can log in to the system using their credentials.

### Employee Management

- **Admin Interface**: Administrators have access to functionalities such as adding, updating, and deleting employees.
  
- **Role Assignment**: Employees can be assigned roles such as Admin or SuperAdmin.

### Vehicle Management

- **Add Vehicles**: Administrators can add new vehicles to the system, specifying details such as plate number, type, and branch ID.
  
- **Assign Vehicles**: Employees can be assigned to vehicles, tracking which employee is using which vehicle.

## Usage Instructions

1. Clone the repository to your local machine.
2. Set up the backend by opening the project in Visual Studio and configuring the database connection string in `appsettings.json`.
3. Run the backend project to start the ASP.NET Core Web API server.
4. Set up the frontend by navigating to the `client` directory and installing dependencies using `npm install`.
5. Run the frontend using `ng serve` to start the Angular development server.
6. Access the application in your web browser by navigating to `http://localhost:4200`.
7. Register a new account or log in with existing credentials.
8. Use the provided functionalities to manage employees and vehicles within the system.

Enjoy managing your company's vehicles and employees efficiently with the Vehicles & Employees Management System!
