import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { courseService } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import LoadingSpinner from '../components/LoadingSpinner';
import { toast } from 'react-toastify';
import { FaShoppingCart } from 'react-icons/fa';

const CourseDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const { addToCart } = useCart();
  const [course, setCourse] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCourse = async () => {
      try {
        const response = await courseService.getCourseById(id);
        setCourse(response.data);
      } catch (err) {
        setError('Failed to load course details. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchCourse();
  }, [id]);

  const handleAddToCart = () => {
    if (!user) {
      toast.info('Please log in to add courses to cart');
      navigate('/login');
      return;
    }

    addToCart(course);
    toast.success('Course added to cart!');
  };

  if (loading) return <LoadingSpinner />;
  if (error) return <div className="alert alert-danger">{error}</div>;
  if (!course) return <div className="alert alert-warning">Course not found</div>;

  return (
    <div className="container py-4">
      <div className="row">
        <div className="col-lg-8">
          <h1 className="mb-4">{course.name}</h1>
          <div className="card mb-4">
            <div className="card-body">
              <h5 className="card-title">Course Description</h5>
              <p className="card-text">{course.description}</p>
              <div className="mt-4">
                <h5>What you'll learn</h5>
                <ul className="list-group list-group-flush">
                  {course.learningOutcomes?.map((outcome, index) => (
                    <li key={index} className="list-group-item">
                      {outcome}
                    </li>
                  ))}
                  Coming soon...
                </ul>
              </div>
            </div>
          </div>
        </div>
        <div className="col-lg-4">
          <div className="card">
            <div className="card-body">
              <h5 className="card-title">Price</h5>
              <p className="h2 mb-4">${course.price}</p>
              <button
                className="btn btn-primary w-100 mb-3"
                onClick={handleAddToCart}
              >
                <FaShoppingCart className="me-2" />
                Add to Cart
              </button>
              {!user && (
                <p className="text-muted text-center">
                  Please log in to purchase this course
                </p>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CourseDetailPage; 