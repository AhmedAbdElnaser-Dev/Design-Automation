<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()
const error = ref<string | null>(null)

onMounted(() => {
  const token = route.query.token as string | undefined
  const expiresAt = route.query.expiresAt as string | undefined
  const userJson = route.query.user as string | undefined
  const errParam = route.query.error as string | undefined

  if (errParam) {
    error.value = errParam
    return
  }

  if (!token || !expiresAt || !userJson) {
    error.value = 'Missing authentication response from Autodesk.'
    return
  }

  try {
    const user = JSON.parse(decodeURIComponent(userJson))
    auth.$patch({ token, expiresAt, user })
    localStorage.setItem('da.auth.token', token)
    localStorage.setItem('da.auth.expiresAt', expiresAt)
    localStorage.setItem('da.auth.user', JSON.stringify(user))
    const redirect = (route.query.redirect as string) || '/'
    router.replace(redirect)
  } catch {
    error.value = 'Invalid authentication response.'
  }
})
</script>

<template>
  <v-card max-width="420" width="100%" elevation="2" rounded="lg" class="pa-6">
    <template v-if="!error">
      <div class="d-flex flex-column align-center py-6">
        <v-progress-circular indeterminate color="primary" size="48" class="mb-4" />
        <p class="text-body-1">Completing Autodesk sign-in...</p>
      </div>
    </template>
    <template v-else>
      <v-alert type="error" variant="tonal" class="mb-4">{{ error }}</v-alert>
      <v-btn color="primary" block :to="{ name: 'login' }">Back to sign in</v-btn>
    </template>
  </v-card>
</template>
