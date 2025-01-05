import axios from 'axios';

// WARNING: SOME API URLs FOR FUTURE REFERENCE (NOT IMPLEMENTED)
const API_URL = 'https://localhost:7288/api'; // API port

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const authService = {
  login: (credentials) => api.post('/users/login', credentials),
  register: (userData) => api.post('/users/register', userData),
  updateProfile: (userData) => api.put('/users/profile', userData),
};

export const userService = {
  getAllUsers: (pageNumber = 1, pageSize = 10) => {
    return api.get('/users', {
      params: { pageNumber, pageSize }
    });
  },
  getUserById: (id) => api.get(`/users/${id}`),
  updateUser: (id, updateData) => api.put(`/users/${id}`, updateData),
  updateUserRole: (userId, role) => api.put(`/users/${userId}/role`, role),
  deleteUser: (id) => api.delete(`/users/${id}`),
};

export const courseService = {
  getAllCourses: (pageNumber = 1, pageSize = 10) => {
    return api.get('/courses', {
      params: { pageNumber, pageSize }
    });
  },
  getCourseById: (id) => api.get(`/courses/${id}`),
  searchCourses: (query) => api.get(`/courses/search?q=${query}`),
  createCourse: (courseData) => api.post('/courses', courseData),
  updateCourse: (id, courseData) => api.put(`/courses/${id}`, courseData),
  deleteCourse: (id) => api.delete(`/courses/${id}`),
  getEducatorCourses: (pageNumber = 1, pageSize = 10) => {
    return api.get('/courses/educator', {
      params: { pageNumber, pageSize } 
    });
  },
};

export const orderService = {
  purchaseCourse: (courseId) => api.post('/orders', { courseId }),
  getUserOrders: () => api.get('/orders/user'),
  getAllOrders: () => api.get('/orders'),
};

export default api; 