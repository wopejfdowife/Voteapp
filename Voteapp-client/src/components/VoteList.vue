<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Voting Options</h2>
    <div class="row">
      <div class="col-md-6" v-for="option in options" :key="option.id">
        <div class="card mb-3">
          <div class="card-body">
            <h5 class="card-title">{{ option.name }}</h5>
            <p class="card-text">{{ option.description }}</p>
            <div class="d-flex justify-content-between align-items-center">
              <span class="badge bg-primary">Votes: {{ option.voteCount }}</span>
              <button class="btn btn-success" @click="vote(option.id)" :disabled="hasVoted">
                Vote
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <div class="modal fade" id="voteModal" tabindex="-1" data-bs-backdrop="static">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Cast Your Vote</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <div class="mb-3">
              <label class="form-label">Your Name</label>
              <input type="text" class="form-control" v-model="voterName" placeholder="Enter your name">
            </div>
            <div class="alert alert-info">
              You are voting for: <strong>{{ selectedOption?.name }}</strong>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
            <button type="button" class="btn btn-primary" @click="confirmVote">Confirm Vote</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { Modal } from 'bootstrap'

const API_URL = '/api/vote'

export default {
  name: 'VoteList',
  data() {
    return {
      options: [],
      selectedOption: null,
      voterName: '',
      hasVoted: false
    }
  },
  mounted() {
    this.fetchOptions()
    this.checkPreviousVote()
  },
  methods: {
    async fetchOptions() {
      try {
        const response = await axios.get(`${API_URL}/options`)
        this.options = response.data
      } catch (error) {
        console.error('Error fetching options:', error)
      }
    },
    async checkPreviousVote() {
      const storedName = localStorage.getItem('voterName')
      if (storedName) {
        try {
          const response = await axios.get(`${API_URL}/check-vote/${storedName}`)
          this.hasVoted = response.data.hasVoted
          if (this.hasVoted) {
            this.voterName = storedName
          }
        } catch (error) {
          console.error('Error checking vote:', error)
        }
      }
    },
    vote(optionId) {
      this.selectedOption = this.options.find(opt => opt.id === optionId)
      const modal = new Modal(document.getElementById('voteModal'))
      modal.show()
    },
    async confirmVote() {
      if (!this.voterName.trim()) {
        alert('Please enter your name')
        return
      }
      try {
        const response = await axios.post(`${API_URL}/vote`, {
          voteOptionId: this.selectedOption.id,
          voterName: this.voterName
        })
        if (response.data.success) {
          localStorage.setItem('voterName', this.voterName)
          this.hasVoted = true
          alert('Vote recorded successfully!')
          this.fetchOptions()
          const modal = Modal.getInstance(document.getElementById('voteModal'))
          modal.hide()
          this.voterName = ''
          this.selectedOption = null
        } else {
          alert(response.data.message)
        }
      } catch (error) {
        console.error('Error voting:', error)
        alert('Error recording vote')
      }
    }
  }
}
</script>

<style scoped>
.card {
  transition: transform 0.2s;
}
.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
}
</style>