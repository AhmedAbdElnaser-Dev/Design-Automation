<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()

const greeting = computed(() => auth.user?.fullName?.split(' ')[0] ?? auth.user?.userName ?? 'there')
const today = computed(() =>
  new Date().toLocaleDateString(undefined, { weekday: 'long', month: 'long', day: 'numeric' }),
)

const stats = [
  { label: 'Buckets', value: '12', delta: '+2', icon: 'mdi-cube-outline', tone: 'primary' },
  { label: 'Objects', value: '348', delta: '+24', icon: 'mdi-file-document-outline', tone: 'info' },
  { label: 'Storage', value: '4.2 GB', delta: '+512 MB', icon: 'mdi-database-outline', tone: 'success' },
  { label: 'Workflows', value: '7', delta: '0', icon: 'mdi-cog-sync-outline', tone: 'accent' },
]

const quickActions = [
  { title: 'Create bucket', desc: 'Provision a new OSS bucket for project assets.', icon: 'mdi-cube-plus', tone: 'primary' },
  { title: 'Upload model', desc: 'Drag-and-drop a Revit, IFC, or DWG file.', icon: 'mdi-cloud-upload-outline', tone: 'info' },
  { title: 'New workflow', desc: 'Wire APIs, viewer, and Design Automation jobs.', icon: 'mdi-graph-outline', tone: 'success' },
  { title: 'Invite teammates', desc: 'Bring engineers and designers into the workspace.', icon: 'mdi-account-multiple-plus-outline', tone: 'accent' },
]
</script>

<template>
  <div class="home">
    <header class="home__head">
      <div>
        <p class="home__date">{{ today }}</p>
        <h1 class="home__title">Good to see you, {{ greeting }}.</h1>
        <p class="home__sub">Here's what's happening across your workspace today.</p>
      </div>
      <div class="home__actions">
        <v-btn color="primary" size="large" prepend-icon="mdi-plus">New project</v-btn>
      </div>
    </header>

    <section class="stats">
      <article v-for="s in stats" :key="s.label" class="stat" :class="`stat--${s.tone}`">
        <div class="stat__icon">
          <v-icon :icon="s.icon" size="22" />
        </div>
        <div class="stat__body">
          <span class="stat__label">{{ s.label }}</span>
          <div class="stat__value-row">
            <span class="stat__value">{{ s.value }}</span>
            <span class="stat__delta" :class="{ 'is-zero': s.delta === '0' }">{{ s.delta }}</span>
          </div>
        </div>
      </article>
    </section>

    <section class="section">
      <header class="section__head">
        <h2 class="section__title">Quick actions</h2>
        <a href="#" class="section__link">View all</a>
      </header>

      <div class="actions-grid">
        <button v-for="a in quickActions" :key="a.title" class="action" :class="`action--${a.tone}`">
          <span class="action__icon">
            <v-icon :icon="a.icon" size="22" />
          </span>
          <span class="action__body">
            <span class="action__title">{{ a.title }}</span>
            <span class="action__desc">{{ a.desc }}</span>
          </span>
          <v-icon icon="mdi-arrow-top-right" size="18" class="action__chev" />
        </button>
      </div>
    </section>

    <section class="section">
      <header class="section__head">
        <h2 class="section__title">Recent activity</h2>
        <a href="#" class="section__link">View all</a>
      </header>

      <v-card class="activity-card">
        <div class="activity-empty">
          <div class="activity-empty__icon">
            <v-icon icon="mdi-timeline-outline" size="28" />
          </div>
          <h3 class="activity-empty__title">No activity yet</h3>
          <p class="activity-empty__sub">Once you upload a model or run a workflow, it'll appear here.</p>
          <v-btn variant="tonal" color="primary" size="small" prepend-icon="mdi-cloud-upload-outline">
            Upload your first file
          </v-btn>
        </div>
      </v-card>
    </section>
  </div>
</template>

<style scoped>
.home { display: flex; flex-direction: column; gap: 32px; }

.home__head {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  gap: 16px;
  flex-wrap: wrap;
}
.home__date {
  font-size: 12.5px;
  font-weight: 500;
  color: rgb(var(--v-theme-on-surface-variant));
  text-transform: uppercase;
  letter-spacing: 0.06em;
  margin: 0 0 6px;
}
.home__title {
  font-size: clamp(24px, 2.4vw, 30px);
  line-height: 1.15;
  font-weight: 700;
  letter-spacing: -0.02em;
  color: rgb(var(--v-theme-on-surface));
  margin: 0 0 6px;
}
.home__sub {
  font-size: 14.5px;
  color: rgb(var(--v-theme-on-surface-variant));
  margin: 0;
}

.stats {
  display: grid;
  grid-template-columns: repeat(4, minmax(0, 1fr));
  gap: 16px;
}
@media (max-width: 1100px) { .stats { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
@media (max-width: 520px) { .stats { grid-template-columns: 1fr; } }

.stat {
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 18px;
  background: rgb(var(--v-theme-surface));
  border: 1px solid rgb(var(--v-theme-surface-variant));
  border-radius: 16px;
  transition: transform 0.15s ease, box-shadow 0.15s ease, border-color 0.15s ease;
}
.stat:hover {
  transform: translateY(-1px);
  box-shadow: 0 8px 24px -16px rgba(15, 23, 42, 0.18);
  border-color: rgba(99, 102, 241, 0.4);
}
.stat__icon {
  width: 44px; height: 44px;
  border-radius: 12px;
  display: grid; place-items: center;
}
.stat--primary .stat__icon { background: rgba(79,70,229,0.12); color: #4F46E5; }
.stat--info    .stat__icon { background: rgba(59,130,246,0.12); color: #3B82F6; }
.stat--success .stat__icon { background: rgba(16,185,129,0.12); color: #10B981; }
.stat--accent  .stat__icon { background: rgba(168,85,247,0.12); color: #A855F7; }

.stat__body { display: flex; flex-direction: column; gap: 4px; min-width: 0; }
.stat__label {
  font-size: 12.5px;
  font-weight: 500;
  color: rgb(var(--v-theme-on-surface-variant));
}
.stat__value-row { display: flex; align-items: baseline; gap: 8px; }
.stat__value {
  font-size: 22px;
  font-weight: 700;
  letter-spacing: -0.02em;
  color: rgb(var(--v-theme-on-surface));
}
.stat__delta {
  font-size: 12px;
  font-weight: 600;
  color: rgb(var(--v-theme-success));
  background: rgba(16,185,129,0.12);
  padding: 2px 6px;
  border-radius: 999px;
}
.stat__delta.is-zero {
  color: rgb(var(--v-theme-on-surface-variant));
  background: rgb(var(--v-theme-surface-light));
}

.section { display: flex; flex-direction: column; gap: 16px; }
.section__head {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.section__title {
  font-size: 16px;
  font-weight: 600;
  letter-spacing: -0.01em;
  color: rgb(var(--v-theme-on-surface));
  margin: 0;
}
.section__link {
  font-size: 13px;
  font-weight: 500;
  color: rgb(var(--v-theme-primary));
  text-decoration: none;
}
.section__link:hover { text-decoration: underline; }

.actions-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 12px;
}
@media (max-width: 720px) { .actions-grid { grid-template-columns: 1fr; } }

.action {
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 16px;
  background: rgb(var(--v-theme-surface));
  border: 1px solid rgb(var(--v-theme-surface-variant));
  border-radius: 14px;
  text-align: left;
  cursor: pointer;
  font-family: inherit;
  transition: transform 0.15s ease, box-shadow 0.15s ease, border-color 0.15s ease;
}
.action:hover {
  transform: translateY(-1px);
  box-shadow: 0 8px 24px -16px rgba(15, 23, 42, 0.18);
  border-color: rgba(99,102,241,0.4);
}
.action:hover .action__chev { transform: translate(2px, -2px); }

.action__icon {
  width: 40px; height: 40px;
  border-radius: 10px;
  display: grid; place-items: center;
  flex-shrink: 0;
}
.action--primary .action__icon { background: rgba(79,70,229,0.12); color: #4F46E5; }
.action--info    .action__icon { background: rgba(59,130,246,0.12); color: #3B82F6; }
.action--success .action__icon { background: rgba(16,185,129,0.12); color: #10B981; }
.action--accent  .action__icon { background: rgba(168,85,247,0.12); color: #A855F7; }

.action__body { display: flex; flex-direction: column; gap: 2px; min-width: 0; flex: 1; }
.action__title {
  font-size: 14px;
  font-weight: 600;
  color: rgb(var(--v-theme-on-surface));
}
.action__desc {
  font-size: 13px;
  color: rgb(var(--v-theme-on-surface-variant));
  line-height: 1.4;
}
.action__chev {
  color: rgb(var(--v-theme-on-surface-variant));
  transition: transform 0.15s ease;
}

.activity-card {
  border: 1px solid rgb(var(--v-theme-surface-variant)) !important;
  background: rgb(var(--v-theme-surface)) !important;
}
.activity-empty {
  padding: 56px 24px;
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.activity-empty__icon {
  width: 56px; height: 56px;
  border-radius: 14px;
  background: rgb(var(--v-theme-surface-light));
  color: rgb(var(--v-theme-on-surface-variant));
  display: grid; place-items: center;
  margin-bottom: 16px;
}
.activity-empty__title {
  font-size: 16px;
  font-weight: 600;
  margin: 0 0 4px;
  color: rgb(var(--v-theme-on-surface));
}
.activity-empty__sub {
  font-size: 13.5px;
  color: rgb(var(--v-theme-on-surface-variant));
  margin: 0 0 16px;
  max-width: 360px;
}
</style>
