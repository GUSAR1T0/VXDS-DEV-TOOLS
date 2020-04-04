<template>
    <DashboardMainCard icon="exclamation-circle" :load="load">
        <template slot>
            <div style="display: flex; justify-content: center">
                <el-link
                        :href="`/system/operations?incidentAssignee=${getUserId}:${getFullName}&hasIncident=true&isIncidentActive=true`"
                        type="primary"
                        :underline="false"
                        style="padding-bottom: 50px"
                >
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Assigned Incidents
                        </div>
                        <div :style="`font-size: ${getAssigneeIncidentsNumberFontSize}px`">
                            <strong>{{ assigneeIncidentsCount }}</strong>
                        </div>
                    </el-tooltip>
                </el-link>
                <div style="font-size: 108px; margin: 0 10px">
                    /
                </div>
                <el-link
                        :href="`/system/operations?incidentAuthor=${getUserId}:${getFullName}&hasIncident=true&isIncidentActive=true`"
                        type="primary"
                        :underline="false"
                        style="padding-top: 50px"
                >
                    <el-tooltip effect="dark" placement="bottom">
                        <div slot="content">
                            Your Incidents
                        </div>
                        <div :style="`font-size: ${getAuthorIncidentsNumberFontSize}px`">
                            <strong>{{ authorIncidentsCount }}</strong>
                        </div>
                    </el-tooltip>
                </el-link>
            </div>
        </template>
    </DashboardMainCard>
</template>

<script>
    import { mapGetters } from "vuex";
    import { GET_INCIDENTS_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardMainCard from "@/components/dashboard/DashboardMainCard";

    export default {
        name: "IncidentsCard",
        components: {
            DashboardMainCard
        },
        data() {
            return {
                assigneeIncidentsCount: 0,
                authorIncidentsCount: 0
            };
        },
        computed: {
            ...mapGetters([
                "getUserId",
                "getFullName"
            ]),
            getAssigneeIncidentsNumberFontSize() {
                return this.assigneeIncidentsCount >= 10 ? 36 : 48;
            },
            getAuthorIncidentsNumberFontSize() {
                return this.authorIncidentsCount >= 10 ? 36 : 48;
            }
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_INCIDENTS_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.assigneeIncidentsCount = response.data.assigneeIncidentsCount;
                    this.authorIncidentsCount = response.data.authorIncidentsCount;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load incident data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>