<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <el-button v-if="isEditable && hasPermissionToManageUserRoles" type="primary" plain
                       class="user-roles-button" @click="openDialogToCreateOrUpdate">
                <span><fa icon="plus-circle"/> | Create User Role</span>
            </el-button>
            <el-card shadow="hover" style="margin-top: 25px;" v-for="item in userRoles" :key="item.id">
                <div slot="header" style="text-align: center">
                    <h3>{{ item.name }}</h3>
                </div>
                <UserRolePermissionsTable :user-role="item"/>
                <el-row v-if="isEditable && hasPermissionToManageUserRoles(item.id)" style="margin-top: 25px;"
                        type="flex" justify="center" align="middle" :gutter="20">
                    <el-col :span="12">
                        <el-button type="primary" plain class="user-roles-button"
                                   @click="openDialogToCreateOrUpdate(item)">
                            <span><fa icon="edit"/> | Edit User Role</span>
                        </el-button>
                    </el-col>
                    <el-col :span="12">
                        <el-button type="primary" plain class="user-roles-button"
                                   @click="openDeleteUserRoleDialog(item)" ref="deleteUserRoleButton">
                            <span><fa icon="minus-circle"/> | Delete User Role</span>
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
        GET_USER_ROLES_FULL_INFO_ENDPOINT
    } from "@/constants/endpoints";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserRolePermissionsTable from "@/components/userRole/UserRolePermissionsTable";
    import UserRoleForm from "@/components/userRole/UserRoleForm";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";

    export default {
        name: "UserRolesPermissionsTables",
        props: {
            isEditable: Boolean
        },
        components: {
            LoadingContainer,
            UserRolePermissionsTable,
            UserRoleForm,
            ConfirmationDialog
        },
        data() {
            return {
                loadingIsActive: true,
                userRoles: [],
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
                "getUserRole",
                "getUserRoleForm"
            ])
        },
        methods: {
            loadUserRoles() {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLES_FULL_INFO_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.userRoles = response.data;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of user roles",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            hasPermissionToManageUserRoles(id) {
                return (id && id !== 1 || !id) && this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_USER_ROLES);
            },
            openDialogToCreateOrUpdate(userRole) {
                this.$store.commit(STORE_USER_ROLE_DATA_REQUEST, userRole);
                this.$store.commit(PREPARE_USER_ROLE_FORM);
                this.dialogUserRoleFormStatus.visible = true;
            },
            submitUserRoleAction() {
                this.loadUserRoles();
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
