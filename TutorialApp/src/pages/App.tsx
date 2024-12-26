import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import NavbarComponent from "../components/NavbarComponent";
import HomePage from "../pages/HomePage";
import ProfilePage from "../pages/ProfilePage";
import CourseDetailsPage from "../pages/CourseDetailsPage";
import LoginPage from "../pages/LoginPage";
import RegisterPage from "../pages/RegisterPage";
import ListCoursesPage from "../pages/ListCoursesPage";
import CreateCoursePage from "../pages/CreateCoursePage";
import UpdateCoursePage from "../pages/UpdateCoursePage";

const App: React.FC = () => {
  return (
    <Router>
      <NavbarComponent />
      <div className="container mt-4">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/courses/:id" element={<CourseDetailsPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/admin/courses" element={<ListCoursesPage />} />
          <Route path="/admin/courses/create" element={<CreateCoursePage />} />
          <Route
            path="/admin/courses/edit/:id"
            element={<UpdateCoursePage />}
          />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
