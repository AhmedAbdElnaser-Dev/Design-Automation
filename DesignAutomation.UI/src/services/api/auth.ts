import api from '@/plugins/axios'
import type { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth'

export const authApi = {
  login(payload: LoginRequest) {
    return api.post<AuthResponse>('/users/login', payload).then(r => r.data)
  },

  register(payload: RegisterRequest) {
    return api.post<AuthResponse>('/users/register', payload).then(r => r.data)
  },
}
