<template>
  <div>
    <div class="page-header">
      <div class="container text-center">
        <h1><i class="bi bi-info-circle-fill me-2"></i>О системе голосования</h1>
        <p>ГолосРСО — цифровая демократия для каждого жителя республики</p>
      </div>
    </div>

    <div class="container pb-5">
      <div class="row mb-5">
        <div class="col-lg-8 mx-auto">
          <div class="card p-4 p-md-5">
            <h2 class="page-title mb-4"><i class="bi bi-shield-check"></i> О проекте</h2>
            <p class="lead">
              <strong>ГолосРСО</strong> — это электронная система голосования, разработанная специально для жителей
              Республики Северная Осетия – Алания и города Владикавказ.
            </p>
            <p>
              Система позволяет гражданам безопасно и прозрачно участвовать в демократических процессах.
              Каждый зарегистрированный избиратель может ознакомиться с программами кандидатов и отдать
              свой голос в режиме онлайн.
            </p>

            <hr class="my-4" />

            <h5 class="fw-bold mb-3">Ключевые возможности:</h5>
            <div class="row g-3">
              <div v-for="feature in features" :key="feature[0]" class="col-md-6">
                <div class="d-flex align-items-start gap-3">
                  <div class="feature-icon">
                    <i :class="'bi ' + feature[0]"></i>
                  </div>
                  <span>{{ feature[1] }}</span>
                </div>
              </div>
            </div>

            <hr class="my-4" />

            <h5 class="fw-bold mb-3">Технологии:</h5>
            <div class="d-flex flex-wrap gap-2">
              <span v-for="tech in technologies" :key="tech" class="tech-badge">{{ tech }}</span>
            </div>
          </div>
        </div>
      </div>

      <div class="row mb-5">
        <div class="col-12 text-center mb-4">
          <h2 class="page-title"><i class="bi bi-people-fill"></i> Команда разработчиков</h2>
          <p class="page-subtitle">Студенты СОГУ, г. Владикавказ</p>
        </div>

        <div v-for="dev in developers" :key="dev.name" class="col-md-4 mb-4">
          <div class="card dev-card h-100 p-4 text-center">
            <div class="dev-icon" :style="{ background: dev.gradient }">
              <i :class="'bi ' + dev.icon"></i>
            </div>
            <h5 class="fw-bold mb-1">{{ dev.name }}</h5>
            <p class="text-muted mb-3" style="font-size:.9rem;">{{ dev.role }}</p>
            <p style="font-size:.88rem;color:#555;">{{ dev.description }}</p>
          </div>
        </div>
      </div>

      <div class="row mb-5">
        <div class="col-lg-8 mx-auto">
          <div class="card p-4 voted-badge">
            <h5 class="fw-bold mb-3"><i class="bi bi-envelope-fill me-2"></i>Обратная связь</h5>
            <p class="mb-1"><i class="bi bi-geo-alt-fill me-2 text-muted"></i><strong>Адрес:</strong> г. Владикавказ, ул. Ватутина 44 (СОГУ)</p>
            <p class="mb-1"><i class="bi bi-envelope me-2 text-muted"></i><strong>Email:</strong> golosrso@sogu.ru</p>
            <p class="mb-0"><i class="bi bi-calendar-fill me-2 text-muted"></i><strong>Год разработки:</strong> 2026</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'AboutPage',
  data() {
    return {
      features: [
        ['bi-person-check-fill', 'Регистрация избирателей с верификацией возраста'],
        ['bi-lock-fill', 'Защита от повторного голосования'],
        ['bi-eye-fill', 'Прозрачные результаты в реальном времени'],
        ['bi-geo-alt-fill', 'Разграничение по районам Владикавказа'],
        ['bi-phone-fill', 'Адаптивный интерфейс для любых устройств'],
        ['bi-database-fill', 'Надёжное хранение данных в SQLite']
      ],
      technologies: [
        'ASP.NET Core Web API', 'C#', 'Entity Framework Core', 'SQLite',
        'Bootstrap 5', 'Vue.js 3', 'Vite'
      ],
      developers: [
        {
          name: 'Неграш Никита',
          role: 'Backend-разработчик',
          icon: 'bi-code-slash',
          gradient: 'var(--gradient-1)',
          description: 'Разработка серверной логики, работа с базой данных, архитектура приложения и система аутентификации.'
        },
        {
          name: 'Дзагоев Марат',
          role: 'Frontend-разработчик',
          icon: 'bi-palette-fill',
          gradient: 'var(--gradient-2)',
          description: 'Проектирование пользовательского интерфейса, вёрстка страниц, адаптивный дизайн и UX-оптимизация.'
        },
        {
          name: 'Тотоев Александр',
          role: 'Fullstack-разработчик / Тестирование',
          icon: 'bi-bug-fill',
          gradient: 'var(--gradient-4)',
          description: 'Интеграция компонентов, тестирование, оптимизация производительности и документирование системы.'
        }
      ]
    }
  }
}
</script>
