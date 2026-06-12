<template>
  <div class="container">
    <h2 class="text-center"><i class="bi bi-bar-chart-fill"></i> Voting Statistics</h2>
    <p class="text-center text-muted mb-4">Live results and winner announcement</p>

    <div class="row g-4">
      <div class="col-lg-7">
        <div class="card">
          <div class="card-header bg-gradient-primary text-white">
            <h5 class="mb-0"><i class="bi bi-pie-chart-fill"></i> Results</h5>
          </div>
          <div class="card-body">
            <div v-for="(result, index) in results" :key="result.optionId" class="mb-4 result-item">
              <div class="d-flex justify-content-between align-items-center mb-2">
                <div class="d-flex align-items-center">
                  <div class="result-dot" :style="{ background: barColors[index % barColors.length] }"></div>
                  <strong class="ms-2">{{ result.optionName }}</strong>
                </div>
                <span class="badge" :style="{ background: barColors[index % barColors.length] }">
                  {{ result.voteCount }} votes ({{ result.percentage.toFixed(1) }}%)
                </span>
              </div>
              <div class="progress progress-custom">
                <div
                  class="progress-bar"
                  :style="{
                    width: result.percentage + '%',
                    background: barColors[index % barColors.length]
                  }"
                >
                </div>
              </div>
            </div>
            <div v-if="results.length === 0" class="text-center text-muted py-4">
              <i class="bi bi-inbox" style="font-size: 2rem;"></i>
              <p class="mt-2">No votes yet</p>
            </div>
          </div>
        </div>
      </div>

      <div class="col-lg-5">
        <div class="card winner-card">
          <div class="card-header bg-gradient-success text-white text-center">
            <h5 class="mb-0"><i class="bi bi-trophy-fill"></i> Winner</h5>
          </div>
          <div class="card-body text-center" v-if="winner">
            <div class="trophy-wrapper">
              <div class="trophy">🏆</div>
            </div>
            <h3 class="winner-name">{{ winner.name }}</h3>
            <div class="winner-stats">
              <div class="stat-item">
                <span class="stat-value">{{ winner.voteCount }}</span>
                <span class="stat-label">votes</span>
              </div>
              <div class="stat-item">
                <span class="stat-value">{{ winner.percentage ? winner.percentage.toFixed(1) : '0' }}%</span>
                <span class="stat-label">of total</span>
              </div>
            </div>
          </div>
          <div class="card-body text-center" v-else>
            <div class="trophy-wrapper">
              <div class="trophy muted">🏆</div>
            </div>
            <p class="text-muted mt-3">Waiting for votes...</p>
          </div>
        </div>

        <div class="card mt-4 summary-card">
          <div class="card-header bg-gradient-info text-white">
            <h5 class="mb-0"><i class="bi bi-info-circle-fill"></i> Summary</h5>
          </div>
          <div class="card-body">
            <div class="row g-3">
              <div class="col-6">
                <div class="summary-box">
                  <i class="bi bi-list-ul summary-icon"></i>
                  <div class="summary-value">{{ optionsCount }}</div>
                  <div class="summary-label">Options</div>
                </div>
              </div>
              <div class="col-6">
                <div class="summary-box">
                  <i class="bi bi-people-fill summary-icon"></i>
                  <div class="summary-value">{{ totalVotes }}</div>
                  <div class="summary-label">Total Votes</div>
                </div>
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
      optionsCount: 0,
      barColors: [
        'linear-gradient(135deg, #667eea, #764ba2)',
        'linear-gradient(135deg, #4facfe, #00f2fe)',
        'linear-gradient(135deg, #43e97b, #38f9d7)',
        'linear-gradient(135deg, #fa709a, #fee140)'
      ]
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

<style scoped>
.bg-gradient-primary {
  background: linear-gradient(135deg, #667eea, #764ba2) !important;
}

.bg-gradient-success {
  background: linear-gradient(135deg, #43e97b, #38f9d7) !important;
  color: #1a3a2a !important;
}

.bg-gradient-info {
  background: linear-gradient(135deg, #4facfe, #00f2fe) !important;
}

.result-item {
  animation: fadeInUp 0.5s ease both;
}

.result-item:nth-child(1) { animation-delay: 0.05s; }
.result-item:nth-child(2) { animation-delay: 0.15s; }
.result-item:nth-child(3) { animation-delay: 0.25s; }
.result-item:nth-child(4) { animation-delay: 0.35s; }

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(15px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.result-dot {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  flex-shrink: 0;
}

.progress-custom {
  height: 12px;
  border-radius: 20px;
  background: #eef0f4;
  overflow: hidden;
}

.progress-bar {
  border-radius: 20px;
  transition: width 1.2s cubic-bezier(0.4, 0, 0.2, 1);
}

.winner-card {
  overflow: hidden;
}

.trophy-wrapper {
  padding: 1.5rem 0;
}

.trophy {
  font-size: 5rem;
  display: inline-block;
  animation: trophyBounce 2s ease-in-out infinite;
  filter: drop-shadow(0 4px 20px rgba(255, 215, 0, 0.3));
}

.trophy.muted {
  filter: grayscale(1) opacity(0.3);
  animation: none;
}

@keyframes trophyBounce {
  0%, 100% { transform: translateY(0) scale(1); }
  50% { transform: translateY(-10px) scale(1.05); }
}

.winner-name {
  font-weight: 800;
  font-size: 1.8rem;
  background: linear-gradient(135deg, #f6d365, #fda085);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.winner-stats {
  display: flex;
  justify-content: center;
  gap: 2rem;
  margin-top: 1rem;
}

.stat-item {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.stat-value {
  font-size: 1.5rem;
  font-weight: 800;
  color: #2d3436;
}

.stat-label {
  font-size: 0.8rem;
  color: #636e72;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.summary-box {
  background: linear-gradient(135deg, #f5f7fa, #e4e9f2);
  border-radius: var(--radius-md);
  padding: 1.25rem;
  text-align: center;
  transition: all 0.3s ease;
}

.summary-box:hover {
  transform: translateY(-3px);
  box-shadow: var(--shadow-sm);
}

.summary-icon {
  font-size: 1.5rem;
  color: #667eea;
}

.summary-value {
  font-size: 2rem;
  font-weight: 800;
  color: #2d3436;
  margin: 0.25rem 0;
}

.summary-label {
  font-size: 0.8rem;
  color: #636e72;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}
</style>
