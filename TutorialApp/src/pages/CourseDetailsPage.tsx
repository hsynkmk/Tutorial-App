import React, { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import { Card, Button, Alert, Spinner } from "react-bootstrap";
import axios from "axios";

const CourseDetailsPage: React.FC = () => {
  const { id } = useParams();
  const [course, setCourse] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const isLoggedIn = false; // Replace with actual authentication logic

  useEffect(() => {
    const fetchCourse = async () => {
      try {
        setLoading(true);
        const response = await axios.get(
          `https://localhost:7288/api/Courses/${id}`
        );
        setCourse(response.data);
      } catch (err) {
        setError("Failed to fetch course details. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchCourse();
  }, [id]);

  if (loading) {
    return (
      <div className="text-center">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </div>
    );
  }

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  if (!course) {
    return <Alert variant="warning">Course not found.</Alert>;
  }

  return (
    <Card>
      <Card.Body>
        <Card.Title>{course.name}</Card.Title>
        <Card.Text>
          <strong>Description:</strong> {course.description}
        </Card.Text>
        <Card.Text>
          <strong>Price:</strong> ${course.price}
        </Card.Text>
        <Card.Text>
          <strong>Category:</strong> {course.category}
        </Card.Text>
        {isLoggedIn ? (
          <Button variant="success">Buy Now</Button>
        ) : (
          <Alert variant="warning">
            Please <Link to="/login">log in</Link> to purchase this course.
          </Alert>
        )}
      </Card.Body>
    </Card>
  );
};

export default CourseDetailsPage;
