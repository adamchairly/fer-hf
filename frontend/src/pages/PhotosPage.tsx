import { useEffect, useRef, useState, type FormEvent } from "react";
import { useNavigate } from "react-router";
import {
  getPhotos,
  uploadPhoto,
  deletePhoto,
  type PhotoDto,
} from "../api/photos";
import { logout } from "../api/auth";
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";

export default function PhotosPage() {
  const navigate = useNavigate();
  const [photos, setPhotos] = useState<PhotoDto[]>([]);
  const [sort, setSort] = useState<string>("date");
  const [lightboxIndex, setLightboxIndex] = useState(-1);
  const [name, setName] = useState("");
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState("");
  const fileInputRef = useRef<HTMLInputElement>(null);

  async function fetchPhotos() {
    try {
      const data = await getPhotos(sort);
      setPhotos(data);
    } catch {
      navigate("/login");
    }
  }

  useEffect(() => {
    fetchPhotos();
  }, [sort]);

  async function handleUpload(e: FormEvent) {
    e.preventDefault();
    setError("");

    if (!name || !file) {
      setError("Name and file are required.");
      return;
    }
    if (name.length > 40) {
      setError("Name must be at most 40 characters.");
      return;
    }

    try {
      await uploadPhoto(name, file);
      setName("");
      setFile(null);
      if (fileInputRef.current) fileInputRef.current.value = "";
      await fetchPhotos();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Upload failed.");
    }
  }

  async function handleDelete(id: string) {
    try {
      await deletePhoto(id);
      setPhotos((prev) => prev.filter((p) => p.id !== id));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Delete failed.");
    }
  }

  async function handleLogout() {
    await logout();
    navigate("/login");
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow px-6 py-4 flex items-center justify-between">
        <h1 className="text-xl font-bold">My Photos</h1>
        <button
          onClick={handleLogout}
          className="text-sm text-red-600 hover:underline cursor-pointer"
        >
          Logout
        </button>
      </nav>

      <div className="max-w-4xl mx-auto p-6 space-y-6">
        {/* Upload form */}
        <form
          onSubmit={handleUpload}
          className="bg-white p-4 rounded-lg shadow flex flex-col sm:flex-row gap-3 items-end"
        >
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Photo name (max 40 chars)
            </label>
            <input
              type="text"
              maxLength={40}
              required
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="flex items-center gap-2">
            <input
              ref={fileInputRef}
              type="file"
              accept="image/*"
              required
              onChange={(e) => setFile(e.target.files?.[0] ?? null)}
              className="hidden"
            />
            <button
              type="button"
              onClick={() => fileInputRef.current?.click()}
              className="border border-gray-300 rounded px-3 py-2 text-sm text-gray-700 hover:bg-gray-50 transition cursor-pointer"
            >
              Choose file
            </button>
            <span className="text-sm text-gray-500 truncate max-w-48">
              {file ? file.name : "No file chosen"}
            </span>
          </div>
          <button
            type="submit"
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition cursor-pointer"
          >
            Upload
          </button>
        </form>

        {error && <p className="text-red-600 text-sm">{error}</p>}

        {/* Sort toggle */}
        <div className="flex gap-2 text-sm">
          <span className="text-gray-600">Sort by:</span>
          <button
            onClick={() => setSort("name")}
            className={`cursor-pointer ${sort === "name" ? "font-bold text-blue-600" : "text-gray-500 hover:text-gray-800"}`}
          >
            Name
          </button>
          <button
            onClick={() => setSort("date")}
            className={`cursor-pointer ${sort === "date" ? "font-bold text-blue-600" : "text-gray-500 hover:text-gray-800"}`}
          >
            Date
          </button>
        </div>

        {/* Photo list */}
        {photos.length === 0 ? (
          <p className="text-gray-500 text-center py-8">
            No photos yet. Upload one above!
          </p>
        ) : (
          <div className="grid gap-3">
            {photos.map((photo, index) => (
              <div
                key={photo.id}
                className="bg-white rounded-lg shadow px-4 py-3 flex items-center justify-between"
              >
                <button
                  onClick={() => setLightboxIndex(index)}
                  className="text-left flex-1 cursor-pointer hover:text-blue-600 transition"
                >
                  <span className="font-medium">{photo.name}</span>
                  <span className="text-gray-500 text-sm ml-3">
                    {photo.date}
                  </span>
                </button>
                <button
                  onClick={() => handleDelete(photo.id)}
                  className="text-red-500 hover:text-red-700 text-sm ml-4 cursor-pointer"
                >
                  Delete
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Lightbox */}
      <Lightbox
        open={lightboxIndex >= 0}
        close={() => setLightboxIndex(-1)}
        index={lightboxIndex}
        slides={photos.map((p) => ({ src: p.url }))}
      />
    </div>
  );
}
