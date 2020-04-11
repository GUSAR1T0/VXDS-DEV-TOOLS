<template>
    <div>
        <el-table :data="notifications" style="width: 100%" border>
            <el-table-column label="Notification ID" min-width="150" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="Manage Notification" min-width="200" align="center"
                             v-if="hasPermissionToManageNotifications">
                <template slot-scope="scope">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Edit This Notification
                        </div>
                        <el-button type="primary" plain circle @click="openDialogToUpdate(scope.row)"
                                   class="rounded-button">
                            <span><fa icon="edit"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Delete This Notification
                        </div>
                        <el-button type="danger" plain circle @click="openDeleteNotificationDialog(scope.row)"
                                   class="rounded-button" :ref="`deleteNotificationButton-${scope.row.id}`">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
            </el-table-column>
            <el-table-column label="Notification Level" min-width="350" align="center">
                <template slot-scope="scope">
                    <div style="display: flex; justify-content: center">
                        <div style="font-size: 14px; margin-right: 5px">
                            <fa icon="circle" :class="getLevelColor(scope.row.level)"/>
                        </div>
                        <strong style="font-size: 18px">
                            {{ getLevelName(scope.row.level) }}
                        </strong>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Message" min-width="900" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 20px">
                        {{ scope.row.message }}
                    </strong>
                </template>
            </el-table-column>
            <el-table-column label="Created by User" min-width="300" align="center">
                <template slot-scope="scope">
                    <UserAvatarAndFullNameWithLink
                            style="text-align: left"
                            :first-name="scope.row.firstName"
                            :last-name="scope.row.lastName"
                            :color="scope.row.color"
                            :user-id="scope.row.userId"
                    />
                </template>
            </el-table-column>
            <el-table-column label="Notification Date / Time" min-width="400" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        <strong>Start Time</strong>: {{ scope.row.startTime }}
                    </div>
                    <div style="font-size: 16px">
                        <strong>Stop Time</strong>: {{ scope.row.stopTime ? scope.row.stopTime : "—" }}
                    </div>
                </template>
            </el-table-column>
        </el-table>

        <NotificationEditForm
                :dialog-status="dialogNotificationFormStatus"
                :notification="getNotification"
                :notification-form="getNotificationForm"
                :closed="submitNotificationAction"/>
        <ConfirmationDialog
                :dialog-status="dialogNotificationDeleteStatus"
                :confirmation-text="dialogNotificationDeleteStatus.confirmationText"
                :cancel-click-action="() => dialogNotificationDeleteStatus.visible = false"
                :submit-click-action="deleteNotification"
                :closed="submitNotificationAction"/>
    </div>
</template>

<style scoped src="@/styles/button.css">
</style>

<style scoped src="@/styles/status.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { LOCALHOST } from "@/constants/servers";
    import {
        DELETE_HTTP_REQUEST,
        PREPARE_NOTIFICATION_FORM,
        RESET_NOTIFICATION_STORE_STATE,
        STORE_NOTIFICATION_DATA_REQUEST
    } from "@/constants/actions";
    import { DELETE_NOTIFICATION_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import NotificationEditForm from "@/components/notifications/NotificationEditForm";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";

    export default {
        name: "NotificationsTable",
        props: {
            notifications: Array,
            reload: Function
        },
        components: {
            NotificationEditForm,
            ConfirmationDialog,
            UserAvatarAndFullNameWithLink
        },
        data() {
            return {
                dialogNotificationFormStatus: {
                    visible: false
                },
                dialogNotificationDeleteStatus: {
                    id: null,
                    confirmationText: "",
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getLookupValues",
                "getNotification",
                "getNotificationForm"
            ]),
            hasPermissionToManageNotifications() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_NOTIFICATIONS);
            }
        },
        methods: {
            getLevelName(levelId) {
                let levels = this.getLookupValues("notificationLevels").filter(level => parseInt(level.value) === levelId);
                return levels && levels.length > 0 ? levels[0].name : "—";
            },
            getLevelColor(levelId) {
                if (levelId === 1) {
                    return "info";
                } else if (levelId === 2) {
                    return "success";
                } else if (levelId === 3) {
                    return "warning";
                } else if (levelId === 4) {
                    return "error";
                }
            },
            openDialogToUpdate(notification) {
                this.$store.commit(STORE_NOTIFICATION_DATA_REQUEST, notification);
                this.$store.commit(PREPARE_NOTIFICATION_FORM);
                this.dialogNotificationFormStatus.visible = true;
            },
            submitNotificationAction() {
                this.reload();
                this.$store.commit(RESET_NOTIFICATION_STORE_STATE);
            },
            openDeleteNotificationDialog(notification) {
                this.dialogNotificationDeleteStatus.id = notification.id;
                this.dialogNotificationDeleteStatus.confirmationText = `Are you sure that you want to delete notification with ID "${notification.id}"`;
                this.dialogNotificationDeleteStatus.visible = true;
            },
            deleteNotification(button) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_NOTIFICATION_LIST_ENDPOINT, {
                        id: this.dialogNotificationDeleteStatus.id
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogNotificationDeleteStatus.visible = false;
                    this.reload();

                    this.$notify.success({
                        title: "Notification was deleted",
                        message: `Notification with ID "${this.dialogNotificationDeleteStatus.id}" was removed`
                    });
                    this.dialogNotificationDeleteStatus.id = null;
                    this.dialogNotificationDeleteStatus.confirmationText = "";
                }).catch(error => {
                    button.loading = false;
                    this.dialogNotificationDeleteStatus.visible = false;

                    this.$notify.error({
                        title: "Failed to delete notification",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>