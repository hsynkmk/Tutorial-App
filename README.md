# Backend Project

Live App: https://hsynkmkstutorialapp.netlify.app/ <br><br>
Educator: educator@gmail.com <br>
Password: Educator1. <br>
<br>
Student: student@gmail.com <br>
Password: Student1.

This repository contains a web application built with **ASP.NET Core** and **React** following the principles of **Clean Architecture**. The project is designed to be modular, scalable, and maintainable, utilizing various modern patterns and libraries. Below is a detailed overview of the technologies and approaches used in the project.

---

## Features

- **Clean Architecture**: Ensures a separation of concerns by organizing the code into layers: `Domain`, `Application`, `Infrastructure`, and `API`.
- **Repository Pattern**: Provides a flexible abstraction layer for data access.
- **Unit of Work**: Manages transactions across multiple repositories.
- **DTOs (Data Transfer Objects)**: Ensures data transfer between layers while maintaining encapsulation.
- **AutoMapper**: Simplifies object-to-object mapping.
- **FluentValidation**: Provides robust validation for input models.
- **Global Exception Handling**: Handles exceptions using a middleware.
- **JWT Authentication**: Implements secure authentication and role-based authorization.
- **Swagger Documentation**: Documents the API and provides an interactive interface for testing endpoints.
- **Seeding**: Seeds initial data into the database for development purposes.
- **Pagination**: Handles efficient pagination for endpoints.
- **CORS Support**: Allows secure cross-origin requests.

---

## Technologies Used

### Frameworks and Libraries

1. **ASP.NET Core**: Core framework for building the API.
2. **Entity Framework Core**: ORM for database access.
3. **FluentValidation**: Validates incoming requests with custom rules.
4. **AutoMapper**: Handles object mapping between models and DTOs.
5. **ASP.NET Identity**: Manages user authentication and authorization.
6. **Swashbuckle**: Integrates Swagger UI for API documentation.

### Patterns

1. **Repository Pattern**: Encapsulates data access logic into reusable repository classes.
2. **Unit of Work**: Ensures atomicity when working with multiple repositories.
3. **Clean Architecture**: Ensures separation of concerns and enforces dependency inversion.

### Middleware

- **Error Handling Middleware**: Centralized error handling for exceptions like validation errors, not found exceptions, and server errors.

---

## Project Structure

```
App.API
├── Controllers          # API controllers to handle HTTP requests
├── Middlewares          # Custom middleware for error handling
├── Properties
├── appsettings.json     # Application configuration
App.Application
├── Common               # Shared classes (e.g., pagination)
├── DTOs                 # Data transfer objects
├── Extensions           # Service registration extensions
├── Interfaces           # Service and repository interfaces
├── Mappings             # AutoMapper profiles
├── Services             # Business logic implementations
├── Validators           # FluentValidation classes
App.Domain
├── Entities             # Core entities
├── Exceptions           # Custom exception classes
App.Infrastructure
├── Extensions           # Infrastructure-level service registration
├── Migrations           # EF Core migrations
├── Repositories         # Repository implementations
├── Seeders              # Database seeders
```

---

## Key Components

### 1. **Middleware for Exception Handling**

Centralized exception handling is implemented through the `ErrorHandlingMiddleware` class. It manages various exception types like:

- `ValidationException`
- `NotFoundException`
- `FailedOperationException`
- Unhandled exceptions (500 Internal Server Error)

### 2. **Authentication & Authorization**

Implemented using **ASP.NET Identity** and **JWT Authentication**. The application supports role-based authorization, ensuring that only authorized users can access certain endpoints.

Swagger UI includes an `Authorize` button for testing endpoints requiring JWT tokens.

### 3. **AutoMapper Integration**

The `AutoMapper` library is used for mapping entities to DTOs and vice versa, reducing boilerplate code in services.

### 4. **Validation**

`FluentValidation` is used to define validation rules for DTOs. Examples:

```csharp
public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(course => course.Name)
            .NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(50).WithMessage("Course name must be less than 50 characters.");

        RuleFor(course => course.Description)
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(course => course.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}
```

### 5. **Repository Pattern and Unit of Work**

Repositories provide abstract data access logic. The `UnitOfWork` pattern coordinates multiple repositories, ensuring consistency and encapsulating transaction management.



## API Endpoints

---

## Orders Controller

### Endpoints

| HTTP Method | Endpoint               | Description                                    | Authorization Required | Parameters                                                                                                                      |
|-------------|------------------------|------------------------------------------------|-------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| **GET**     | `/api/orders/user`     | Get all orders for the current user.           | Yes                    | None                                                                                                                           |
| **GET**     | `/api/orders`          | Get all orders.                                | Yes (Educator Role)     | None                                                                                                                           |
| **POST**    | `/api/orders`          | Create a new order.                            | Yes                    | Request Body: `CreateOrderDto`                                                                                                |

### Notes
- The Educator Role is required for accessing all orders.
- Users can create orders or fetch their own orders.

---

## Users Controller

### Endpoints

| HTTP Method | Endpoint                       | Description                                      | Authorization Required      | Parameters                                                                                                                   |
|-------------|--------------------------------|--------------------------------------------------|------------------------------|-----------------------------------------------------------------------------------------------------------------------------|
| **POST**    | `/api/users/login`            | Log in to the application.                      | No                           | Request Body: `LoginDto`                                                                                                     |
| **POST**    | `/api/users/register`         | Register a new user.                            | No                           | Request Body: `RegisterDto`                                                                                                  |
| **PUT**     | `/api/users/profile`          | Update the current user’s profile.              | Yes                          | Request Body: `UpdateProfileDto`                                                                                            |
| **GET**     | `/api/users`                  | Get all users with pagination.                  | Yes (Educator Role)          | `pageNumber` (int, optional, default: 1), `pageSize` (int, optional, default: 10)                                            |
| **PUT**     | `/api/users/{userId}/role`    | Update a user’s role.                           | Yes                          | `userId` (string, required), Request Body: `newRole` (string, required)                                                     |
| **PUT**     | `/api/users/{userId}`         | Update a user’s details.                        | Yes (Educator Role)          | `userId` (string, required), Request Body: `UpdateUserDto`                                                                   |
| **DELETE**  | `/api/users/{userId}`         | Delete a user by ID.                            | Yes (Educator Role)          | `userId` (string, required)                                                                                                 |
| **GET**     | `/api/users/currentUser`      | Get details of the current authenticated user.  | Yes                          | None                                                                                                                        |

### Notes
- Role management requires appropriate permissions (Educator Role).
- Users can only update their own profiles unless they have Educator access.

---

## Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/hsynkmk/Tutorial-App.git
   ```

2. Navigate to the project directory:
   ```bash
   cd Tutorial-App
   cd Backend
   ```

3. Update the `appsettings.json` file with your database, AllowedOrigins and JWT configuration.

4. Apply migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

6. Access Swagger UI at:
   ```
   https://localhost:{port}/swagger
   ```

---

## Future Improvements

- Implement unit tests and integration tests.
- Add caching for frequently accessed data.
- Support file uploads for course materials.

---

# Frontend Project


## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
