import { createRouter, createWebHistory } from 'vue-router'
import Identity_User from '../views/Identity/User.vue'
import Login from '../views/Login.vue'
import Product_PostgresType from '../views/product/PostgresType.vue'
// import Product_Signalr from '../views/product/Signalr.vue'
// import Product_User from '../views/product/User.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'index', component: Login },
    { path: '/Login', name: 'Login', component: Login },
    { path: '/Identity/User', name: 'Identity_User', component: Identity_User },
    { path: '/Product/PostgresType', name: 'PostgresType', component: Product_PostgresType },
    // { path: '/Product/Signalr', name: 'Product_Signalr', component: Product_Signalr },
    // { path: '/Product/User', name: 'Product_User', component: Product_User },
  ],
})

export default router
