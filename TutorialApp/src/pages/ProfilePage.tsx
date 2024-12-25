import React from "react";
import { Card, Button } from "react-bootstrap";

const ProfilePage: React.FC = () => {
  const user = {
    name: "John Doe",
    email: "john.doe@example.com",
    courses: [
      { id: 1, name: "React for Beginners", purchasedDate: "2023-12-01" },
      { id: 2, name: "Advanced React", purchasedDate: "2023-12-05" },
    ],
  };

  return (
    <div>
      <Card className="mb-4">
        <Card.Body>
          <Card.Title>Profile</Card.Title>
          <Card.Text>
            <strong>Name:</strong> {user.name}
          </Card.Text>
          <Card.Text>
            <strong>Email:</strong> {user.email}
          </Card.Text>
          <Button variant="primary">Edit Profile</Button>
        </Card.Body>
      </Card>

      <h3>My Courses</h3>
      {user.courses.map((course) => (
        <Card key={course.id} className="mb-3">
          <Card.Body>
            <Card.Title>{course.name}</Card.Title>
            <Card.Text>
              <strong>Purchased Date:</strong> {course.purchasedDate}
            </Card.Text>
          </Card.Body>
        </Card>
      ))}
    </div>
  );
};

export default ProfilePage;
