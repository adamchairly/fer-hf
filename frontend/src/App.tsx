import { Routes, Route, Navigate } from "react-router";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import PhotosPage from "./pages/PhotosPage";

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/photos" element={<PhotosPage />} />
      <Route path="*" element={<Navigate to="/photos" replace />} />
    </Routes>
  );
}
