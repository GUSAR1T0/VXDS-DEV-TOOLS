<script>
    import { Pie } from "vue-chartjs";
    import { mapGetters } from "vuex";

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
        name: "HostOperatingSystemsChart",
        extends: Pie,
        props: {
            operatingSystems: Array
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ]),
            chartData() {
                return {
                    labels: this.operatingSystems.map(item => this.getOperatingSystemName(item.operatingSystem)),
                    datasets: [ {
                        borderWidth: 1,
                        backgroundColor: getColors(this.operatingSystems.length),
                        borderColor: "#FFF",
                        hoverBorderColor: "#FFF",
                        hoverBorderWidth: 5,
                        weight: 5,
                        data: this.operatingSystems.map(item => item.count)
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
                        text: "Popular OS",
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
                                label += value + ` host${value > 1 ? "s" : ""}`;
                                return label;
                            }
                        }
                    }
                };
            }
        },
        methods: {
            getOperatingSystemName(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => parseInt(os.value) === osId);
                return osList && osList.length > 0 ? osList[0].name : "—";
            }
        },
        mounted() {
            this.renderChart(this.chartData, this.options);
        }
    };
</script>