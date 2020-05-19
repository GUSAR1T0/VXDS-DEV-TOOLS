<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @open="prepareModal" @closed="closed">
        <span slot="title" class="modal-title">Note Notification Send Menu</span>
        <el-form :model="notificationSendForm" :rules="notificationSendRules" ref="notificationSendForm"
                 label-width="120px" @submit.native.prevent="submitForm('notificationSendForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="userIds" label="Users">
                        <el-select v-model="notificationSendForm.userIds" filterable remote reserve-keyword
                                   :remote-method="filterByUserIds" :loading="userIdsSearchLoading"
                                   style="width: 100%" clearable>
                            <el-option v-for="item in userIdOptions" :key="item.id"
                                       :label="item.fullName" :value="item.id"/>
                        </el-select>
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
                                <el-button type="primary" ref="notificationSendFormButton" native-type="submit"
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

<script>
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST, UNIFIED_PORTAL } from "@/constants/servers";
    import { SEARCH_USERS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { SEND_NOTIFICATION_ABOUT_NOTE_ENDPOINT } from "../../constants/endpoints";
    import format from "string-format";

    export default {
        name: "NoteNotificationDialog",
        props: {
            dialogStatus: Object,
            folderId: Number,
            noteId: Number,
            closed: Function
        },
        data() {
            return {
                notificationSendForm: {
                    userIds: []
                },
                notificationSendRules: {
                    userIds: [
                        {required: true, message: "Please, choose users", trigger: "change"}
                    ]
                },

                userIdsSearchLoading: false,
                userIdOptions: []
            };
        },
        methods: {
            prepareModal() {
                this.notificationSendForm.userIds = [];
            },
            filterByUserIds(query) {
                if (query !== "") {
                    this.userIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: UNIFIED_PORTAL,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.userIdsSearchLoading = false;
                        this.userIdOptions = response.data;
                    }).catch(error => {
                        this.userIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of users",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.userIdOptions = [];
                }
            },
            cancel() {
                this.dialogStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.notificationSendFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.notificationSendFormButton.loading = false;
                        return false;
                    }

                    this.$store.dispatch(POST_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEND_NOTIFICATION_ABOUT_NOTE_ENDPOINT, {
                            folderId: this.folderId,
                            noteId: this.noteId,
                            userIds: this.notificationSendForm.userIds.map(item => `u=${item}`).join("&")
                        }),
                        config: getConfiguration()
                    }).then(() => {
                        this.dialogStatus.visible = false;
                        this.$refs.notificationSendFormButton.loading = false;

                        this.$notify.success({
                            title: "Notification about this note is prepared",
                            message: "All chosen users will get notification by email"
                        });
                    }).catch(error => {
                        this.$refs.notificationSendFormButton.loading = false;

                        this.$notify.error({
                            title: "Failed to send notification about note",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                });
            }
        }
    };
</script>