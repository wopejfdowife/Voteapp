<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Voting Statistics</h2>
    <div class="row">
      <div class="col-md-6">
        <div class="card">
          <div class="card-header bg-primary text-white">
            <h5 class="mb-0">Results</h5>
          </div>
          <div class="card-body">
            <div v-for="result in results" :key="result.optionId" class="mb-3">
              <div class="d-flex justify-content-between">
                <strong>{{ result.optionName }}</strong>
                <span>{{ result.voteCount }} votes ({{ result.percentage.toFixed(1) }}%)</span>
              </div>
              <div class="progress">
                <div class="progress-bar" role="progressbar" :style="{ width: result.percentage + '%' }">
                  {{ result.percentage.toFixed(1) }}%
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-header bg-success text-white">
            <h5 class="mb-0">Winner</h5>
          </div>
          <div class="card-body text-center" v-if="winner">
            <div class="display-4">🏆</div>
            <h3>{{ winner.name }}</h3>
            <p class="lead">{{ winner.voteCount }} votes</p>
            <p>Total votes: {{ totalVotes }}</p>
          </div>
          <div class="card-body text-center" v-else>
            <p>No votes yet</p>
          </div>
        </div>
        <div class="card mt-3">
          <div class="card-header bg-info text-white">
            <h5 class="mb-0">Summary</h5>
          </div>
          <div class="card-body">
            <div class="row text-center">
              <div class="col-6">
                <h3>{{ optionsCount }}</h3>
                <p>Options</p>
              </div>
              <div class="col-6">
                <h3>{{ totalVotes }}</h3>
                <p>Total Votes</p>
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
  name: 'Statistics',
  data() {
    return {
      results: [],
      winner: null,
      totalVotes: 0,
      optionsCount: 0
    }
  },
  mounted() {
    this.fetchStatistics()
    this.fetchWinner()
    this.fetchTotalVotes()
    this.fetchOptionsCount()
  },
  methods: {
    async fetchStatistics() {
      try {
        const response = await axios.get(`${API_URL}/results`)
        this.results = response.data
      } catch (error) {
        console.error('Error fetching statistics:', error)
      }
    },
    async fetchWinner() {
      try {
        const response = await axios.get(`${API_URL}/winner`)
        this.winner = response.data.winner
      } catch (error) {
        console.error('Error fetching winner:', error)
      }
    },
    async fetchTotalVotes() {
      try {
        const response = await axios.get(`${API_URL}/total-votes`)
        this.totalVotes = response.data.totalVotes
      } catch (error) {
        console.error('Error fetching total votes:', error)
      }
    },
    async fetchOptionsCount() {
      try {
        const response = await axios.get(`${API_URL}/options`)
        this.optionsCount = response.data.length
      } catch (error) {
        console.error('Error fetching options count:', error)
      }
    }
  }
}
</script>