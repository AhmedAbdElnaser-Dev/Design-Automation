<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()
const error = ref<string | null>(null)

onMounted(async () => {
  const token = route.query.token as string | undefined
  const expiresAt = route.query.expiresAt as string | undefined
  const errParam = route.query.error as string | undefined
  const redirect = (route.query.redirect as string) || '/'

  if (errParam) {
    error.value = errParam
    return
  }

  if (!token || !expiresAt) {
    error.value = 'Missing authentication response from Autodesk.'
    return
  }

  try {
    auth.setToken(token, expiresAt)
    await auth.refreshUser()
    router.replace(redirect)
  } catch (e) {
    error.value = (e as Error)?.message ?? 'Could not load your profile.'
    auth.logout()
  }
})
</script>

<template>
  <div class="callback">
    <template v-if="!error">
      <div class="callback__spinner">
        <v-progress-circular indeterminate color="primary" size="56" width="3" />
      </div>
      <h2 class="callback__title">Completing sign-in</h2>
      <p class="callback__sub">Securing your Autodesk session…</p>
    </template>

    <template v-else>
      <div class="callback__error-icon">
        <v-icon icon="mdi-alert-circle-outline" size="32" />
      </div>
      <h2 class="callback__title">Sign-in failed</h2>
      <p class="callback__sub">{{ error }}</p>
      <v-btn color="primary" size="large" :to="{ name: 'login' }" class="mt-4">
        Back to sign in
      </v-btn>
    </template>
  </div>
</template>

<style scoped>
.callback {
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 24px 0;
}
.callback__spinner {
  margin-bottom: 24px;
}
.callback__error-icon {
  width: 56px;
  height: 56px;
  border-radius: 16px;
  background: rgba(239, 68, 68, 0.1);
  color: rgb(var(--v-theme-error));
  display: grid;
  place-items: center;
  margin-bottom: 20px;
}
.callback__title {
  font-size: 22px;
  font-weight: 700;
  letter-spacing: -0.01em;
  color: rgb(var(--v-theme-on-surface));
  margin: 0 0 6px;
}
.callback__sub {
  font-size: 14.5px;
  color: rgb(var(--v-theme-on-surface-variant));
  margin: 0;
  max-width: 320px;
}
</style>
