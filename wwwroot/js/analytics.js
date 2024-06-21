document.addEventListener('DOMContentLoaded', function () {
    const frequenciaCtx = document.getElementById('frequenciaChart').getContext('2d');
    const quantidadeAlunosMesCtx = document.getElementById('quantidadeAlunosMesChart').getContext('2d');
    const quantidadeAlunosHorarioCtx = document.getElementById('quantidadeAlunosHorarioChart').getContext('2d');
    const quantidadeAlunosSexoCtx = document.getElementById('quantidadeAlunosSexoChart').getContext('2d');

    const frequenciaChart = new Chart(frequenciaCtx, {
        type: 'bar',
        data: {
            labels: ['7x/semana', '5x/semana', '3x/semana'],
            datasets: [{
                label: 'Alunos',
                data: [120, 80, 45],
                backgroundColor: ['#ff6384', '#36a2eb', '#ffce56'],
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    const quantidadeAlunosMesChart = new Chart(quantidadeAlunosMesCtx, {
        type: 'line',
        data: {
            labels: ['Jan/23', 'Fev/23', 'Mar/23', 'Abr/23', 'Mai/23', 'Jun/23', 'Jul/23', 'Ago/23', 'Set/23', 'Out/23', 'Nov/23', 'Dez/23', 'Jan/24'],
            datasets: [{
                label: 'Quantidade de Alunos',
                data: [45, 50, 31, 35, 40, 37, 61, 57, 50, 52, 51, 49, 43],
                borderColor: '#ff6384',
                backgroundColor: '#ff6384',
                fill: false,
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    const quantidadeAlunosHorarioChart = new Chart(quantidadeAlunosHorarioCtx, {
        type: 'pie',
        data: {
            labels: ['Noite', 'Dia'],
            datasets: [{
                data: [20, 25],
                backgroundColor: ['#36a2eb', '#ffce56'],
            }]
        },
        options: {
            responsive: true
        }
    });

    const quantidadeAlunosSexoChart = new Chart(quantidadeAlunosSexoCtx, {
        type: 'pie',
        data: {
            labels: ['Feminino', 'Masculino'],
            datasets: [{
                data: [25, 15],
                backgroundColor: ['#ff6384', '#36a2eb'],
            }]
        },
        options: {
            responsive: true
        }
    });
});