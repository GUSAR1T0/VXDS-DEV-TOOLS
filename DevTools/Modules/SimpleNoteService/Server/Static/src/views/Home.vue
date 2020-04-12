<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="Notes"
                    :reset-filters="resetFilters"
                    :reload="loadNotes"
                    :settings="settings"
            >
                <template slot="buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Create New Note
                        </div>
                        <el-button type="primary" circle class="rounded-button"
                                   @click="$router.push('/note')">
                            <span><fa icon="plus-circle"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="filters">
                    <NotesTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <NotesTable :notes="items" :reload="loadNotes"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<script>
    import { LOCALHOST } from "@/constants/servers";
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { GET_NOTE_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, getDate, getOnlyNumbers, renderErrorNotificationMessage } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import NotesTableFilters from "@/components/notes/NotesTableFilters";
    import NotesTable from "@/components/notes/NotesTable";

    export default {
        name: "Home",
        components: {
            LoadingContainer,
            FilterableTableView,
            NotesTableFilters,
            NotesTable
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
                    userIds: [],
                    titles: [],
                    editTimeRange: [],

                    userIdOptions: [],
                    userIdsSearchLoading: false
                },
                items: [],
                dialogNotificationFormStatus: {
                    visible: false
                }
            };
        },
        methods: {
            loadNotes() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    userIds: this.filter.userIds,
                    titles: this.filter.titles,
                    editTimeRange: this.filter.editTimeRange && this.filter.editTimeRange.length > 1 ? {
                        min: getDate(this.filter.editTimeRange[0]),
                        max: getDate(this.filter.editTimeRange[1])
                    } : null
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_NOTE_LIST_ENDPOINT,
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
                        title: "Failed to load list of notes",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.userIds = [];
                this.filter.titles = [];
                this.filter.editTimeRange = [];

                this.filter.userIdOptions = [];
                this.filter.userIdsSearchLoading = false;
            }
        },
        mounted() {
            this.loadNotes();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadNotes();
            next();
        }
    };
</script>
