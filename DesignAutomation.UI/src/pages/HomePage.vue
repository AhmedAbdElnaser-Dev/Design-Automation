<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const greeting = computed(() => auth.user?.fullName ?? auth.user?.userName ?? 'there')
</script>

<template>
  <div>
    <h1 class="text-h4 font-weight-bold mb-1">Hello, {{ greeting }}</h1>
    <p class="text-subtitle-1 text-medium-emphasis mb-6">Welcome to your Design Automation workspace.</p>

    <v-row>
      <v-col cols="12" md="4">
        <v-card variant="tonal" color="primary" class="pa-4">
          <div class="d-flex align-center mb-2">
            <v-icon icon="mdi-cube-outline" size="32" class="me-3" />
            <span class="text-h6">APS Buckets</span>
          </div>
          <p class="text-body-2">Manage your Object Storage buckets and files.</p>
        </v-card>
      </v-col>
      <v-col cols="12" md="4">
        <v-card variant="tonal" color="success" class="pa-4">
          <div class="d-flex align-center mb-2">
            <v-icon icon="mdi-cog-outline" size="32" class="me-3" />
            <span class="text-h6">Workflows</span>
          </div>
          <p class="text-body-2">Build and run Design Automation workflows.</p>
        </v-card>
      </v-col>
      <v-col cols="12" md="4">
        <v-card variant="tonal" color="info" class="pa-4">
          <div class="d-flex align-center mb-2">
            <v-icon icon="mdi-account-group-outline" size="32" class="me-3" />
            <span class="text-h6">Users</span>
          </div>
          <p class="text-body-2">Invite and manage team members.</p>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>
