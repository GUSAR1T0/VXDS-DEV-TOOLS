<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <el-collapse v-model="activeCollapseItems">
                <el-collapse-item title="Users" name="users">
                    <DashboardBlocks>
                        <template slot="first">
                            <UsersCard :users-count="usersCount"/>
                        </template>
                        <template slot="second">
                            <UserRolesCard :roles-count="rolesCount"/>
                        </template>
                    </DashboardBlocks>
                </el-collapse-item>
                <el-collapse-item title="System" name="system">
                    <DashboardBlocks>
                        <template slot="first">
                            <LogsCard :logs-count="logsCount"/>
                        </template>
                    </DashboardBlocks>
                </el-collapse-item>
            </el-collapse>
        </template>
    </LoadingContainer>
</template>

<script>
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { GET_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";

    import DashboardBlocks from "@/components/dashboard/DashboardBlocks";
    import UsersCard from "@/components/dashboard/UsersCard";
    import UserRolesCard from "@/components/dashboard/UserRolesCard";
    import LogsCard from "@/components/dashboard/LogsCard";
    import LoadingContainer from "@/components/page/LoadingContainer";

    export default {
        name: "Home",
        components: {
            DashboardBlocks,
            LoadingContainer,
            UsersCard,
            UserRolesCard,
            LogsCard
        },
        data() {
            return {
                loadingIsActive: true,
                activeCollapseItems: [ "users", "system" ],
                usersCount: 0,
                rolesCount: 0,
                logsCount: 0
            };
        },
        methods: {
            loadDashboardData() {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.usersCount = response.data.usersCount;
                    this.rolesCount = response.data.rolesCount;
                    this.logsCount = response.data.logsCount;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load data for admin panel",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
        },
        mounted() {
            this.loadDashboardData();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadDashboardData();
            next();
        }
    };
</script>
