<template>
    <el-card shadow="hover">
        <el-container v-loading="loadingIsActive" class="dashboard-main-item">
            <div v-if="!loadingIsActive" style="width: 100%">
                <div style="font-size: 72px">
                    <fa :icon="['far', defineIcon]" :class="defineClass"/>
                </div>
                <h3 v-if="isOk">Server works fine</h3>
                <h3 v-else>Server has issues</h3>
            </div>
        </el-container>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<style scoped>
    .is-successful {
        color: #0C7C59;
    }

    .is-unsuccessful {
        color: #DB2B3D;
    }
</style>

<script>
    import { GET_SERVER_HEALTH_CHECK_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
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
                    return "is-successful";
                } else if (this.isOk === false) {
                    return "is-unsuccessful";
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
                    endpoint: GET_SERVER_HEALTH_CHECK_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.isOk = response.data.isOk;
                    this.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load server health check data",
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