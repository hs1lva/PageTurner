// authService.js
import { Navigate } from 'react-router-dom';
import useAuthStore from './authService';

export const RequireLoginRoute = ({ Component }) => {
  const store = useAuthStore();
  const isAuthenticated = store.isAuthenticated();

  return isAuthenticated ? <Component /> : <Navigate to="/login" />;
};

export const RequireGuestRoute = ({ Component }) => {
  const store = useAuthStore();
  const isAuthenticated = store.isAuthenticated();

  return isAuthenticated ? <Navigate to="/Leitor" /> : <Component />;
};