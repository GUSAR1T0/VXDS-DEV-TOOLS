<template>
    <div>
        <el-table :data="userRoles" style="width: 100%" border highlight-current-row default-expand-all>
            <el-table-column type="expand">
                <template slot-scope="scope">
                    <UserRolePermissionsTable :user-role-permissions="userRolePermissions" :user-role="scope.row"/>
                </template>
            </el-table-column>
            <el-table-column label="Manage Role" min-width="200" align="center"
                             v-if="hasPermissionToManageUserRoles(null)">
                <template slot-scope="scope">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Edit This User Role
                        </div>
                        <el-button type="primary" plain circle @click="openDialogToCreateOrUpdate(scope.row)"
                                   v-if="hasPermissionToManageUserRoles(scope.row.id)" class="rounded-button">
                            <span><fa icon="edit"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Delete This User Role
                        </div>
                        <el-button type="danger" plain circle @click="openDeleteUserRoleDialog(scope.row)"
                                   v-if="hasPermissionToManageUserRoles(scope.row.id)" class="rounded-button"
                                   :ref="`deleteUserRoleButton-${scope.row.id}`">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
            </el-table-column>
            <el-table-column label="User Role ID" min-width="100" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="User Role Name" min-width="500" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 18px">{{ scope.row.name }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="User Count" min-width="100" align="center">
                <template slot-scope="scope">
                    <el-link :href="`/users?userRole=${scope.row.id}:${scope.row.name}`" type="primary"
                             :underline="false"
                             v-if="scope.row.userCount > 0">
                        <strong style="font-size: 16px">{{ scope.row.userCount }}</strong>
                    </el-link>
                    <div v-else>—</div>
                </template>
            </el-table-column>
        </el-table>

        <UserRoleEditForm
                :dialog-status="dialogUserRoleFormStatus"
                :user-role-permissions="userRolePermissions"
                :permissions-for-user-role="permissionsForUserRole"
                :user-role="getUserRole"
                :user-role-form="getUserRoleForm"
                :closed="submitUserRoleAction"/>
        <ConfirmationDialog
                :dialog-status="dialogUserRoleDeleteStatus"
                :confirmation-text="dialogUserRoleDeleteStatus.confirmationText"
                :additional-text="dialogUserRoleDeleteStatus.additionalText"
                :cancel-click-action="() => dialogUserRoleDeleteStatus.visible = false"
                :submit-click-action="deleteUserRole"
                :closed="submitUserRoleAction"/>
    </div>
</template>

<style scoped src="@/styles/button.css">
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
    import { DELETE_USER_ROLE_ENDPOINT, GET_AFFECTED_USERS_COUNT_BY_USER_ROLE_ENDPOINT } from "@/constants/endpoints";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import format from "string-format";

    import UserRolePermissionsTable from "@/components/user-role/UserRolePermissionsTable";
    import UserRoleEditForm from "@/components/user-role/UserRoleEditForm";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";

    export default {
        name: "UserRolesTable",
        props: {
            userRoles: Array,
            userRolePermissions: Array,
            reload: Function,
            getPermissionsForUserRole: Function
        },
        components: {
            UserRolePermissionsTable,
            UserRoleEditForm,
            ConfirmationDialog
        },
        data() {
            return {
                loadingIsActive: true,
                permissionsForUserRole: {},
                dialogUserRoleFormStatus: {
                    visible: false
                },
                dialogUserRoleDeleteStatus: {
                    visible: false,
                    id: null,
                    name: "",
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
                this.reload();
                this.permissionsForUserRole = {};
                this.$store.commit(RESET_USER_ROLE_STORE_STATE);
            },
            openDeleteUserRoleDialog(userRole) {
                let button = this.$refs[`deleteUserRoleButton-${userRole.id}`];
                button.loading = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_AFFECTED_USERS_COUNT_BY_USER_ROLE_ENDPOINT, {
                        id: userRole.id
                    }),
                    config: getConfiguration()
                }).then(response => {
                    button.loading = false;

                    let count = response.data;
                    this.dialogUserRoleDeleteStatus.confirmationText = `Are you sure that you want to delete user role "${userRole.name}"?`;
                    this.dialogUserRoleDeleteStatus.additionalText = count > 0 ? `Count of affected users: ${count}` : "";
                    this.dialogUserRoleDeleteStatus.id = userRole.id;
                    this.dialogUserRoleDeleteStatus.name = userRole.name;
                    this.dialogUserRoleDeleteStatus.visible = true;
                }).catch(error => {
                    button.loading = false;
                    this.$notify.error({
                        title: "Failed to get affected users count",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            deleteUserRole(button) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_USER_ROLE_ENDPOINT, {
                        id: this.dialogUserRoleDeleteStatus.id
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogUserRoleDeleteStatus.visible = false;
                    this.reload();

                    this.$notify.success({
                        title: "User role was deleted",
                        message: `User role "${this.dialogUserRoleDeleteStatus.name}" was removed`
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
            }
        }
    };
</script>
