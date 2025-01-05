import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { courseService } from '../services/api';
import { toast } from 'react-toastify';
import { FaEdit, FaTrash, FaPlus } from 'react-icons/fa';
import LoadingSpinner from '../components/LoadingSpinner';
import Pagination from '../components/Pagination';

const ManagePublishedCoursesPage = () => {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10,
    totalRecords: 0,
  });

  useEffect(() => {
    fetchCourses();
  }, [pagination.pageNumber, pagination.pageSize]);

  const fetchCourses = async () => {
    try {
      const response = await courseService.getEducatorCourses(
        pagination.pageNumber,
        pagination.pageSize
      );
      setCourses(response.data.data);
      setPagination({
        ...pagination,
        totalRecords: response.data.totalRecords,
      });
    } catch (err) {
      setError('Failed to load courses');
      toast.error(err.response?.data?.Message || 'Failed to load courses');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (courseId) => {
    if (!window.confirm('Are you sure you want to delete this course?')) {
      return;
    }

    try {
      await courseService.deleteCourse(courseId);
      toast.success('Course deleted successfully');
      fetchCourses();
    } catch (error) {
      toast.error(error.response?.data?.Message || 'Failed to delete course');
    }
  };

  const handlePageChange = (newPageNumber) => {
    setPagination({ ...pagination, pageNumber: newPageNumber });
  };

  if (loading) return <LoadingSpinner />;
  if (error) return <div className="alert alert-danger">{error}</div>;

  return (
    <div className="container py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>My Published Courses</h2>
        <Link to="/courses/new" className="btn btn-primary">
          <FaPlus className="me-2" />
          Add New Course
        </Link>
      </div>

      <div className="table-responsive">
        <table className="table table-hover">
          <thead>
            <tr>
              <th style={{ width: '20%' }}>Name</th>
              <th style={{ width: '50%' }}>Description</th>
              <th style={{ width: '15%' }}>Price</th>
              <th style={{ width: '15%' }}>Actions</th>
            </tr>
          </thead>
          <tbody>
            {courses.map((course) => (
              <tr key={course.id}>
                <td>{course.name}</td>
                <td>
                  <div style={{ maxHeight: '3rem', overflow: 'hidden', textOverflow: 'ellipsis' }}>
                    {course.description}
                  </div>
                </td>
                <td>${course.price}</td>
                <td>
                  <div className="btn-group">
                    <Link to={`/courses/edit/${course.id}`} className="btn btn-sm btn-outline-primary">
                      <FaEdit />
                    </Link>
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(course.id)}
                    >
                      <FaTrash />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Pagination
        currentPage={pagination.pageNumber}
        totalRecords={pagination.totalRecords}
        pageSize={pagination.pageSize}
        onPageChange={handlePageChange}
      />
    </div>
  );
};

export default ManagePublishedCoursesPage;
