import axios from 'axios';

const API_URL = 'https://localhost:7288/api'; // Update this to match your .NET Core API port

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

export const courseService = {
  getAllCourses: () => api.get('/courses'),
  getCourseById: (id) => api.get(`/courses/${id}`),
  searchCourses: (query) => api.get(`/courses/search?q=${query}`),
  createCourse: (courseData) => api.post('/courses', courseData),
  updateCourse: (id, courseData) => api.put(`/courses/${id}`, courseData),
  deleteCourse: (id) => api.delete(`/courses/${id}`),
};

export const orderService = {
  purchaseCourse: (courseId) => api.post('/orders', { courseId }),
  getUserOrders: () => api.get('/orders/user'),
  getAllOrders: () => api.get('/orders'), // For educators
};

export default api; 