import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'login',
    component: () => import('@/pages/auth/LoginPage.vue'),
    meta: { layout: 'auth', guest: true },
  },
  {
    path: '/auth/autodesk/callback',
    name: 'autodesk-callback',
    component: () => import('@/pages/auth/AutodeskCallbackPage.vue'),
    meta: { layout: 'auth', guest: true },
  },
  {
    path: '/',
    name: 'home',
    component: () => import('@/pages/HomePage.vue'),
    meta: { layout: 'default', requiresAuth: true },
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/',
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach((to) => {
  const auth = useAuthStore()

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }

  if (to.meta.guest && auth.isAuthenticated && to.name === 'login') {
    return { name: 'home' }
  }

  return true
})

export default router
