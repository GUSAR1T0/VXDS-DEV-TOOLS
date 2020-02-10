<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <el-collapse v-model="activeCollapseItems">
                <el-collapse-item title="Users" name="users">
                    <Blocks>
                        <template slot="first">
                            <UsersCard :users-count="usersCount"/>
                        </template>
                        <template slot="second">
                            <UserRolesCard :roles-count="rolesCount"/>
                        </template>
                    </Blocks>
                </el-collapse-item>
                <el-collapse-item title="System" name="system">
                    <Blocks>
                        <template slot="first">
                            <el-card shadow="hover">
                                <div slot="header">
                                    <h3>System Statistics</h3>
                                </div>
                                <Blocks>
                                    <template slot="first">
                                        <OperationsCard :operations-count="operationsCount"/>
                                    </template>
                                    <template slot="second">
                                        <LogsCard :logs-count="logsCount"/>
                                    </template>
                                </Blocks>
                                <el-button type="primary" plain class="system-row-button"
                                           @click="$router.push('/system/operations')">
                                    <span>See Operations</span>
                                </el-button>
                            </el-card>
                        </template>
                    </Blocks>
                </el-collapse-item>
            </el-collapse>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { GET_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";

    import Blocks from "@/components/page/Blocks";
    import UsersCard from "@/components/dashboard/UsersCard";
    import UserRolesCard from "@/components/dashboard/UserRolesCard";
    import LogsCard from "@/components/dashboard/LogsCard";
    import OperationsCard from "@/components/dashboard/OperationsCard";
    import LoadingContainer from "@/components/page/LoadingContainer";

    export default {
        name: "Home",
        components: {
            Blocks,
            LoadingContainer,
            UsersCard,
            UserRolesCard,
            LogsCard,
            OperationsCard
        },
        data() {
            return {
                loadingIsActive: true,
                activeCollapseItems: [ "users", "system" ],
                usersCount: "—",
                rolesCount: "—",
                operationsCount: "—",
                logsCount: "—"
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
                    this.operationsCount = response.data.operationsCount;
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
