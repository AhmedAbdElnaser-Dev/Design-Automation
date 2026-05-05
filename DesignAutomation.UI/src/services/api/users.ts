import api from '@/plugins/axios'
import type { User } from '../types/user'

export const usersApi = {
  me() {
    return api.get<User>('/users/me').then(r => r.data)
  },

  list() {
    return api.get<User[]>('/users').then(r => r.data)
  },

  getById(id: string) {
    return api.get<User>(`/users/${id}`).then(r => r.data)
  },
}
