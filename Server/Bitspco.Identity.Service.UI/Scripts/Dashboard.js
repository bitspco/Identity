var ctx = document.getElementById('myChart').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'line',
    data: {
        datasets: [{
            label: 'First dataset',

            data: [0, 20, 40, 50]
        }],
        labels: ['January', 'February', 'March', 'April']
    }
});