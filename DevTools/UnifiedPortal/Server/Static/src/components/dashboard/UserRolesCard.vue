<template>
    <DashboardCardWithTotal
            :total="userRolesTotal"
            entityName="User Roles"
            entityIcon="users-cog"
            linkToEntity="/users/roles"
            :load="load"
    >
        <template v-slot="{ state }">
            <UserRolesChart v-if="!state.loadingIsActive"
                            :user-roles="userRoles"
                            style="height: 150px"
            />
        </template>
    </DashboardCardWithTotal>
</template>

<script>
    import { GET_USER_ROLES_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardCardWithTotal from "@/components/dashboard/DashboardCardWithTotal";
    import UserRolesChart from "@/components/charts/UserRolesChart";

    export default {
        name: "UserRolesCard",
        components: {
            DashboardCardWithTotal,
            UserRolesChart
        },
        data() {
            return {
                userRolesTotal: 0,
                userRoles: []
            };
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLES_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.userRolesTotal = response.data.total;
                    this.userRoles = response.data.userRoles;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load user roles data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>