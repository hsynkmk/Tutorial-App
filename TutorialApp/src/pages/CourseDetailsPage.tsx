import React from "react";
import { useParams } from "react-router-dom";
import { Card, Button, Alert } from "react-bootstrap";

const CourseDetailsPage: React.FC = () => {
  const { id } = useParams();

  // Replace with API data
  const course = {
    id: 1,
    name: "React for Beginners",
    description: "Learn React basics from scratch",
    price: 100,
    category: "Web Development",
  };

  const isLoggedIn = false; // Replace with auth logic

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
            Please <a href="/login">log in</a> to purchase this course.
          </Alert>
        )}
      </Card.Body>
    </Card>
  );
};

export default CourseDetailsPage;
