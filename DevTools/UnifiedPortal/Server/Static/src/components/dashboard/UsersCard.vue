<template>
    <DashboardCardWithTotal
            :total="usersTotal"
            entityName="Users"
            entityIcon="users"
            linkToEntity="/users"
            :load="load"
    >
        <template v-slot="{ state }">
            <UsersChart v-if="!state.loadingIsActive"
                        :activated-count="activatedCount"
                        :deactivated-count="deactivatedCount"
                        style="height: 150px"
            />
        </template>
    </DashboardCardWithTotal>
</template>

<script>
    import { GET_USERS_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardCardWithTotal from "@/components/dashboard/DashboardCardWithTotal";
    import UsersChart from "@/components/charts/UsersChart";

    export default {
        name: "UsersCard",
        components: {
            DashboardCardWithTotal,
            UsersChart
        },
        data() {
            return {
                activatedCount: 0,
                deactivatedCount: 0
            };
        },
        computed: {
            usersTotal() {
                return this.activatedCount + this.deactivatedCount;
            }
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USERS_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.activatedCount = response.data.activatedCount;
                    this.deactivatedCount = response.data.deactivatedCount;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load users data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>