<template>
    <el-card shadow="hover">
        <el-container v-loading="loadingIsActive" class="dashboard-main-item">
            <div v-if="!loadingIsActive" style="width: 100%">
                <div style="margin-top: -20px; display: flex; justify-content: center">
                    <strong style="font-size: 72px">
                        {{ hours }}
                    </strong>
                    <strong style="font-size: 72px; width: 20px">
                        {{ blink ? ":" : " "}}
                    </strong>
                    <strong style="font-size: 72px">
                        {{ minutes }}
                    </strong>
                </div>
                <div>
                    <strong style="font-size: 32px">{{ dayOfWeek }}</strong>
                </div>
                <div>
                    <strong style="font-size: 32px">{{ date }}</strong>
                </div>
            </div>
        </el-container>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    import { GET_SERVER_TIME_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    export default {
        name: "ServerTimeCard",
        data() {
            return {
                loadingIsActive: true,
                blink: true,
                hours: null,
                minutes: null,
                dayOfWeek: null,
                date: null
            };
        },
        methods: {
            loadTime() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_SERVER_TIME_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.hours = response.data.hours;
                    this.minutes = response.data.minutes;
                    this.dayOfWeek = response.data.dayOfWeek;
                    this.date = response.data.date;
                    this.loadingIsActive = false;
                    this.setupBlinkEffect();
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load server time data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            setupBlinkEffect() {
                setInterval(() => this.blink = !this.blink, 1000);
            }
        },
        mounted() {
            this.loadTime();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadTime();
            next();
        }
    };
</script>