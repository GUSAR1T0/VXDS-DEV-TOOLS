<template>
    <DashboardCard
            entityName="System Statistics"
            entityIcon="file-alt"
            linkToEntity="/system/operations"
            :load="load"
    >
        <template v-slot="{ state }">
            <Blocks style="width: 100%">
                <template slot="first">
                    <OperationsChart
                            style="height: 400px; width: 100%"
                            v-if="!state.loadingIsActive"
                            :dates="dates"
                            :operations="operations"
                    />
                    <el-divider/>
                    <div>
                        <div class="dashboard-total-key">Total:</div>
                        <div class="dashboard-total-value">
                            <strong>{{ operationsTotal > 0 ? operationsTotal : "—" }}</strong>
                        </div>
                    </div>
                </template>
                <template slot="second">
                    <LogsChart
                            style="height: 400px; width: 100%"
                            v-if="!state.loadingIsActive"
                            :dates="dates"
                            :logs="logs"
                    />
                    <el-divider/>
                    <div>
                        <div class="dashboard-total-key">Total:</div>
                        <div class="dashboard-total-value">
                            <strong>{{ logsTotal > 0 ? logsTotal : "—" }}</strong>
                        </div>
                    </div>
                </template>
            </Blocks>
        </template>
    </DashboardCard>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<style scoped>
    .card-content {
        width: 100%;
        display: flex;
    }

    .card-side {
        width: 50%;
        margin: 0 10px;
    }
</style>

<script>
    import { GET_SYSTEM_STATISTICS_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import Blocks from "@/components/page/Blocks";
    import DashboardCard from "@/components/dashboard/DashboardCard";
    import OperationsChart from "@/components/charts/OperationsChart";
    import LogsChart from "@/components/charts/LogsChart";

    export default {
        name: "SystemCard",
        components: {
            Blocks,
            DashboardCard,
            OperationsChart,
            LogsChart
        },
        data() {
            return {
                dates: [],
                operations: [],
                operationsTotal: 0,
                logs: [],
                logsTotal: 0
            };
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_SYSTEM_STATISTICS_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.dates = response.data.dates;
                    this.operations = response.data.operations;
                    this.operationsTotal = response.data.operationsTotal;
                    this.logs = response.data.logs;
                    this.logsTotal = response.data.logsTotal;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load system statistics data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>