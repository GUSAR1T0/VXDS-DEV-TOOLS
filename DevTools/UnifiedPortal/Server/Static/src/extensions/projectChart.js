import { Pie } from "vue-chartjs";

export default {
    extends: Pie,
    props: {
        chart: Object,
        options: Object
    },
    mounted() {
        this.renderChart(this.chart, this.options);
    }
};
