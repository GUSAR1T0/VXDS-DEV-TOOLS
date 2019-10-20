<template>
    <el-dialog :visible.sync="pageStatus.visible" width="75%" @closed="closed">
        <span slot="title" class="modal-title">Update Account Specific Info</span>
        <el-form :model="accountSpecificInfoUpdateForm" :rules="accountSpecificInfoUpdateRules"
                 ref="accountSpecificInfoUpdateForm"
                 label-width="120px" @submit.native.prevent="submitForm('accountSpecificInfoUpdateForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="userRole" label="User Role">
                        <el-select v-model="accountSpecificInfoUpdateForm.userRole.id" placeholder="Select"
                                   :value="accountSpecificInfoUpdateForm.userRole.id" clearable style="width: 100%"
                                   :disabled="isAboutMe(getUserId)">
                            <el-option v-for="item in userRoles" :key="item.value" :label="item.label"
                                       :value="item.value">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-button type="danger" @click="cancel" plain>Cancel</el-button>
                        <el-button type="danger" ref="accountSpecificInfoUpdateFormButton" native-type="submit">Submit
                        </el-button>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import HttpClient from "@/extensions/httpClient";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_USER_ROLES_ENDPOINT, UPDATE_PROFILE_ACCOUNT_SPECIFIC_INFO_ENDPOINT } from "@/constants/endpoints";
    import format from "string-format";

    export default {
        name: "AccountSpecificInfoUpdateForm",
        props: {
            user: Object,
            accountSpecificInfoUpdateForm: Object,
            pageStatus: Object,
            closed: Function
        },
        data() {
            return {
                accountSpecificInfoUpdateRules: {},
                userRoles: []
            };
        },
        computed: {
            ...mapGetters([
                "isAboutMe",
                "getUserId"
            ]),
            isNotChanged() {
                return this.user.userRole.id === this.accountSpecificInfoUpdateForm.userRole.id;
            }
        },
        methods: {
            loadUserRoles() {
                HttpClient.init().get(LOCALHOST, GET_USER_ROLES_ENDPOINT, getConfiguration()).then(response => {
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
                        let endpoint = format(UPDATE_PROFILE_ACCOUNT_SPECIFIC_INFO_ENDPOINT, {id: this.user.id});
                        HttpClient.init().put(LOCALHOST, endpoint, {
                            userRoleId: this.accountSpecificInfoUpdateForm.userRole.id
                        }, getConfiguration()).then(() => {
                            this.pageStatus.visible = false;
                            this.$refs.accountSpecificInfoUpdateFormButton.loading = false;

                            this.$notify.info({
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

                        this.$notify.warning({
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