import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../views/HomePage.vue'
import CandidatesPage from '../views/CandidatesPage.vue'
import CandidateDetail from '../views/CandidateDetail.vue'
import MyVotePage from '../views/MyVotePage.vue'
import AboutPage from '../views/AboutPage.vue'
import LoginPage from '../views/LoginPage.vue'
import RegisterPage from '../views/RegisterPage.vue'
import AdminPanel from '../components/AdminPanel.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomePage
    },
    {
      path: '/candidates',
      name: 'candidates',
      component: CandidatesPage
    },
    {
      path: '/candidates/:id',
      name: 'candidate-detail',
      component: CandidateDetail,
      props: true
    },
    {
      path: '/candidates/my-vote',
      name: 'my-vote',
      component: MyVotePage
    },
    {
      path: '/about',
      name: 'about',
      component: AboutPage
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterPage
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminPanel
    }
  ]
})

export default router
