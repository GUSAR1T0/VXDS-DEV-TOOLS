<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="Operations"
                    :reset-filters="resetFilters"
                    :reload="loadOperations"
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
                </template>
                <template slot="filters">
                    <OperationsTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <OperationsTable :items="items" :reload="loadOperations"/>
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
    import { GET_OPERATION_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage, getOnlyNumbers, getDate } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import OperationsTableFilters from "@/components/operations/OperationsTableFilters";
    import OperationsTable from "@/components/operations/OperationsTable";

    export default {
        name: "Operations",
        components: {
            LoadingContainer,
            FilterableTableView,
            OperationsTableFilters,
            OperationsTable
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
                    scopes: [],
                    contextNames: [],
                    userIds: [],
                    isSystemAction: null,
                    isSuccessful: null,
                    startTimeRange: [],
                    stopTimeRange: [],
                    incidentAuthorIds: [],
                    incidentAssigneeIds: [],
                    incidentInitialTimeRange: [],
                    incidentStatuses: [],
                    hasIncident: null,

                    userIdOptions: [],
                    userIdsSearchLoading: false,
                    incidentAuthorIdOptions: [],
                    incidentAuthorIdsSearchLoading: false,
                    incidentAssigneeIdOptions: [],
                    incidentAssigneeIdsSearchLoading: false
                },
                items: []
            };
        },
        methods: {
            loadOperations() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    scopes: this.filter.scopes,
                    contextNames: this.filter.contextNames,
                    userIds: this.filter.userIds,
                    isSystemAction: this.filter.isSystemAction,
                    isSuccessful: this.filter.isSuccessful,
                    startTimeRange: this.filter.startTimeRange && this.filter.startTimeRange.length > 1 ? {
                        min: getDate(this.filter.startTimeRange[0]),
                        max: getDate(this.filter.startTimeRange[1])
                    } : null,
                    stopTimeRange: this.filter.stopTimeRange && this.filter.stopTimeRange.length > 1 ? {
                        min: getDate(this.filter.stopTimeRange[0]),
                        max: getDate(this.filter.stopTimeRange[1])
                    } : null,
                    incidentAuthorIds: getOnlyNumbers(this.filter.incidentAuthorIds),
                    incidentAssigneeIds: getOnlyNumbers(this.filter.incidentAssigneeIds),
                    incidentInitialTimeRange: this.filter.incidentInitialTimeRange && this.filter.incidentInitialTimeRange.length > 1 ? {
                        min: getDate(this.filter.incidentInitialTimeRange[0]),
                        max: getDate(this.filter.incidentInitialTimeRange[1])
                    } : null,
                    incidentStatuses: getOnlyNumbers(this.filter.incidentStatuses),
                    hasIncident: this.filter.hasIncident
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_OPERATION_LIST_ENDPOINT,
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
                        title: "Failed to load list of operations",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.scopes = [];
                this.filter.contextNames = [];
                this.filter.userIds = [];
                this.filter.isSystemAction = null;
                this.filter.isSuccessful = null;
                this.filter.startTimeRange = [];
                this.filter.stopTimeRange = [];
                this.filter.incidentAuthorIds = [];
                this.filter.incidentAssigneeIds = [];
                this.filter.incidentInitialTimeRange = [];
                this.filter.incidentStatuses = [];
                this.filter.hasIncident = null;

                this.filter.userIdOptions = [];
                this.filter.userIdsSearchLoading = false;
                this.filter.incidentAuthorIdOptions = [];
                this.filter.incidentAuthorIdsSearchLoading = false;
                this.filter.incidentAssigneeIdOptions = [];
                this.filter.incidentAssigneeIdsSearchLoading = false;
            },
            searchById(id) {
                if (id && !isNaN(id)) {
                    id = parseInt(id);
                    this.filter.ids = [ id ];
                }
            },
            searchByIncidentAssignee(user) {
                if (user) {
                    let userIdAndFullName = user.split(":");
                    if (userIdAndFullName && userIdAndFullName.length === 2 && !isNaN(userIdAndFullName[0]) && userIdAndFullName[1]) {
                        let userId = parseInt(userIdAndFullName[0]);
                        this.filter.incidentAssigneeIdOptions = [ {
                            id: userId,
                            fullName: userIdAndFullName[1]
                        } ];
                        this.filter.incidentAssigneeIds = [ userId ];
                    }
                }
            },
            searchByIncidentAuthor(user) {
                if (user) {
                    let userIdAndFullName = user.split(":");
                    if (userIdAndFullName && userIdAndFullName.length === 2 && !isNaN(userIdAndFullName[0]) && userIdAndFullName[1]) {
                        let userId = parseInt(userIdAndFullName[0]);
                        this.filter.incidentAuthorIdOptions = [ {
                            id: userId,
                            fullName: userIdAndFullName[1]
                        } ];
                        this.filter.incidentAuthorIds = [ userId ];
                    }
                }
            }
        },
        mounted() {
            this.searchById(this.$route.query.id);
            this.searchByIncidentAssignee(this.$route.query.incidentAssignee);
            this.searchByIncidentAuthor(this.$route.query.incidentAuthor);
            this.filter.hasIncident = this.$route.query.hasIncident !== undefined ? this.$route.query.hasIncident === "true" : null;
            this.filter.incidentStatuses = this.$route.query.isIncidentActive !== undefined && this.$route.query.isIncidentActive === "true" ? [ "1", "2" ] : [];
            this.loadOperations();
        },
        beforeRouteUpdate(to, from, next) {
            this.searchById(to.query.id);
            this.searchByIncidentAssignee(to.query.incidentAssignee);
            this.searchByIncidentAuthor(to.query.incidentAuthor);
            this.filter.hasIncident = to.query.hasIncident !== undefined ? to.query.hasIncident === "true" : null;
            this.filter.incidentStatuses = to.query.isIncidentActive !== undefined && to.query.isIncidentActive === "true" ? [ "1", "2" ] : [];
            this.loadOperations();
            next();
        }
    };
</script>