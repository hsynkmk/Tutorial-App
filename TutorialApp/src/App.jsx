import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import { AuthProvider, useAuth } from './context/AuthContext';
import CartProvider from './context/CartContext';
import Navbar from './components/Navbar';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProfilePage from './pages/ProfilePage';
import CourseDetailPage from './pages/CourseDetailPage';
import CartPage from './pages/CartPage';
import CheckoutPage from './pages/CheckoutPage';
import ManageCoursesPage from './pages/ManageCoursesPage';
import CourseFormPage from './pages/CourseFormPage';
import ManageUsersPage from './pages/ManageUsersPage';

// CSS imports
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-toastify/dist/ReactToastify.css';
import './App.css';

const PrivateRoute = ({ children, roles }) => {
  const { user, loading } = useAuth();

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!user) {
    return <Navigate to="/login" />;
  }

  if (roles && !roles.includes(user.role)) {
    return <Navigate to="/" />;
  }

  return children;
};

const App = () => {
  return (
    <AuthProvider>
      <CartProvider>
        <Router>
          <div className="min-vh-100 d-flex flex-column">
            <Navbar />
            <main className="flex-grow-1">
              <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/courses/:id" element={<CourseDetailPage />} />
                <Route
                  path="/profile"
                  element={
                    <PrivateRoute>
                      <ProfilePage />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/cart"
                  element={
                    <PrivateRoute>
                      <CartPage />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/checkout"
                  element={
                    <PrivateRoute>
                      <CheckoutPage />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/manage-courses"
                  element={
                    <PrivateRoute roles={['Educator']}>
                      <ManageCoursesPage />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/courses/new"
                  element={
                    <PrivateRoute roles={['Educator']}>
                      <CourseFormPage />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/courses/edit/:id"
                  element={
                    <PrivateRoute roles={['Educator']}>
                      <CourseFormPage />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/manage-users"
                  element={
                    <PrivateRoute roles={['Educator']}>
                      <ManageUsersPage />
                    </PrivateRoute>
                  }
                />
              </Routes>
            </main>
            <footer className="bg-dark text-light py-3 mt-auto">
              <div className="container text-center">
                <p className="mb-0">&copy; 2024 Learning Platform. All rights reserved.</p>
              </div>
            </footer>
          </div>
          <ToastContainer position="top-right" autoClose={3000} />
        </Router>
      </CartProvider>
    </AuthProvider>
  );
};

export default App;
