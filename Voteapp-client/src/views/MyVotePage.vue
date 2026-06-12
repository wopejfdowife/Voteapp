<template>
  <div>
    <div class="page-header">
      <div class="container">
        <h1><i class="bi bi-person-check-fill me-2"></i>Мой голос</h1>
        <p>Личный кабинет избирателя</p>
      </div>
    </div>

    <div class="container pb-5">
      <div class="row justify-content-center">
        <div class="col-lg-7">
          <div class="card mb-4">
            <div class="card-header bg-gradient-primary text-white">
              <h5 class="mb-0"><i class="bi bi-person-circle me-2"></i>Данные избирателя</h5>
            </div>
            <div class="card-body">
              <div class="row g-3">
                <div class="col-sm-6">
                  <div class="summary-box text-start">
                    <small class="text-muted d-block">Имя избирателя</small>
                    <strong>{{ voterName || 'Не указано' }}</strong>
                  </div>
                </div>
                <div class="col-sm-6">
                  <div class="summary-box text-start">
                    <small class="text-muted d-block">Email</small>
                    <strong>{{ voterName ? `${voterName}@mail.ru` : 'Не указан' }}</strong>
                  </div>
                </div>
                <div class="col-sm-6">
                  <div class="summary-box text-start">
                    <small class="text-muted d-block">Район</small>
                    <strong>г. Владикавказ</strong>
                  </div>
                </div>
                <div class="col-sm-6">
                  <div class="summary-box text-start">
                    <small class="text-muted d-block">Статус</small>
                    <strong>{{ hasVoted ? 'Проголосовал' : 'Не голосовал' }}</strong>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div v-if="!hasVoted" class="card text-center">
            <div class="card-body py-5">
              <i class="bi bi-hourglass-split text-warning mb-3" style="font-size:3rem;"></i>
              <h5 class="fw-bold mb-2">Вы ещё не проголосовали</h5>
              <p class="text-muted mb-4">Ознакомьтесь со списком кандидатов и сделайте свой выбор.</p>
              <router-link to="/candidates" class="btn btn-primary px-4">
                <i class="bi bi-hand-index-thumb me-2"></i>Перейти к кандидатам
              </router-link>
            </div>
          </div>
          <div v-else class="card voted-badge">
            <div class="card-header bg-gradient-success">
              <h5 class="mb-0"><i class="bi bi-patch-check-fill me-2"></i>Голос подан!</h5>
            </div>
            <div class="card-body text-center">
              <div class="py-2">
                <div class="trophy" style="font-size:3.5rem;">✅</div>
              </div>
              <small class="text-muted d-block mb-3">{{ votedAt }}</small>
              <h6 class="text-muted mb-1">Вы проголосовали за:</h6>
              <h3 class="winner-name">{{ votedCandidateName }}</h3>
              <p class="text-muted mb-2">{{ votedCandidateParty }}</p>
              <div class="mt-4">
                <router-link v-if="votedCandidateId" :to="`/candidates/${votedCandidateId}`" class="btn btn-outline-primary">
                  <i class="bi bi-eye-fill me-2"></i>Страница кандидата
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

const API_URL = '/api/vote'

export default {
  name: 'MyVotePage',
  data() {
    return {
      voterName: '',
      hasVoted: false,
      votedCandidateName: '',
      votedCandidateParty: '',
      votedCandidateId: null,
      votedAt: ''
    }
  },
  mounted() {
    this.loadMyVote()
  },
  methods: {
    async loadMyVote() {
      this.voterName = localStorage.getItem('voterName') || ''
      const votedFor = localStorage.getItem('votedFor')
      if (votedFor && this.voterName) {
        this.hasVoted = true
        try {
          const res = await axios.get(`${API_URL}/options`)
          const candidate = res.data.find(o => o.id === parseInt(votedFor))
          if (candidate) {
            this.votedCandidateName = candidate.name
            this.votedCandidateParty = candidate.description || 'Независимый кандидат'
            this.votedCandidateId = candidate.id
          }
          this.votedAt = new Date().toLocaleString('ru-RU', {
            day: '2-digit', month: '2-digit', year: 'numeric',
            hour: '2-digit', minute: '2-digit'
          })
        } catch (error) {
          console.error('Error loading vote:', error)
        }
      }
    }
  }
}
</script>
