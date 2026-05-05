import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { createVuetify, type ThemeDefinition } from 'vuetify'
import { aliases, mdi } from 'vuetify/iconsets/mdi'

const lightTheme: ThemeDefinition = {
  dark: false,
  colors: {
    background: '#F8FAFC',
    surface: '#FFFFFF',
    'surface-bright': '#FFFFFF',
    'surface-light': '#F1F5F9',
    'surface-variant': '#E2E8F0',
    'on-surface-variant': '#475569',
    primary: '#4F46E5',
    'primary-darken-1': '#4338CA',
    secondary: '#0EA5E9',
    accent: '#A855F7',
    error: '#EF4444',
    info: '#3B82F6',
    success: '#10B981',
    warning: '#F59E0B',
    'on-background': '#0F172A',
    'on-surface': '#0F172A',
    'on-primary': '#FFFFFF',
    'on-secondary': '#FFFFFF',
  },
  variables: {
    'border-color': '#E2E8F0',
    'border-opacity': 1,
    'high-emphasis-opacity': 1,
    'medium-emphasis-opacity': 0.7,
    'disabled-opacity': 0.4,
    'theme-overlay-multiplier': 1,
  },
}

const darkTheme: ThemeDefinition = {
  dark: true,
  colors: {
    background: '#0B1020',
    surface: '#111827',
    'surface-bright': '#1F2937',
    'surface-light': '#1E293B',
    'surface-variant': '#334155',
    'on-surface-variant': '#CBD5E1',
    primary: '#818CF8',
    'primary-darken-1': '#6366F1',
    secondary: '#38BDF8',
    accent: '#C084FC',
    error: '#F87171',
    info: '#60A5FA',
    success: '#34D399',
    warning: '#FBBF24',
    'on-background': '#E5E7EB',
    'on-surface': '#E5E7EB',
    'on-primary': '#0B1020',
  },
  variables: {
    'border-color': '#1F2937',
    'border-opacity': 1,
    'high-emphasis-opacity': 1,
    'medium-emphasis-opacity': 0.7,
    'disabled-opacity': 0.4,
  },
}

export default createVuetify({
  theme: {
    defaultTheme: 'light',
    themes: { light: lightTheme, dark: darkTheme },
  },
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: { mdi },
  },
  defaults: {
    global: {
      ripple: true,
    },
    VBtn: {
      rounded: 'lg',
      class: 'text-none font-weight-medium',
      style: 'letter-spacing: 0;',
    },
    VCard: {
      rounded: 'xl',
      flat: true,
    },
    VTextField: {
      variant: 'outlined',
      density: 'comfortable',
      color: 'primary',
      hideDetails: 'auto',
    },
    VTextarea: {
      variant: 'outlined',
      density: 'comfortable',
      color: 'primary',
      hideDetails: 'auto',
    },
    VSelect: {
      variant: 'outlined',
      density: 'comfortable',
      color: 'primary',
      hideDetails: 'auto',
    },
    VAutocomplete: {
      variant: 'outlined',
      density: 'comfortable',
      color: 'primary',
      hideDetails: 'auto',
    },
    VCombobox: {
      variant: 'outlined',
      density: 'comfortable',
      color: 'primary',
      hideDetails: 'auto',
    },
    VAlert: {
      rounded: 'lg',
      variant: 'tonal',
      density: 'comfortable',
    },
    VChip: {
      rounded: 'md',
    },
    VList: {
      density: 'comfortable',
    },
    VAppBar: {
      flat: true,
    },
    VNavigationDrawer: {
      elevation: 0,
    },
    VTooltip: {
      location: 'bottom',
    },
  },
})
