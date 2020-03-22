<script>
    import { HorizontalBar } from "vue-chartjs";

    export default {
        name: "SystemChart",
        extends: HorizontalBar,
        props: {
            dates: Array,
            logs: Array
        },
        computed: {
            chartData() {
                return {
                    labels: this.dates,
                    datasets: [ {
                        label: "Logs",
                        borderWidth: 1,
                        backgroundColor: "#0B3954",
                        borderColor: "#FFF",
                        hoverBorderColor: "#FFF",
                        hoverBorderWidth: 5,
                        weight: 5,
                        data: this.logs
                    } ]
                };
            },
            options() {
                return {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        labels: {
                            fontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                            fontSize: 16
                        },
                        display: false
                    },
                    title: {
                        display: true,
                        text: "Logs",
                        fontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                        fontSize: 18
                    },
                    tooltips: {
                        bodyFontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                        bodyFontSize: 16,
                        displayColors: false,
                        callbacks: {
                            label: function (tooltipItem, data) {
                                let value = parseInt(data.datasets[0].data[tooltipItem.index]);
                                return value + ` log record${value > 1 ? "s" : ""}`;
                            }
                        }
                    }
                };
            }
        },
        mounted() {
            this.renderChart(this.chartData, this.options);
        }
    };
</script>