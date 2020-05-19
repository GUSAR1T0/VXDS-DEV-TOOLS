<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="Modules"
                    :reset-filters="resetFilters"
                    :reload="loadModules"
                    :settings="settings"
            >
                <template slot="buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Reload This Page
                        </div>
                        <el-button type="info" plain circle @click="$router.go(0)"
                                   class="rounded-button">
                            <span><fa icon="sync-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Upload Module Configuration
                        </div>
                        <el-button type="primary" plain circle v-if="hasPermissionToManageModules"
                                   class="rounded-button" @click="uploadDialog.visible = true">
                            <span><fa icon="upload"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="filters">
                    <ModulesTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <ModulesTable :modules="items"/>
                </template>
            </FilterableTableView>

            <ModuleConfigurationUploadForm
                    :dialog-status="uploadDialog"
                    :closed="loadModules"
            />
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { getConfiguration, getOnlyNumbers, renderErrorNotificationMessage } from "@/extensions/utils";
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_MODULES_ENDPOINT } from "@/constants/endpoints";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import ModulesTableFilters from "@/components/modules/ModulesTableFilters";
    import ModulesTable from "@/components/modules/ModulesTable";
    import ModuleConfigurationUploadForm from "@/components/modules/ModuleConfigurationUploadForm";

    export default {
        name: "Modules",
        components: {
            LoadingContainer,
            FilterableTableView,
            ModulesTableFilters,
            ModulesTable,
            ModuleConfigurationUploadForm
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
                    userIds: [],
                    hostIds: [],
                    statuses: [],

                    userIdOptions: [],
                    userIdsSearchLoading: false,
                    hostIdOptions: [],
                    hostIdsSearchLoading: false
                },
                items: [],
                uploadDialog: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission"
            ]),
            hasPermissionToManageModules() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_MODULES);
            }
        },
        methods: {
            loadModules() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    names: this.filter.names,
                    aliases: this.filter.aliases,
                    userIds: this.filter.userIds,
                    hostIds: this.filter.hostIds,
                    statuses: getOnlyNumbers(this.filter.statuses)
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_MODULES_ENDPOINT,
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
                this.filter.userIds = [];
                this.filter.hostIds = [];
                this.filter.statuses = [];

                this.filter.userIdOptions = [];
                this.filter.userIdsSearchLoading = false;
                this.filter.hostIdOptions = [];
                this.filter.hostIdsSearchLoading = false;
            }
        },
        mounted() {
            this.loadModules();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadModules();
            next();
        }
    };
</script>