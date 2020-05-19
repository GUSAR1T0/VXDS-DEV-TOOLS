<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="Users"
                    :reset-filters="resetFilters"
                    :reload="loadUsers"
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
                    <UsersTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <UsersTable :users="items" :reload="loadUsers"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { getConfiguration, renderErrorNotificationMessage, getOnlyNumbers } from "@/extensions/utils";
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_USERS_ENDPOINT } from "@/constants/endpoints";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import UsersTable from "@/components/user/UsersTable";
    import UsersTableFilters from "@/components/user/UsersTableFilters";

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
                    ids: getOnlyNumbers(this.filter.ids),
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

                this.filter.userRoleIdOptions = [];
                this.filter.userRoleIdsSearchLoading = false;
            },
            searchByUserRole(userRole) {
                if (userRole) {
                    let userRoleIdAndName = userRole.split(":");
                    if (userRoleIdAndName && userRoleIdAndName.length === 2 && !isNaN(userRoleIdAndName[0]) && userRoleIdAndName[1]) {
                        let userRoleId = parseInt(userRoleIdAndName[0]);
                        this.filter.userRoleIdOptions = [ {
                            id: userRoleId,
                            name: userRoleIdAndName[1]
                        } ];
                        this.filter.userRoleIds = [ userRoleId ];
                    }
                }
            }
        },
        mounted() {
            this.searchByUserRole(this.$route.query.userRole);
            this.loadUsers();
        },
        beforeRouteUpdate(to, from, next) {
            this.searchByUserRole(to.query.userRole);
            this.loadUsers();
            next();
        }
    };
</script>