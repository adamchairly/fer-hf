import { apiFetch } from './client';

export interface PhotoDto {
  id: string;
  name: string;
  url: string;
  date: string;
}

export function getPhotos(sort?: string) {
  const query = sort ? `?sort=${sort}` : '';
  return apiFetch<PhotoDto[]>(`/photo${query}`);
}

export function uploadPhoto(name: string, file: File) {
  const form = new FormData();
  form.append('name', name);
  form.append('file', file);

  return apiFetch<PhotoDto>('/photo', {
    method: 'POST',
    body: form,
  });
}

export function deletePhoto(id: string) {
  return apiFetch<void>(`/photo/${id}`, { method: 'DELETE' });
}
