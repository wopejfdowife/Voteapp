import { createRouter, createWebHistory } from 'vue-router'
import VoteList from '../components/VoteList.vue'
import Statistics from '../components/Statistics.vue'
import AdminPanel from '../components/AdminPanel.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'vote',
      component: VoteList
    },
    {
      path: '/statistics',
      name: 'statistics',
      component: Statistics
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminPanel
    }
  ]
})

export default router
