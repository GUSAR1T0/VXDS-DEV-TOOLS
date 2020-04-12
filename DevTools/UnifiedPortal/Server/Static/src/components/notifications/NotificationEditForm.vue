<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">{{ notification.id ? "Update" : "Create" }} Notification</span>
        <el-form :model="notificationForm" :rules="notificationRules" ref="notificationForm"
                 label-width="200px" @submit.native.prevent="submitForm('notificationForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="message" label="Message">
                        <el-input v-model="notificationForm.message" clearable type="textarea" maxlength="1024"
                                  show-word-limit/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="level" label="Level">
                        <el-select v-model="notificationForm.level" filterable reserve-keyword
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in getLookupValues('notificationLevels')" :key="item.value"
                                       :label="item.name" :value="item.value">
                                <div style="display: flex;">
                                    <div style="font-size: 14px; margin-right: 5px">
                                        <fa icon="circle" :class="getLevelColor(item.value)"/>
                                    </div>
                                    {{ item.name }}
                                </div>
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="startDateTime" label="Start Time">
                        <el-date-picker style="width: 100%"
                                        v-model="notificationForm.startDateTime"
                                        type="datetime"
                                        range-separator="—"/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="stopDateTime" label="Stop Time">
                        <el-date-picker style="width: 100%"
                                        v-model="notificationForm.stopDateTime"
                                        type="datetime"
                                        range-separator="—"/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-row type="flex" justify="center" align="middle" :gutter="20">
                            <el-col :span="12">
                                <el-button @click="cancel" style="width: 100%">
                                    <strong>Cancel</strong>
                                </el-button>
                            </el-col>
                            <el-col :span="12">
                                <el-button type="primary" ref="notificationFormButton" native-type="submit"
                                           style="width: 100%">
                                    <strong>Submit</strong>
                                </el-button>
                            </el-col>
                        </el-row>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<style scoped src="@/styles/status.css">
</style>

<script>
    import { PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { MODIFY_NOTIFICATION_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { mapGetters } from "vuex";

    export default {
        name: "NotificationEditForm",
        props: {
            notification: Object,
            notificationForm: Object,
            dialogStatus: Object,
            closed: Function
        },
        data() {
            return {
                notificationRules: {
                    message: [
                        {required: true, message: "Please, input notification message", trigger: "change"}
                    ],
                    level: [
                        {required: true, message: "Please, choose notification level", trigger: "change"}
                    ],
                    startDateTime: [
                        {required: true, message: "Please, choose notification start time", trigger: "change"}
                    ],
                    stopDateTime: [
                        {required: true, message: "Please, choose notification stop time", trigger: "change"}
                    ]
                }
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            getLevelColor(levelId) {
                levelId = parseInt(levelId);
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
            cancel() {
                this.dialogStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.notificationFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.notificationFormButton.loading = false;
                        return false;
                    }

                    this.$store.dispatch(PUT_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: MODIFY_NOTIFICATION_LIST_ENDPOINT,
                        data: {
                            id: this.notificationForm.id,
                            message: this.notificationForm.message,
                            level: parseInt(this.notificationForm.level),
                            startTime: this.notificationForm.startDateTime,
                            stopTime: this.notificationForm.stopDateTime
                        },
                        config: getConfiguration()
                    }).then(() => {
                        this.dialogStatus.visible = false;
                        this.$refs.notificationFormButton.loading = false;

                        this.$notify.success({
                            title: `Notification was ${this.notificationForm.id ? "updated" : " created"}`,
                            message: "Profile changes took effect"
                        });
                    }).catch(error => {
                        this.$refs.notificationFormButton.loading = false;

                        this.$notify.error({
                            title: `Failed to ${this.notificationForm.id ? "update" : " create"} notification`,
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                });
            }
        }
    };
</script>