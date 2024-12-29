import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { courseService } from '../services/api';
import { toast } from 'react-toastify';
import LoadingSpinner from '../components/LoadingSpinner';

const CourseFormPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(id ? true : false);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: '',
    learningOutcomes: [''],
  });

  useEffect(() => {
    if (id) {
      fetchCourse();
    }
  }, [id]);

  const fetchCourse = async () => {
    try {
      const response = await courseService.getCourseById(id);
      const course = response.data;
      setFormData({
        name: course.name,
        description: course.description,
        price: course.price.toString(),
        learningOutcomes: course.learningOutcomes || [''],
      });
    } catch (error) {
      toast.error('Failed to load course');
      navigate('/manage-courses');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleOutcomeChange = (index, value) => {
    const newOutcomes = [...formData.learningOutcomes];
    newOutcomes[index] = value;
    setFormData((prev) => ({
      ...prev,
      learningOutcomes: newOutcomes,
    }));
  };

  const addOutcome = () => {
    setFormData((prev) => ({
      ...prev,
      learningOutcomes: [...prev.learningOutcomes, ''],
    }));
  };

  const removeOutcome = (index) => {
    if (formData.learningOutcomes.length === 1) return;
    const newOutcomes = formData.learningOutcomes.filter((_, i) => i !== index);
    setFormData((prev) => ({
      ...prev,
      learningOutcomes: newOutcomes,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    const courseData = {
      ...formData,
      price: parseFloat(formData.price),
      learningOutcomes: formData.learningOutcomes.filter(Boolean),
    };

    try {
      if (id) {
        await courseService.updateCourse(id, courseData);
        toast.success('Course updated successfully');
      } else {
        await courseService.createCourse(courseData);
        toast.success('Course created successfully');
      }
      navigate('/manage-courses');
    } catch (error) {
      toast.error(id ? 'Failed to update course' : 'Failed to create course');
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <LoadingSpinner />;

  return (
    <div className="container py-4">
      <h2>{id ? 'Edit Course' : 'Create New Course'}</h2>
      <div className="row">
        <div className="col-lg-8">
          <div className="card">
            <div className="card-body">
              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label htmlFor="name" className="form-label">
                    Course Name
                  </label>
                  <input
                    type="text"
                    className="form-control"
                    id="name"
                    name="name"
                    value={formData.name}
                    onChange={handleChange}
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="description" className="form-label">
                    Description
                  </label>
                  <textarea
                    className="form-control"
                    id="description"
                    name="description"
                    rows="4"
                    value={formData.description}
                    onChange={handleChange}
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="price" className="form-label">
                    Price
                  </label>
                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input
                      type="number"
                      className="form-control"
                      id="price"
                      name="price"
                      min="0"
                      step="0.01"
                      value={formData.price}
                      onChange={handleChange}
                      required
                    />
                  </div>
                </div>
                <div className="mb-3">
                  <label className="form-label">Learning Outcomes</label>
                  {formData.learningOutcomes.map((outcome, index) => (
                    <div key={index} className="input-group mb-2">
                      <input
                        type="text"
                        className="form-control"
                        value={outcome}
                        onChange={(e) =>
                          handleOutcomeChange(index, e.target.value)
                        }
                        placeholder="Enter a learning outcome"
                      />
                      <button
                        type="button"
                        className="btn btn-outline-danger"
                        onClick={() => removeOutcome(index)}
                        disabled={formData.learningOutcomes.length === 1}
                      >
                        Remove
                      </button>
                    </div>
                  ))}
                  <button
                    type="button"
                    className="btn btn-outline-secondary"
                    onClick={addOutcome}
                  >
                    Add Learning Outcome
                  </button>
                </div>
                <div className="d-flex gap-2">
                  <button type="submit" className="btn btn-primary">
                    {id ? 'Update Course' : 'Create Course'}
                  </button>
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => navigate('/manage-courses')}
                  >
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CourseFormPage; 