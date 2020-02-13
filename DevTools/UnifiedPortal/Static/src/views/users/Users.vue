<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    :reset-filters="resetFilters"
                    :reload="loadUsers"
                    :settings="settings"
            >
                <template slot="filters">
                    <UsersTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <UsersTable :users="items" :reload="loadUsers"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<script>
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_USERS_ENDPOINT } from "@/constants/endpoints";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import UsersTable from "@/components/users/UsersTable";
    import UsersTableFilters from "@/components/users/UsersTableFilters";

    export default {
        name: "Users",
        components: {
            LoadingContainer,
            FilterableTableView,
            UsersTable,
            UsersTableFilters
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
                    userNames: [],
                    emails: [],
                    userRoleIds: [],
                    isActivated: null,

                    idOptions: [],
                    idGenerator: 0,
                    userNameOptions: [],
                    userNameGenerator: 0,
                    emailOptions: [],
                    emailGenerator: 0,
                    userRoleIdOptions: [],
                    userRoleIdsSearchLoading: false
                },
                items: []
            };
        },
        methods: {
            loadUsers() {
                this.loadingIsActive = true;
                let request = {
                    ids: this.filter.ids,
                    userNames: this.filter.userNames,
                    emails: this.filter.emails,
                    userRoleIds: this.filter.userRoleIds,
                    isActivated: this.filter.isActivated
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USERS_ENDPOINT,
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
                        title: "Failed to load list of users",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.userNames = [];
                this.filter.emails = [];
                this.filter.userRoleIds = [];
                this.filter.isActivated = null;

                this.filter.idOptions = [];
                this.filter.idGenerator = 0;
                this.filter.userNameOptions = [];
                this.filter.userNameGenerator = 0;
                this.filter.emailOptions = [];
                this.filter.emailGenerator = 0;
                this.filter.userRoleIdOptions = [];
                this.filter.userRoleIdsSearchLoading = false;
            }
        },
        mounted() {
            this.loadUsers();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadUsers();
            next();
        }
    };
</script>