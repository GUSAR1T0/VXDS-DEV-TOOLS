<template>
    <DashboardMainCard icon="bell" :load="load">
        <template slot>
            <el-link :href="`/system/notifications`" type="primary" :underline="false">
                <div style="font-size: 72px">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Active Notifications
                        </div>
                        <strong>{{ notificationsCount }}</strong>
                    </el-tooltip>
                </div>
            </el-link>
        </template>
    </DashboardMainCard>
</template>

<script>
    import { GET_NOTIFICATIONS_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardMainCard from "@/components/dashboard/DashboardMainCard";

    export default {
        name: "NotificationsCard",
        components: {
            DashboardMainCard
        },
        data() {
            return {
                notificationsCount: null
            };
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_NOTIFICATIONS_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.notificationsCount = response.data.notificationsCount;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load notifications data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>