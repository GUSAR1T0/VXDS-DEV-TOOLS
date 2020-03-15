<script>
    import { HorizontalBar } from "vue-chartjs";

    export default {
        name: "SystemChart",
        extends: HorizontalBar,
        props: {
            dates: Array,
            operations: Array
        },
        computed: {
            chartData() {
                return {
                    labels: this.dates,
                    datasets: [ {
                        label: "Operations",
                        borderWidth: 1,
                        backgroundColor: "#0B3954",
                        borderColor: "#FFF",
                        hoverBorderColor: "#FFF",
                        hoverBorderWidth: 5,
                        weight: 5,
                        data: this.operations
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
                        text: "Operations",
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
                                return value + ` operation${value > 1 ? "s" : ""}`;
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