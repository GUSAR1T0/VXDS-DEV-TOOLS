<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="Projects"
                    :reset-filters="resetFilters"
                    :reload="loadProjects"
                    :settings="settings"
            >
                <template slot="buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Create New Project
                        </div>
                        <el-button type="primary" circle
                                   class="rounded-button">
                            <span><fa icon="plus-circle"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="filters">
                    <ProjectsTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <ProjectsTable :projects="items"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_PROJECTS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage, getOnlyNumbers } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import ProjectsTableFilters from "@/components/projects/ProjectsTableFilters";
    import ProjectsTable from "@/components/projects/ProjectsTable";

    export default {
        name: "Projects",
        components: {
            LoadingContainer,
            FilterableTableView,
            ProjectsTableFilters,
            ProjectsTable
        },
        data() {
            return {
                loadingIsActive: true,
                settings: {
                    pageNo: 1,
                    pageSize: 10,
                    total: 0
                },
                filter: {
                    ids: [],
                    names: [],
                    aliases: [],
                    gitHubRepoIds: [],
                    isActive: null,

                    gitHubRepoIdOptions: [],
                    gitHubRepoIdsSearchLoading: false
                },
                items: []
            };
        },
        methods: {
            loadProjects() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    names: this.filter.names,
                    aliases: this.filter.aliases,
                    gitHubRepoIds: this.filter.gitHubRepoIds,
                    isActive: this.filter.isActive
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_PROJECTS_ENDPOINT,
                    data: {
                        pageNo: this.settings.pageNo - 1,
                        pageSize: this.settings.pageSize,
                        filter: request
                    },
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.settings.total = response.data.total;
                    this.items = response.data.items;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of projects",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.names = [];
                this.filter.aliases = [];
                this.filter.gitHubRepoIds = [];
                this.filter.isActive = null;

                this.filter.gitHubRepoIdOptions = [];
                this.filter.gitHubRepoIdsSearchLoading = false;
            }
        },
        mounted() {
            this.loadProjects();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadProjects();
            next();
        }
    };
</script>