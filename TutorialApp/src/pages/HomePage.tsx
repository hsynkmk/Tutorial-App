import React, { useState } from "react";
import { Row, Col, Form } from "react-bootstrap";
import CourseCard from "../components/CourseCard";

const HomePage: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState("");

  // Replace with API data
  const courses = [
    {
      id: 1,
      name: "React for Beginners",
      description: "Learn React basics",
      price: 100,
    },
    {
      id: 2,
      name: "Advanced React",
      description: "Dive deep into React",
      price: 200,
    },
  ];

  const filteredCourses = courses.filter((course) =>
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
      <Row>
        {filteredCourses.length > 0 ? (
          filteredCourses.map((course) => (
            <Col key={course.id} sm={12} md={6} lg={4} className="mb-4">
              <CourseCard course={course} />
            </Col>
          ))
        ) : (
          <p>No courses found.</p>
        )}
      </Row>
    </div>
  );
};

export default HomePage;
