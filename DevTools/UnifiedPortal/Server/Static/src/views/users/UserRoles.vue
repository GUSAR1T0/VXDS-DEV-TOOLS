<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="User Roles"
                    :reset-filters="resetFilters"
                    :reload="loadUserRoles"
                    :settings="settings"
            >
                <template slot="buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Create New User Role
                        </div>
                        <el-button type="primary" circle @click="openUserRoleEditFormToCreate"
                                   v-if="hasPermissionToManageUserRoles" class="rounded-button">
                            <span><fa icon="plus-circle"/></span>
                        </el-button>
                    </el-tooltip>
                    <UserRoleEditForm
                            :dialog-status="dialogUserRoleFormStatus"
                            :user-role-permissions="userRolePermissions"
                            :permissions-for-user-role="permissionsForUserRole"
                            :user-role="getUserRole"
                            :user-role-form="getUserRoleForm"
                            :closed="submitUserRoleAction"/>
                </template>
                <template slot="filters">
                    <UserRolesTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <UserRolesTable :user-roles="userRoles" :user-role-permissions="userRolePermissions"
                                    :reload="loadUserRoles" :get-permissions-for-user-role="getPermissionsForUserRole"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST, RESET_USER_ROLE_STORE_STATE } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_USER_ROLE_PERMISSIONS_ENDPOINT, GET_USER_ROLES_FULL_INFO_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage, getOnlyNumbers } from "@/extensions/utils";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import UserRolesTableFilters from "@/components/user-role/UserRolesTableFilters";
    import UserRolesTable from "@/components/user-role/UserRolesTable";
    import UserRoleEditForm from "@/components/user-role/UserRoleEditForm";

    export default {
        name: "UserRoles",
        components: {
            LoadingContainer,
            FilterableTableView,
            UserRolesTableFilters,
            UserRolesTable,
            UserRoleEditForm
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
                    userRoleNames: []
                },
                userRoles: [],
                userRolePermissions: [],
                permissionsForUserRole: {},
                dialogUserRoleFormStatus: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getUserRole",
                "getUserRoleForm"
            ]),
            hasPermissionToManageUserRoles() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_USER_ROLES);
            }
        },
        methods: {
            loadUserRoles() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    userRoleNames: this.filter.userRoleNames
                };
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLE_PERMISSIONS_ENDPOINT,
                    config: getConfiguration()
                }).then(firstResponse => {
                    this.userRolePermissions = firstResponse.data;
                    this.$store.dispatch(POST_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: GET_USER_ROLES_FULL_INFO_ENDPOINT,
                        data: {
                            pageNo: this.settings.pageNo - 1,
                            pageSize: this.settings.pageSize,
                            filter: request
                        },
                        config: getConfiguration()
                    }).then(secondResponse => {
                        this.loadingIsActive = false;
                        this.settings.total = secondResponse.data.total;
                        this.userRoles = secondResponse.data.items;
                    }).catch(error => {
                        this.loadingIsActive = false;
                        this.$notify.error({
                            title: "Failed to load list of user roles",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of user role permissions",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.userRoleNames = [];
            },
            searchById(id) {
                if (id && !isNaN(id)) {
                    id = parseInt(id);
                    this.filter.ids = [ id ];
                }
            },
            openUserRoleEditFormToCreate() {
                this.permissionsForUserRole = this.getPermissionsForUserRole({});
                this.dialogUserRoleFormStatus.visible = true;
            },
            submitUserRoleAction() {
                this.loadUserRoles();
                this.permissionsForUserRole = {};
                this.$store.commit(RESET_USER_ROLE_STORE_STATE);
            },
            getPermissionsForUserRole(userRole) {
                let x = {};
                for (let i = 0; i < this.userRolePermissions.length; i++) {
                    if (userRole.permissions) {
                        let y = userRole.permissions.filter(item => item.permissionGroupId === this.userRolePermissions[i].id);
                        x[this.userRolePermissions[i].id] = y && y.length > 0 ? y[0].permissions : [];
                    } else {
                        x[this.userRolePermissions[i].id] = [];
                    }
                }

                return x;
            }
        },
        mounted() {
            this.searchById(this.$route.query.id);
            this.loadUserRoles();
        },
        beforeRouteUpdate(to, from, next) {
            this.searchById(to.query.id);
            this.loadUserRoles();
            next();
        }
    };
</script>