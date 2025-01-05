# Frontend Project

Live App: https://hsynkmkstutorialapp.netlify.app/ <br><br>
Educator: educator@gmail.com <br>
Password: Educator1. <br>
<br>
Student: student@gmail.com <br>
Password: Student1.

This repository contains a web application built with **ASP.NET Core** and **React** following the principles of **Clean Architecture**. The project is designed to be modular, scalable, and maintainable, utilizing various modern patterns and libraries. Below is a detailed overview of the technologies and approaches used in the project.

---



This is the frontend of the **TutorialApp**, a learning platform designed for users to explore, purchase, and manage courses. The application supports both learners and educators, with role-based functionalities such as managing courses and users. The frontend is built with **Vite**, **React**, and several modern web development tools.

---

## Features

### User Roles
- **Learner**: Browse courses, view course details, add courses to the cart, and checkout.
- **Educator**: Manage courses, publish courses, and manage users (for admin-level access).

### Key Features
- Authentication (Login/Logout/Register) with JWT
- Light/Dark mode support using a custom theme context
- Responsive design using Bootstrap
- Dynamic routing with React Router
- API integration with Axios for backend communication
- Toast notifications for a better user experience
- Role-based access control for private routes

---

## Tech Stack

### Frontend Tools and Libraries
- **Vite**: Build tool for fast development
- **React**: Library for building the UI
- **React Router**: For routing and navigation
- **Axios**: For HTTP requests and API communication
- **React Bootstrap**: For responsive and mobile-friendly design
- **React Icons**: For reusable icons
- **React Toastify**: For toast notifications
- **PropTypes**: For type-checking React props
- **JWT Decode**: To decode JWT tokens for authentication
- **Bootstrap**: CSS framework for styling

---


---

## How It Works

### Authentication
- **Context**: `AuthContext` manages user authentication and role-based access.
- JWT token is stored in `localStorage` for session persistence.
- Decoded JWT token provides user details (name, email, role).

### Theme
- **Context**: `ThemeContext` toggles between light and dark themes.
- Components dynamically adjust styles based on the current theme.

### Cart Management
- **Context**: `CartContext` manages the shopping cart, providing functions to add, remove, and clear cart items.

### API Integration
- Axios interceptors ensure that authenticated requests include a valid JWT token in the `Authorization` header.
- Separate services (`authService`, `userService`, `courseService`, `orderService`) organize API calls for clarity and reuse.

### Private Routing
- Private routes restrict access to pages based on user roles (e.g., Educator-only pages).
- Unauthorized access redirects users to the login page or home.

---

## Installation and Setup

### Prerequisites
- **Node.js**
- **npm** or **yarn**

### Steps to Run Locally
1. Clone the repository:
   ```bash
   git clone https://github.com/hsynkmk/Tutorial-App.git
   cd Tutorial-App
   cd Backend
    ```


## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
