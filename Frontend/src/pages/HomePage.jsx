import { useState, useEffect } from 'react';
import { courseService } from '../services/api';
import CourseCard from '../components/CourseCard';
import SearchBar from '../components/SearchBar';
import LoadingSpinner from '../components/LoadingSpinner';
import Pagination from '../components/Pagination';

const HomePage = () => {
  const [courses, setCourses] = useState([]);
  const [filteredCourses, setFilteredCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(9);
  const [totalRecords, setTotalRecords] = useState(0);

  useEffect(() => {
    fetchCourses();
  }, [pageNumber, pageSize]);

  const fetchCourses = async () => {
    try {
      const response = await courseService.getAllCourses(pageNumber, pageSize);
      setCourses(response.data.data); // Assuming API response structure includes data
      setFilteredCourses(response.data.data);
      setTotalRecords(response.data.totalRecords); // Assuming totalRecords is returned by the API
    } catch (err) {
      setError('Failed to load courses. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (query) => {
    const filtered = courses.filter((course) =>
      course.name.toLowerCase().includes(query.toLowerCase())
    );
    setFilteredCourses(filtered);
  };

  const handlePageChange = (newPageNumber) => {
    setPageNumber(newPageNumber);
  };

  if (loading) return <LoadingSpinner />;
  if (error) return <div className="alert alert-danger">{error}</div>;

  return (
    <div className="container py-4">
      <h1 className="mb-4">Available Courses</h1>
      <SearchBar onSearch={handleSearch} />

      {filteredCourses.length === 0 ? (
        <p>No courses found.</p>
      ) : (
        <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
          {filteredCourses.map((course) => (
            <div key={course.id} className="col">
              <CourseCard course={course} />
            </div>
          ))}
        </div>
      )}

      <Pagination
        currentPage={pageNumber}
        totalRecords={totalRecords}
        pageSize={pageSize}
        onPageChange={handlePageChange}
      />
    </div>
  );
};

export default HomePage;
