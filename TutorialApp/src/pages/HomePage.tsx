import React, { useState, useEffect } from "react";
import { Row, Col, Form, Spinner, Alert } from "react-bootstrap";
import CourseCard from "../components/CourseCard";
import axios from "axios";

const HomePage: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const response = await axios.get("https://localhost:7288/api/Courses");
        setCourses(response.data);
      } catch (err) {
        setError("Failed to fetch courses. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  const filteredCourses = courses.filter((course: any) =>
    course.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div>
      <Form.Group className="mb-4">
        <Form.Control
          type="text"
          placeholder="Search for courses..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </Form.Group>

      {loading && (
        <div className="text-center">
          <Spinner animation="border" role="status">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </div>
      )}

      {error && <Alert variant="danger">{error}</Alert>}

      {!loading && !error && (
        <Row>
          {filteredCourses.length > 0 ? (
            filteredCourses.map((course: any) => (
              <Col key={course.id} sm={12} md={6} lg={4} className="mb-4">
                <CourseCard course={course} />
              </Col>
            ))
          ) : (
            <p>No courses found.</p>
          )}
        </Row>
      )}
    </div>
  );
};

export default HomePage;
