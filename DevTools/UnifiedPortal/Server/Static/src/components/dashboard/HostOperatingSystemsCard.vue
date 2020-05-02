<template>
    <DashboardCardWithTotal
            :total="operatingSystemsTotal"
            entityName="Host Operating Systems"
            entityIcon="network-wired"
            linkToEntity="/system/settings?tab=environment"
            :load="load"
    >
        <template v-slot="{ state }">
            <HostOperatingSystemsChart v-if="!state.loadingIsActive"
                                       :operating-systems="operatingSystems"
                                       style="height: 150px"
            />
        </template>
    </DashboardCardWithTotal>
</template>

<script>
    import { GET_HOST_OPERATING_SYSTEMS_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardCardWithTotal from "@/components/dashboard/DashboardCardWithTotal";
    import HostOperatingSystemsChart from "@/components/charts/HostOperatingSystemsChart";

    export default {
        name: "HostOperatingSystemsCard",
        components: {
            DashboardCardWithTotal,
            HostOperatingSystemsChart
        },
        data() {
            return {
                operatingSystemsTotal: 0,
                operatingSystems: []
            };
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_HOST_OPERATING_SYSTEMS_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.operatingSystemsTotal = response.data.total;
                    this.operatingSystems = response.data.operatingSystems;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load host operating systems data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>