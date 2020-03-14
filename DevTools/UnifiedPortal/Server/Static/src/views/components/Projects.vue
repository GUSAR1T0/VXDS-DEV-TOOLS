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
                        <el-button type="primary" circle v-if="hasPermissionToManageProjects"
                                   @click="openDialogToCreate" class="rounded-button">
                            <span><fa icon="plus-circle"/></span>
                        </el-button>
                    </el-tooltip>

                    <ProjectEditForm v-if="hasPermissionToManageProjects"
                                     :dialog-status="dialogProjectFormStatus"
                                     :project="getProject"
                                     :project-form="getProjectForm"
                                     :closed="submitProjectAction"
                                     :git-hub-repo-id-options="[]"
                                     :git-hub-token-setup="gitHubTokenSetup"/>
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
    import { mapGetters } from "vuex";
    import { POST_HTTP_REQUEST, RESET_PROJECT_STORE_STATE } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { GET_PROJECTS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage, getOnlyNumbers } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import ProjectsTableFilters from "@/components/projects/ProjectsTableFilters";
    import ProjectsTable from "@/components/projects/ProjectsTable";
    import ProjectEditForm from "@/components/projects/ProjectEditForm";

    export default {
        name: "Projects",
        components: {
            LoadingContainer,
            FilterableTableView,
            ProjectsTableFilters,
            ProjectsTable,
            ProjectEditForm
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
                gitHubTokenSetup: null,
                items: [],
                dialogProjectFormStatus: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getProject",
                "getProjectForm"
            ]),
            hasPermissionToManageProjects() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_PROJECTS);
            }
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
                    this.gitHubTokenSetup = response.data.gitHubTokenSetup;
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
            },
            openDialogToCreate() {
                this.$store.commit(RESET_PROJECT_STORE_STATE);
                this.dialogProjectFormStatus.visible = true;
            },
            submitProjectAction() {
                this.loadProjects();
                this.$store.commit(RESET_PROJECT_STORE_STATE);
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