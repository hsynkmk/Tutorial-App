import React from "react";
import { Card, Button } from "react-bootstrap";
import { Link } from "react-router-dom";

interface CourseCardProps {
  course: {
    id: number;
    name: string;
    description: string;
    price: number;
  };
}

const CourseCard: React.FC<CourseCardProps> = ({ course }) => {
  return (
    <Card>
      <Card.Body>
        <Card.Title>{course.name}</Card.Title>
        <Card.Text className="text-truncate" style={{ maxHeight: "3em" }}>
          {course.description}
        </Card.Text>
        <Card.Text>
          <strong>Price:</strong> ${course.price}
        </Card.Text>
        <Button as={Link} to={`/courses/${course.id}`} variant="primary">
          View Details
        </Button>
      </Card.Body>
    </Card>
  );
};

export default CourseCard;
