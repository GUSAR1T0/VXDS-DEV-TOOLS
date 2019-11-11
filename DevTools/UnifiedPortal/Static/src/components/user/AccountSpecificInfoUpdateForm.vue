<template>
    <el-dialog :visible.sync="pageStatus.visible" width="75%" @closed="closed">
        <span slot="title" class="modal-title">Update Account Specific Info</span>
        <el-form :model="accountSpecificInfoUpdateForm" :rules="accountSpecificInfoUpdateRules"
                 ref="accountSpecificInfoUpdateForm"
                 label-width="120px" @submit.native.prevent="submitForm('accountSpecificInfoUpdateForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="userRole" label="User Role">
                        <el-row type="flex" justify="center">
                            <el-col :span="20">
                                <el-select v-model="accountSpecificInfoUpdateForm.userRole.id" placeholder="Select"
                                           :value="accountSpecificInfoUpdateForm.userRole.id" clearable
                                           style="width: 100%" :disabled="isAboutMe(getUserId)">
                                    <el-option v-for="item in userRoles" :key="item.value" :label="item.label"
                                               :value="item.value">
                                    </el-option>
                                </el-select>
                            </el-col>
                            <el-col :span="4">
                                <el-button type="info" plain circle @click="userRolesInfoDialogVisible = true">
                                    <fa icon="info-circle"/>
                                </el-button>
                            </el-col>
                        </el-row>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="userRole" label="User Status">
                        <el-select v-model="accountSpecificInfoUpdateForm.isActivated" :value="accountSpecificInfoUpdateForm.isActivated"
                                   placeholder="Select" style="width: 100%" :disabled="isAboutMe(getUserId)">
                            <el-option label="Activated" :value="true"></el-option>
                            <el-option label="Deactivated" :value="false"></el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-row type="flex" justify="center" align="middle" :gutter="20">
                            <el-col :span="12">
                                <el-button type="primary" plain @click="cancel" style="width: 100%">
                                    <strong>Cancel</strong>
                                </el-button>
                            </el-col>
                            <el-col :span="12">
                                <el-button type="danger" plain ref="accountSpecificInfoUpdateFormButton" native-type="submit"
                                           style="width: 100%">
                                    <strong>Submit</strong>
                                </el-button>
                            </el-col>
                        </el-row>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
        <el-dialog :visible.sync="userRolesInfoDialogVisible" width="75%" style="text-align: center" append-to-body>
            <span slot="title" class="modal-title">User Roles Description</span>
            <div style="margin-bottom: -50px"></div>
            <UserRolesPermissionsTables/>
        </el-dialog>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { LOCALHOST } from "@/constants/servers";
    import {
        GET_USER_ROLES_SHORT_INFO_ENDPOINT,
        UPDATE_PROFILE_ACCOUNT_SPECIFIC_INFO_ENDPOINT
    } from "@/constants/endpoints";
    import format from "string-format";
    import { GET_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";

    import UserRolesPermissionsTables from "@/components/userRole/UserRolesPermissionsTables";

    export default {
        name: "AccountSpecificInfoUpdateForm",
        props: {
            user: Object,
            accountSpecificInfoUpdateForm: Object,
            pageStatus: Object,
            closed: Function
        },
        components: {
            UserRolesPermissionsTables
        },
        data() {
            return {
                accountSpecificInfoUpdateRules: {},
                userRolesInfoDialogVisible: false,
                userRoles: []
            };
        },
        computed: {
            ...mapGetters([
                "isAboutMe",
                "getUserId"
            ]),
            isNotChanged() {
                return this.user.userRole.id === this.accountSpecificInfoUpdateForm.userRole.id &&
                    this.user.isActivated === this.accountSpecificInfoUpdateForm.isActivated;
            }
        },
        methods: {
            loadUserRoles() {
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLES_SHORT_INFO_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.userRoles = response.data.map(item => {
                        return {
                            value: item.id,
                            label: item.name
                        };
                    });
                }).catch(error => {
                    this.$notify.error({
                        title: "Failed to load user roles",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            cancel() {
                this.pageStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.accountSpecificInfoUpdateFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.accountSpecificInfoUpdateFormButton.loading = false;
                        return false;
                    }

                    if (!this.isNotChanged) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_PROFILE_ACCOUNT_SPECIFIC_INFO_ENDPOINT, {
                                id: this.user.id
                            }),
                            data: {
                                userRoleId: this.accountSpecificInfoUpdateForm.userRole.id,
                                isActivated: this.accountSpecificInfoUpdateForm.isActivated
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.pageStatus.visible = false;
                            this.$refs.accountSpecificInfoUpdateFormButton.loading = false;

                            this.$notify.success({
                                title: "Profile was updated",
                                message: "Your profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.accountSpecificInfoUpdateFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update profile",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    } else {
                        this.pageStatus.visible = false;
                        this.$refs.accountSpecificInfoUpdateFormButton.loading = false;

                        this.$notify.info({
                            title: "Profile wasn't updated",
                            message: "No change for update of your profile"
                        });
                    }
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