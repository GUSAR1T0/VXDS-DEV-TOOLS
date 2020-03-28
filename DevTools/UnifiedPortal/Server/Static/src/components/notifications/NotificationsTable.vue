<template>
    <div>
        <el-table :data="notifications" style="width: 100%" border>
            <el-table-column label="Notification ID" min-width="150" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="Notification Level" min-width="350" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 18px">
                        {{ getLevelName(scope.row.level) }}
                    </strong>
                </template>
            </el-table-column>
            <el-table-column label="Message" min-width="900" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 20px">
                        {{ scope.row.message }}
                    </strong>
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
    </div>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    export default {
        name: "NotificationsTable",
        props: {
            notifications: Array
        },
        data() {
            return {
                dialogNotificationFormStatus: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getLookupValues"
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
            // openDialogToModify() {
                // this.$store.commit(STORE_INCIDENT_DATA_REQUEST, {
                //     operation: operation,
                //     exists: !!operation.incident,
                //     authorId: this.getUserId,
                //     authorFirstName: this.getFullName,
                //     status: "1"
                // });
                // this.$store.commit(PREPARE_INCIDENT_FORM);
                // this.dialogIncidentFormStatus.visible = true;
            // },
            // submitNotificationAction() {
                // this.$store.commit(RESET_INCIDENT_STORE_STATE);
            // }
        }
    };
</script>