import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';

const CourseCard = ({ course }) => {
  return (
    <div className="card h-100 shadow-sm">
      <div className="card-body">
        <h5 className="card-title">{course.name}</h5>
        <p className="card-text text-truncate">{course.description}</p>
        <div className="d-flex justify-content-between align-items-center">
          <span className="h5 mb-0">${course.price}</span>
          <Link
            to={`/courses/${course.id}`}
            className="btn btn-primary"
          >
            View Details
          </Link>
        </div>
      </div>
    </div>
  );
};

CourseCard.propTypes = {
  course: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
  }).isRequired,
};

export default CourseCard; 