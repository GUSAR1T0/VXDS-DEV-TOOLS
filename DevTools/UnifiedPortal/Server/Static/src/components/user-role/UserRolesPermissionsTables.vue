<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <ActionToolbar>
                <template slot="left">
                    <el-pagination :total="userRoles.length" layout="total"/>
                </template>
                <template slot="right">
                    <el-button v-if="isEditable && hasPermissionToManageUserRoles(null)" type="primary"
                               @click="openDialogToCreateOrUpdate">
                        <span><fa icon="plus-circle"/><strong> | Create User Role</strong></span>
                    </el-button>
                </template>
            </ActionToolbar>
            <el-card shadow="hover" style="margin-top: 25px;" v-for="item in userRoles" :key="item.id">
                <div slot="header" style="text-align: center">
                    <h3>{{ item.name }}</h3>
                </div>
                <UserRolePermissionsTable :user-role-permissions="userRolePermissions" :user-role="item"/>
                <el-row v-if="isEditable && hasPermissionToManageUserRoles(item.id)" style="margin-top: 25px;"
                        type="flex" justify="center" align="middle" :gutter="20">
                    <el-col :span="12">
                        <el-button type="primary" plain class="user-roles-button"
                                   @click="openDialogToCreateOrUpdate(item)">
                            <span><fa icon="edit"/><strong> | Edit User Role</strong></span>
                        </el-button>
                    </el-col>
                    <el-col :span="12">
                        <el-button type="danger" plain class="user-roles-button"
                                   @click="openDeleteUserRoleDialog(item)" ref="deleteUserRoleButton">
                            <span><fa icon="minus-circle"/><strong> | Delete User Role</strong></span>
                        </el-button>
                    </el-col>
                </el-row>
                <ConfirmationDialog v-if="isEditable && hasPermissionToManageUserRoles(item.id)"
                                    :dialog-status="dialogUserRoleDeleteStatus"
                                    :confirmation-text="dialogUserRoleDeleteStatus.confirmationText"
                                    :additional-text="dialogUserRoleDeleteStatus.additionalText"
                                    :cancel-click-action="() => dialogUserRoleDeleteStatus.visible = false"
                                    :submit-click-action="button => deleteUserRole(button, item)"
                                    :closed="submitUserRoleAction"/>
            </el-card>
            <UserRoleForm v-if="isEditable"
                          :dialog-status="dialogUserRoleFormStatus"
                          :user-role-permissions="userRolePermissions"
                          :permissions-for-user-role="permissionsForUserRole"
                          :user-role="getUserRole"
                          :user-role-form="getUserRoleForm"
                          :closed="submitUserRoleAction"/>
        </template>
    </LoadingContainer>
</template>

<style scoped>
    .user-roles-button {
        width: 100%;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import {
        DELETE_HTTP_REQUEST,
        GET_HTTP_REQUEST,
        PREPARE_USER_ROLE_FORM,
        RESET_USER_ROLE_STORE_STATE,
        STORE_USER_ROLE_DATA_REQUEST
    } from "@/constants/actions";
    import {
        DELETE_USER_ROLE_ENDPOINT,
        GET_AFFECTED_USERS_COUNT_BY_USER_ROLE_ENDPOINT,
        GET_USER_ROLES_FULL_INFO_ENDPOINT,
        GET_USER_ROLE_PERMISSIONS_ENDPOINT
    } from "@/constants/endpoints";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserRolePermissionsTable from "@/components/user-role/UserRolePermissionsTable";
    import UserRoleForm from "@/components/user-role/UserRoleForm";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";
    import ActionToolbar from "@/components/page/ActionToolbar";

    export default {
        name: "UserRolesPermissionsTables",
        props: {
            isEditable: Boolean
        },
        components: {
            LoadingContainer,
            UserRolePermissionsTable,
            UserRoleForm,
            ConfirmationDialog,
            ActionToolbar
        },
        data() {
            return {
                loadingIsActive: true,
                userRoles: [],
                userRolePermissions: [],
                permissionsForUserRole: {},
                dialogUserRoleFormStatus: {
                    visible: false
                },
                dialogUserRoleDeleteStatus: {
                    visible: false,
                    confirmationText: "",
                    additionalText: ""
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getUserRoleId",
                "getUserRole",
                "getUserRoleForm"
            ])
        },
        methods: {
            loadUserRoles() {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLE_PERMISSIONS_ENDPOINT,
                    config: getConfiguration()
                }).then(firstResponse => {
                    this.userRolePermissions = firstResponse.data;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: GET_USER_ROLES_FULL_INFO_ENDPOINT,
                        config: getConfiguration()
                    }).then(secondResponse => {
                        this.loadingIsActive = false;
                        this.userRoles = secondResponse.data;
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
            hasPermissionToManageUserRoles(id) {
                if (this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_USER_ROLES)) {
                    return id ? id !== 1 && id !== this.getUserRoleId : true;
                }
                return false;
            },
            openDialogToCreateOrUpdate(userRole) {
                this.$store.commit(STORE_USER_ROLE_DATA_REQUEST, userRole);
                this.$store.commit(PREPARE_USER_ROLE_FORM);
                this.permissionsForUserRole = this.getPermissionsForUserRole(userRole);
                this.dialogUserRoleFormStatus.visible = true;
            },
            submitUserRoleAction() {
                this.loadUserRoles();
                this.permissionsForUserRole = {};
                this.$store.commit(RESET_USER_ROLE_STORE_STATE);
            },
            openDeleteUserRoleDialog(userRole) {
                this.$refs.deleteUserRoleButton.loading = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_AFFECTED_USERS_COUNT_BY_USER_ROLE_ENDPOINT, {
                        id: userRole.id
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.$refs.deleteUserRoleButton.loading = false;

                    let count = response.data;
                    this.dialogUserRoleDeleteStatus.confirmationText = `Are you sure that you want to delete user role "${userRole.name}"?`;
                    this.dialogUserRoleDeleteStatus.additionalText = count > 0 ? `Count of affected users: ${count}` : "";
                    this.dialogUserRoleDeleteStatus.visible = true;
                }).catch(error => {
                    this.$refs.deleteUserRoleButton.loading = false;
                    this.$notify.error({
                        title: "Failed to get affected users count",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            deleteUserRole(button, userRole) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_USER_ROLE_ENDPOINT, {
                        id: userRole.id
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogUserRoleDeleteStatus.visible = false;

                    this.$notify.success({
                        title: "User role was deleted",
                        message: `User role "${userRole.name}" was removed`
                    });
                }).catch(error => {
                    button.loading = false;
                    this.dialogUserRoleDeleteStatus.visible = false;

                    this.$notify.error({
                        title: "Failed to delete user role",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
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
            this.loadUserRoles();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadUserRoles();
            next();
        }
    };
</script>
