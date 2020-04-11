<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile header="Health Checks">
                <template slot="profile-content">
                    <el-table :data="checks" style="width: 100%" border>
                        <el-table-column label="Type of Health Check" align="center">
                            <template slot-scope="scope">
                                <strong style="font-size: 22px">{{ scope.row.type }}</strong>
                            </template>
                        </el-table-column>
                        <el-table-column label="Health Check Status" align="center">
                            <template slot-scope="scope">
                                <div style="font-size: 48px">
                                    <fa :icon="['far', defineIcon(scope.row)]" :class="defineClass(scope.row)"/>
                                </div>
                            </template>
                        </el-table-column>
                    </el-table>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/status.css">
</style>

<script>
    import { LOAD_HEALTH_CHECKS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import Profile from "@/components/page/Profile";

    export default {
        name: "HealthChecksPage",
        components: {
            LoadingContainer,
            Profile
        },
        data() {
            return {
                loadingIsActive: false,
                checks: []
            };
        },
        methods: {
            defineIcon(row) {
                if (row.isOk === true) {
                    return "check-circle";
                } else if (row.isOk === false) {
                    return "times-circle";
                } else {
                    return "";
                }
            },
            defineClass(row) {
                if (row.isOk === true) {
                    return "is-successful";
                } else if (row.isOk === false) {
                    return "is-unsuccessful";
                } else {
                    return "";
                }
            },
            loadHealthChecks() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: LOAD_HEALTH_CHECKS_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.checks = response.data;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load health checks data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.loadHealthChecks();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadHealthChecks();
            next();
        }
    };
</script>