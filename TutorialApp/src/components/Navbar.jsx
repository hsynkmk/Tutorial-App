import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import { useTheme } from '../context/ThemeContext';
import { FaUser, FaSignOutAlt, FaShoppingCart, FaSun, FaMoon } from 'react-icons/fa';

const Navbar = () => {
  const { user, logout } = useAuth();
  const { getCartCount, clearCart } = useCart();
  const { darkMode, toggleTheme } = useTheme();
  const navigate = useNavigate();

  const handleLogout = () => {
    clearCart();
    logout();
    navigate('/');
  };

  return (
    <nav className={`navbar navbar-expand-lg ${darkMode ? 'navbar-dark' : 'navbar-light'}`}>
      <div className="container">
        <Link className="navbar-brand" to="/">
          Learning Platform
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <Link className="nav-link" to="/">
                Home
              </Link>
            </li>
            {user?.role === 'Educator' && (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/manage-courses">
                    Manage All Courses
                  </Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/my-courses">
                    My Published Courses
                  </Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/manage-users">
                    Manage Users
                  </Link>
                </li>
              </>
            )}
          </ul>
          <ul className="navbar-nav">
            <li className="nav-item">
              <button
                className="nav-link btn btn-link"
                onClick={toggleTheme}
                title={darkMode ? 'Switch to Light Mode' : 'Switch to Dark Mode'}
              >
                {darkMode ? <FaSun className="text-warning" /> : <FaMoon />}
              </button>
            </li>
            <li className="nav-item">
              <Link className="nav-link position-relative" to="/cart">
                <FaShoppingCart size={20} />
                {getCartCount() > 0 && (
                  <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                    {getCartCount()}
                  </span>
                )}
              </Link>
            </li>
            {user ? (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/profile">
                    <FaUser className="me-1" />
                    Profile
                  </Link>
                </li>
                <li className="nav-item">
                  <button
                    className="nav-link btn btn-link"
                    onClick={handleLogout}
                  >
                    <FaSignOutAlt className="me-1" />
                    Logout
                  </button>
                </li>
              </>
            ) : (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/login">
                    Login
                  </Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/register">
                    Register
                  </Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar; 