<script>
    import { Pie } from "vue-chartjs";

    export default {
        name: "UsersChart",
        extends: Pie,
        props: {
            activatedCount: Number,
            deactivatedCount: Number
        },
        computed: {
            chartData() {
                return {
                    labels: [ "Activated", "Deactivated" ],
                    datasets: [ {
                        borderWidth: 1,
                        backgroundColor: [ "#0B3954", "#C0C4CC" ],
                        borderColor: "#FFF",
                        hoverBorderColor: "#FFF",
                        hoverBorderWidth: 5,
                        weight: 5,
                        data: [ this.activatedCount, this.deactivatedCount ]
                    } ]
                };
            },
            options() {
                return {
                    responsive: true,
                    maintainAspectRatio: false,
                    rotation: Math.PI,
                    circumference: Math.PI,
                    cutoutPercentage: 80,
                    legend: {
                        labels: {
                            fontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                            fontSize: 16
                        },
                        position: "right"
                    },
                    title: {
                        display: true,
                        text: "User Status",
                        fontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                        fontSize: 18,
                        position: "left"
                    },
                    tooltips: {
                        bodyFontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                        bodyFontSize: 16,
                        displayColors: false,
                        callbacks: {
                            label: function (tooltipItem, data) {
                                let label = data.labels[tooltipItem.index] || "";

                                if (label) {
                                    label += ": ";
                                }

                                let value = parseInt(data.datasets[0].data[tooltipItem.index]);
                                label += value + ` user${value > 1 ? "s" : ""}`;
                                return label;
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