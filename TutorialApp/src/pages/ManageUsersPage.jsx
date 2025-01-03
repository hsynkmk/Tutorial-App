import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { userService } from '../services/api';
import { toast } from 'react-toastify';
import { FaUserGraduate, FaChalkboardTeacher, FaEdit, FaTrash } from 'react-icons/fa';
import LoadingSpinner from '../components/LoadingSpinner';

const ManageUsersPage = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editMode, setEditMode] = useState(null);
  const [editData, setEditData] = useState({
    fullName: '',
    email: '',
    role: '',
    password: ''
  });

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await userService.getAllUsers();
      setUsers(response.data);
    } catch (err) {
      setError('Failed to load users');
      toast.error('Failed to load users');
    } finally {
      setLoading(false);
    }
  };

  const handleRoleChange = async (userId, newRole) => {
    try {
      await userService.updateUserRole(userId, newRole);
      toast.success('User role updated successfully');
      fetchUsers();
    } catch (error) {
      toast.error('Failed to update user role');
    }
  };

  const handleDelete = async (userId) => {
    if (!window.confirm('Are you sure you want to delete this user?')) {
      return;
    }

    try {
      await userService.deleteUser(userId);
      toast.success('User deleted successfully');
      fetchUsers();
    } catch (error) {
      toast.error('Failed to delete user');
    }
  };

  const handleEdit = (user) => {
    setEditMode(user.id);
    setEditData({
      fullName: user.fullName,
      email: user.email,
      role: user.role,
      password: ''
    });
  };

  const handleEditSubmit = async (userId) => {
    try {
      const updateData = {
        id: userId,
        fullName: editData.fullName,
        email: editData.email,
        currentPassword: editData.currentPassword || '',
        newPassword: editData.newPassword || '',
      };

      await userService.updateUser(userId, updateData);
      toast.success("User updated successfully");
      setEditMode(null);
      fetchUsers();
    } catch (error) {
      console.error(error);
      toast.error("Failed to update user");
    }
  };


  const handleEditCancel = () => {
    setEditMode(null);
    setEditData({
      fullName: '',
      email: '',
      role: '',
      password: ''
    });
  };

  if (loading) return <LoadingSpinner />;
  if (error) return <div className="alert alert-danger">{error}</div>;

  return (
    <div className="container py-4">
      <h2 className="mb-4">Manage Users</h2>
      <div className="table-responsive">
        <table className="table table-hover">
          <thead>
            <tr>
              <th>Name</th>
              <th>Email</th>
              <th>Role</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {users.map((user) => (
              <tr key={user.id}>
                <td>
                  {editMode === user.id ? (
                    <input
                      type="text"
                      className="form-control"
                      value={editData.fullName}
                      onChange={(e) => setEditData({ ...editData, fullName: e.target.value })}
                    />
                  ) : (
                    user.fullName
                  )}
                </td>
                <td>
                  {editMode === user.id ? (
                    <>
                      <input
                        type="email"
                        className="form-control mb-2"
                        value={editData.email}
                        onChange={(e) => setEditData({ ...editData, email: e.target.value })}
                      />
                      <input
                        className="form-control"
                        placeholder="New Password (optional)"
                        value={editData.password}
                        onChange={(e) => setEditData({ ...editData, password: e.target.value })}
                      />
                    </>
                  ) : (
                    user.email
                  )}
                </td>
                <td>
                  <span className="badge bg-primary">
                    {user.role === 'Educator' ? (
                      <FaChalkboardTeacher className="me-1" />
                    ) : (
                      <FaUserGraduate className="me-1" />
                    )}
                    {user.role}
                  </span>
                </td>
                <td>
                  <div className="d-flex gap-2">
                    {editMode === user.id ? (
                      <>
                        <button
                          className="btn btn-success btn-sm"
                          onClick={() => handleEditSubmit(user.id)}
                        >
                          Save
                        </button>
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={handleEditCancel}
                        >
                          Cancel
                        </button>
                      </>
                    ) : (
                      <>
                        <select
                          className="form-select form-select-sm w-auto"
                          value={user.role}
                          onChange={(e) => handleRoleChange(user.id, e.target.value)}
                        >
                          <option value="Student">Student</option>
                          <option value="Educator">Educator</option>
                        </select>
                        <button
                          className="btn btn-outline-primary btn-sm"
                          onClick={() => handleEdit(user)}
                        >
                          <FaEdit />
                        </button>
                        <button
                          className="btn btn-outline-danger btn-sm"
                          onClick={() => handleDelete(user.id)}
                        >
                          <FaTrash />
                        </button>
                      </>
                    )}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ManageUsersPage; 