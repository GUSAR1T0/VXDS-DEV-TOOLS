<template>
    <DashboardCardWithTotal
            :total="modulesTotal"
            entityName="Modules"
            entityIcon="cubes"
            linkToEntity="/components/modules"
            :load="load"
    >
        <template v-slot="{ state }">
            <ModulesChart v-if="!state.loadingIsActive"
                           :active-count="activeCount"
                           :inactive-count="inactiveCount"
                           style="height: 150px"
            />
        </template>
    </DashboardCardWithTotal>
</template>

<script>
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_MODULES_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardCardWithTotal from "@/components/dashboard/DashboardCardWithTotal";
    import ModulesChart from "@/components/charts/ModulesChart";

    export default {
        name: "ModulesCard",
        components: {
            DashboardCardWithTotal,
            ModulesChart
        },
        data() {
            return {
                activeCount: 0,
                inactiveCount: 0
            };
        },
        computed: {
            modulesTotal() {
                return this.activeCount + this.inactiveCount;
            }
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_MODULES_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.activeCount = response.data.activeCount;
                    this.inactiveCount = response.data.inactiveCount;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load modules data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>