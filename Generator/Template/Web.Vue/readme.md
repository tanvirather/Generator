# Project structure
web/
├── public/                 # Static assets (favicon, index.html)
├── src/
│   ├── assets/             # Images, fonts, and other static assets
│   ├── components/         # Reusable UI components
│   ├── views/              # Route-level components (pages)
│   ├── layouts/            # App layout components (optional)
│   ├── router/             # Vue Router setup
│   ├── store/              # Pinia or Vuex store modules
│   ├── composables/        # Reusable logic (Vue 3 Composition API)
│   ├── services/           # API calls or business logic
│   ├── directives/         # Custom directives
│   ├── plugins/            # Vue plugins (e.g., i18n, axios)
│   ├── utils/              # Utility functions/helpers
│   ├── App.vue             # Root component
│   └── main.js             # App entry point
├── .env                   # Environment variables
├── .editorconfig
├── .eslintrc.js
├── vite.config.js         # Or vue.config.js if using Vue CLI
└── package.json


## Create new project
```sh
npm create vue@latest -- --default --bare web.vue
cd web.vue
npm install
```

### Compile and Hot-Reload for Development
```sh
npm run dev
```

### Compile and Minify for Production
```sh
npm run build
```
