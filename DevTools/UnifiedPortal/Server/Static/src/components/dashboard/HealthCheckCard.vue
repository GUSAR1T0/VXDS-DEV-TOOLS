<template>
    <el-card shadow="hover">
        <el-container v-loading="loadingIsActive" class="dashboard-main-item">
            <div v-if="!loadingIsActive" style="width: 100%">
                <el-link :href="`/system/health`" type="primary" :underline="false">
                    <div style="font-size: 72px">
                        <fa :icon="['far', defineIcon]" :class="defineClass"/>
                    </div>
                    <h3 v-if="isOk">System works fine</h3>
                    <h3 v-else>System has issues</h3>
                </el-link>
            </div>
        </el-container>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<style scoped src="@/styles/status.css">
</style>

<script>
    import { GET_SYSTEM_HEALTH_CHECK_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    export default {
        name: "HealthCheckCard",
        data() {
            return {
                loadingIsActive: true,
                isOk: null
            };
        },
        computed: {
            defineIcon() {
                if (this.isOk === true) {
                    return "check-circle";
                } else if (this.isOk === false) {
                    return "times-circle";
                } else {
                    return "";
                }
            },
            defineClass() {
                if (this.isOk === true) {
                    return "success";
                } else if (this.isOk === false) {
                    return "error";
                } else {
                    return "";
                }
            }
        },
        methods: {
            loadHealthCheckData() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_SYSTEM_HEALTH_CHECK_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.isOk = response.data.isOk;
                    this.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load system health check data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.loadHealthCheckData();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadHealthCheckData();
            next();
        }
    };
</script>