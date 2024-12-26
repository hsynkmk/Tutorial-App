import React, { useState, useEffect } from "react";
import { Table, Button, Alert, Spinner } from "react-bootstrap";
import axios from "axios";

const ListCoursesPage: React.FC = () => {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");

  const fetchCourses = async () => {
    try {
      setLoading(true);
      const response = await axios.get("https://localhost:7288/api/Courses");
      setCourses(response.data);
    } catch (error) {
      setErrorMessage("Failed to fetch courses.");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await axios.delete(`https://localhost:7288/api/Courses/${id}`);
      setCourses(courses.filter((course: any) => course.id !== id));
    } catch (error) {
      setErrorMessage("Failed to delete course.");
    }
  };

  useEffect(() => {
    fetchCourses();
  }, []);

  if (loading) {
    return (
      <div className="text-center">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </div>
    );
  }

  return (
    <div>
      <h3>Manage Courses</h3>
      {errorMessage && <Alert variant="danger">{errorMessage}</Alert>}
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Description</th>
            <th>Category</th>
            <th>Price</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {courses.map((course: any) => (
            <tr key={course.id}>
              <td>{course.id}</td>
              <td>{course.name}</td>
              <td>{course.description}</td>
              <td>{course.category}</td>
              <td>${course.price}</td>
              <td>
                <Button
                  variant="danger"
                  onClick={() => handleDelete(course.id)}
                >
                  Delete
                </Button>{" "}
                <Button
                  variant="warning"
                  href={`/admin/courses/edit/${course.id}`}
                >
                  Edit
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default ListCoursesPage;
