<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    :reset-filters="resetFilters"
                    :reload="loadOperations"
                    :settings="settings"
            >
                <template slot="filters">
                    <OperationsTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <OperationsTable :items="items"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<script>
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_OPERATION_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

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

                    idOptions: [],
                    idGenerator: 0,
                    scopeOptions: [],
                    scopeGenerator: 0,
                    contextNameOptions: [],
                    contextNameGenerator: 0,
                    userIdOptions: [],
                    userIdsSearchLoading: false
                },
                items: []
            };
        },
        methods: {
            getDate(date) {
                let transformer = (value) => ("0" + value).slice(-2);
                if (date) {
                    let year = date.getFullYear();
                    let month = date.getMonth() + 1;
                    let day = date.getDate();
                    let hour = date.getHours();
                    let minute = date.getMinutes();
                    let second = date.getSeconds();
                    return `${year}-${transformer(month)}-${transformer(day)}T${transformer(hour)}:${transformer(minute)}:${transformer(second)}`;
                } else {
                    return null;
                }
            },
            loadOperations() {
                this.loadingIsActive = true;
                let request = {
                    ids: this.filter.ids,
                    scopes: this.filter.scopes,
                    contextNames: this.filter.contextNames,
                    userIds: this.filter.userIds,
                    isSystemAction: this.filter.isSystemAction,
                    isSuccessful: this.filter.isSuccessful,
                    startTimeRange: this.filter.startTimeRange && this.filter.startTimeRange.length > 1 ? {
                        min: this.getDate(this.filter.startTimeRange[0]),
                        max: this.getDate(this.filter.startTimeRange[1])
                    } : null,
                    stopTimeRange: this.filter.stopTimeRange && this.filter.stopTimeRange.length > 1 ? {
                        min: this.getDate(this.filter.stopTimeRange[0]),
                        max: this.getDate(this.filter.stopTimeRange[1])
                    } : null
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
                this.filter.isSystemAction = null;
                this.filter.isSuccessful = null;
                this.filter.startTimeRange = [];
                this.filter.stopTimeRange = [];

                this.filter.idOptions = [];
                this.filter.idGenerator = 0;
                this.filter.scopeOptions = [];
                this.filter.scopeGenerator = 0;
                this.filter.contextNameOptions = [];
                this.filter.contextNameGenerator = 0;
                this.filter.userIdOptions = [];
                this.filter.userIdsSearchLoading = false;
            },
            searchById(id) {
                if (id && !isNaN(id)) {
                    id = parseInt(id);
                    this.filter.ids = [id];
                }
            }
        },
        mounted() {
            this.searchById(this.$route.query.id);
            this.loadOperations();
        },
        beforeRouteUpdate(to, from, next) {
            this.searchById(to.query.id);
            this.loadOperations();
            next();
        }
    };
</script>