<template>
    <div>
        <el-table :data="users" style="width: 100%" border highlight-current-row>
            <el-table-column label="Operations" min-width="200" align="center" fixed>
                <template slot-scope="scope">
                    <el-tooltip class="item" effect="dark" content="Open User Profile" placement="top">
                        <el-button @click="$router.push(`/user/${scope.row.id}`)" circle type="primary"
                                   style="margin-right: 10px">
                            <span><fa icon="external-link-alt"/></span>
                        </el-button>
                    </el-tooltip>
                    <a :href="`mailto:${scope.row.email}`" style="margin-right: 10px">
                        <el-tooltip class="item" effect="dark" content="Send Email to User" placement="top">
                            <el-button circle type="primary">
                                <span><fa icon="envelope"/></span>
                            </el-button>
                        </el-tooltip>
                    </a>
                    <el-tooltip
                            v-if="hasPermissionToManageUserStatus(scope.row.id) && !scope.row.isActivated"
                            class="item" effect="dark" content="Activate User Account" placement="top">
                        <el-button
                                v-if="hasPermissionToManageUserStatus(scope.row.id) && !scope.row.isActivated"
                                @click="openManageUserStatusDialog(scope.row, true)" circle type="primary">
                            <span><fa icon="plus-circle"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip
                            v-if="hasPermissionToManageUserStatus(scope.row.id) && scope.row.isActivated"
                            class="item" effect="dark" content="Deactivate User Account" placement="top">
                        <el-button
                                v-if="hasPermissionToManageUserStatus(scope.row.id) && scope.row.isActivated"
                                @click="openManageUserStatusDialog(scope.row, false)" circle type="primary">
                            <span><fa icon="minus-circle"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
            </el-table-column>
            <el-table-column label="User ID" min-width="75" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                </template>
            </el-table-column>
            <el-table-column prop="type" label="User Name" min-width="300" align="left">
                <template slot-scope="scope">
                    <el-link :href="`/user/${scope.row.id}`" type="primary" :underline="false">
                        <UserAvatarAndFullName
                                :first-name="scope.row.firstName"
                                :last-name="scope.row.lastName"
                                :color="scope.row.color"
                        />
                    </el-link>
                </template>
            </el-table-column>
            <el-table-column label="Email Address" min-width="300" align="center">
                <template slot-scope="scope">
                    <el-link :href="`mailto:${scope.row.email}`" type="primary" :underline="false">
                        <strong style="font-size: 16px">{{ scope.row.email }}</strong>
                    </el-link>
                </template>
            </el-table-column>
            <el-table-column label="User Role" min-width="200" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.userRole }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="User Status" min-width="200" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.isActivated ? "Activated" : "Deactivated" }}</strong>
                </template>
            </el-table-column>
        </el-table>
        <ConfirmationDialog v-if="hasPermissionToManageUserStatus"
                            :dialog-status="userActivationConfirmDialog"
                            :confirmation-text="confirmationText(true)"
                            :cancel-click-action="() => cancelManageUserStatusDialog(true)"
                            :submit-click-action="button => manageUserStatus(button, true)"
                            :closed="submitUserStatus"/>
        <ConfirmationDialog v-if="hasPermissionToManageUserStatus"
                            :dialog-status="userDeactivationConfirmDialog"
                            :confirmation-text="confirmationText(false)"
                            :cancel-click-action="() => cancelManageUserStatusDialog(false)"
                            :submit-click-action="button => manageUserStatus(button, false)"
                            :closed="submitUserStatus"/>
    </div>
</template>

<script>
    import { mapGetters } from "vuex";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { ACTIVATE_USER_ENDPOINT, DEACTIVATE_USER_ENDPOINT } from "@/constants/endpoints";

    import ConfirmationDialog from "@/components/page/ConfirmationDialog";
    import UserAvatarAndFullName from "@/components/user/UserAvatarAndFullName";

    export default {
        name: "UsersTable",
        props: {
            users: Array,
            reload: Function
        },
        components: {
            ConfirmationDialog,
            UserAvatarAndFullName
        },
        computed: {
            ...mapGetters([
                "getUserId",
                "hasPortalPermission"
            ])
        },
        data() {
            return {
                userActivationConfirmDialogForUser: null,
                userDeactivationConfirmDialogForUser: null,
                userActivationConfirmDialog: {
                    visible: false
                },
                userDeactivationConfirmDialog: {
                    visible: false
                }
            };
        },
        methods: {
            hasPermissionToManageUserStatus(id) {
                return this.getUserId !== id && this.hasPortalPermission(PORTAL_PERMISSION.UPDATE_USER_PROFILE);
            },
            openManageUserStatusDialog(user, activate) {
                if (activate) {
                    this.userActivationConfirmDialogForUser = user;
                    this.userActivationConfirmDialog.visible = true;
                } else {
                    this.userDeactivationConfirmDialogForUser = user;
                    this.userDeactivationConfirmDialog.visible = true;
                }
            },
            cancelManageUserStatusDialog(activate) {
                if (activate) {
                    this.userActivationConfirmDialog.visible = false;
                } else {
                    this.userDeactivationConfirmDialog.visible = false;
                }
            },
            confirmationText(activate) {
                let user = activate ? this.userActivationConfirmDialogForUser : this.userDeactivationConfirmDialogForUser;
                return `Are you sure that you want to ${activate ? "" : "de"}activate account${user ? " of " + user.userName : ""}?`;
            },
            manageUserStatus(button, activate) {
                button.loading = true;
                if (activate) {
                    this.$store.dispatch(PUT_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(ACTIVATE_USER_ENDPOINT, {
                            id: this.userActivationConfirmDialogForUser.id
                        }),
                        config: getConfiguration()
                    }).then(() => {
                        button.loading = false;
                        let user = this.userActivationConfirmDialogForUser;
                        this.userActivationConfirmDialog.visible = false;

                        this.$notify.success({
                            title: "Profile was updated",
                            message: `Account ${user.userName} (ID: ${user.id}) was activated`
                        });
                    }).catch(error => {
                        button.loading = false;
                        this.userActivationConfirmDialog.visible = false;

                        this.$notify.error({
                            title: "Failed to activate user",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.$store.dispatch(PUT_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(DEACTIVATE_USER_ENDPOINT, {
                            id: this.userDeactivationConfirmDialogForUser.id
                        }),
                        config: getConfiguration()
                    }).then(() => {
                        button.loading = false;
                        let user = this.userDeactivationConfirmDialogForUser;
                        this.userDeactivationConfirmDialog.visible = false;

                        this.$notify.success({
                            title: "Profile was updated",
                            message: `Account ${user.userName} (ID: ${user.id}) was deactivated`
                        });

                        this.reload();
                    }).catch(error => {
                        button.loading = false;
                        this.userDeactivationConfirmDialog.visible = false;

                        this.$notify.error({
                            title: "Failed to deactivate user",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                }
            },
            submitUserStatus() {
                this.userDeactivationConfirmDialogForUser = null;
                this.reload();
            }
        }
    };
</script>