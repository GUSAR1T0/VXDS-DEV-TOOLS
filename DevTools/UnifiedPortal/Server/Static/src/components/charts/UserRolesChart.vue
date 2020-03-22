<script>
    import { Pie } from "vue-chartjs";

    function getColors(count) {
        let colors = [];

        if (count > 0) {
            colors.push("#0B3954");
        }

        if (count > 1) {
            colors.push("#085caa");
        }

        if (count > 2) {
            colors.push("#9cbedd");
        }

        return colors;
    }

    export default {
        name: "UserRolesChart",
        extends: Pie,
        props: {
            userRoles: Array
        },
        computed: {
            chartData() {
                return {
                    labels: this.userRoles.map(item => item.name),
                    datasets: [ {
                        borderWidth: 1,
                        backgroundColor: getColors(this.userRoles.length),
                        borderColor: "#FFF",
                        hoverBorderColor: "#FFF",
                        hoverBorderWidth: 5,
                        weight: 5,
                        data: this.userRoles.map(item => item.count)
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
                        text: "Popular Roles",
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