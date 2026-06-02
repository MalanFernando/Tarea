var colores = ['#4e73df','#1cc88a','#36b9cc','#f6c23e','#e74a3b','#858796','#5a5c69','#2c9faf'];

function crearChart(id, config) {
    var canvas = document.getElementById(id);
    if (canvas) { new Chart(canvas, config); }
}

function inicializarCharts(categorias, roles, proveedores) {
    if (categorias.labels.length) {
        crearChart('chartCategorias', {
            type: 'bar',
            data: {
                labels: categorias.labels,
                datasets: [{ label: 'Productos', data: categorias.data, backgroundColor: colores.slice(0, categorias.labels.length), borderRadius: 4 }]
            },
            options: { responsive: true, plugins: { legend: { display: false } }, scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } } }
        });
    }

    if (roles.labels.length) {
        crearChart('chartRoles', {
            type: 'doughnut',
            data: {
                labels: roles.labels,
                datasets: [{ data: roles.data, backgroundColor: ['#4e73df','#1cc88a'], borderWidth: 0 }]
            },
            options: { responsive: true, plugins: { legend: { position: 'bottom' } } }
        });
    }

    if (proveedores.labels.length) {
        crearChart('chartProveedores', {
            type: 'bar',
            data: {
                labels: proveedores.labels,
                datasets: [{ label: 'Productos', data: proveedores.data, backgroundColor: colores.slice(0, proveedores.labels.length), borderRadius: 4 }]
            },
            options: { indexAxis: 'y', responsive: true, plugins: { legend: { display: false } }, scales: { x: { beginAtZero: true, ticks: { stepSize: 1 } } } }
        });
    }
}
