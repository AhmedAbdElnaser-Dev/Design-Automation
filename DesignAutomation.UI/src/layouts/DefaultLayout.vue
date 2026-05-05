<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()
const drawer = ref(true)

const initials = computed(() => {
  const name = auth.user?.fullName ?? auth.user?.userName ?? auth.user?.email ?? '?'
  return name.split(/\s+/).map(p => p.charAt(0)).slice(0, 2).join('').toUpperCase()
})

function logout() {
  auth.logout()
  router.replace({ name: 'login' })
}
</script>

<template>
  <v-app>
    <v-app-bar color="primary" density="comfortable" flat>
      <v-app-bar-nav-icon @click="drawer = !drawer" />
      <v-app-bar-title>Design Automation</v-app-bar-title>
      <v-spacer />
      <v-menu>
        <template #activator="{ props }">
          <v-btn icon v-bind="props">
            <v-avatar color="white" size="36">
              <span class="text-primary font-weight-bold">{{ initials }}</span>
            </v-avatar>
          </v-btn>
        </template>
        <v-list>
          <v-list-item>
            <v-list-item-title>{{ auth.user?.fullName ?? auth.user?.userName }}</v-list-item-title>
            <v-list-item-subtitle>{{ auth.user?.email }}</v-list-item-subtitle>
          </v-list-item>
          <v-divider />
          <v-list-item @click="logout" prepend-icon="mdi-logout">
            <v-list-item-title>Sign out</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" permanent>
      <v-list nav density="comfortable">
        <v-list-item :to="{ name: 'home' }" prepend-icon="mdi-view-dashboard" title="Home" />
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <v-container fluid class="pa-6">
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>
