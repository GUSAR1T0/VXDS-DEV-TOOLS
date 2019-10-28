<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <el-row :gutter="20">
                <el-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
                    <UserRolesCard :roles-count="rolesCount"/>
                </el-col>
                <el-col :lg="8" :xl="8" class="hidden-md-and-down">
                    <LogsCard :logs-count="logsCount"/>
                </el-col>
                <el-col :lg="8" :xl="8" class="hidden-md-and-down">
                    <el-card shadow="hover">
                        <div slot="header">
                            <h3>OMT</h3>
                        </div>
                        ...
                    </el-card>
                </el-col>
            </el-row>
            <el-row class="small-display-row hidden-lg-and-up">
                <el-col :xs="24" :sm="24" :md="24">
                    <LogsCard :logs-count="logsCount"/>
                </el-col>
            </el-row>
            <el-row class="small-display-row hidden-lg-and-up">
                <el-col :xs="24" :sm="24" :md="24">
                    <el-card shadow="hover">
                        <div slot="header">
                            <h3>OMT</h3>
                        </div>
                        ...
                    </el-card>
                </el-col>
            </el-row>
        </template>
    </LoadingContainer>
</template>

<style scoped>
    .small-display-row {
        margin-top: 20px;
    }
</style>

<script>
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { GET_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";

    import UserRolesCard from "@/components/dashboard/UserRolesCard";
    import LogsCard from "@/components/dashboard/LogsCard";
    import LoadingContainer from "@/components/page/LoadingContainer";

    export default {
        name: "Home",
        components: {
            LoadingContainer,
            UserRolesCard,
            LogsCard
        },
        data() {
            return {
                loadingIsActive: true,
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
