<template>
    <DashboardCardWithTotal
            :total="projectsTotal"
            entityName="Projects"
            entityIcon="code"
            linkToEntity="/components/projects"
            :load="load"
    >
        <template v-slot="{ state }">
            <ProjectsChart v-if="!state.loadingIsActive"
                           :active-count="activeCount"
                           :inactive-count="inactiveCount"
                           style="height: 150px"
            />
        </template>
    </DashboardCardWithTotal>
</template>

<script>
    import { GET_PROJECTS_DATA_FOR_DASHBOARD_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import DashboardCardWithTotal from "@/components/dashboard/DashboardCardWithTotal";
    import ProjectsChart from "@/components/charts/ProjectsChart";

    export default {
        name: "ProjectsCard",
        components: {
            DashboardCardWithTotal,
            ProjectsChart
        },
        data() {
            return {
                activeCount: 0,
                inactiveCount: 0
            };
        },
        computed: {
            projectsTotal() {
                return this.activeCount + this.inactiveCount;
            }
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_PROJECTS_DATA_FOR_DASHBOARD_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.activeCount = response.data.activeCount;
                    this.inactiveCount = response.data.inactiveCount;
                    state.loadingIsActive = false;
                }).catch(error => {
                    // state.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load projects data",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>