import { apiFetch } from './client';

export function login(email: string, password: string) {
  return apiFetch<void>('/user/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password }),
  });
}

export function register(email: string, password: string) {
  return apiFetch<void>('/user/register', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password }),
  });
}

export function logout() {
  return apiFetch<void>('/user/logout', { method: 'POST' });
}
