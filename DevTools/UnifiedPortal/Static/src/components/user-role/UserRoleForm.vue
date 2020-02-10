<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" @closed="closed">
        <span slot="title" class="modal-title">{{ userRoleForm.id ? "Update" : "Create" }} User Role</span>
        <el-form :model="userRoleForm" :rules="userRoleRules" ref="userRoleForm"
                 label-width="200px"
                 @submit.native.prevent="submitForm('userRoleForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="name" label="Role Name">
                        <el-input v-model="userRoleForm.name" clearable/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="portalPermissions" label="Portal Permissions">
                        <el-select v-model="userRoleForm.portalPermissions" :value="userRoleForm.portalPermissions" multiple
                                   clearable placeholder="Select" style="width: 100%">
                            <el-option v-for="item in getLookupValues('portalPermissions')" :key="item.value"
                                       :label="item.name" :value="item.value">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-row type="flex" justify="center" align="middle" :gutter="20">
                            <el-col :span="12">
                                <el-button type="primary" @click="cancel" plain style="width: 100%">
                                    Cancel
                                </el-button>
                            </el-col>
                            <el-col :span="12">
                                <el-button type="primary" ref="userRoleFormButton" native-type="submit"
                                           style="width: 100%">
                                    Submit
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
    import { mapGetters } from "vuex";
    import { LOCALHOST } from "@/constants/servers";
    import { POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { ADD_USER_ROLE_ENDPOINT, UPDATE_USER_ROLE_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    export default {
        name: "UserRoleForm",
        props: {
            userRole: Object,
            userRoleForm: Object,
            dialogStatus: Object,
            closed: Function
        },
        data() {
            return {
                userRoleRules: {
                    name: [
                        {required: true, message: "Please, input role name", trigger: "change"},
                        {min: 1, max: 50, message: "Length should be from 1 to 50", trigger: "change"}
                    ]
                }
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ]),
            isNotChanged() {
                return this.userRole.name === this.userRoleForm.name &&
                    this.userRole.portalPermissions === this.userRoleForm.portalPermissions.map(Number).reduce((a, b) => a + b, 0);
            }
        },
        methods: {
            cancel() {
                this.dialogStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.userRoleFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.userRoleFormButton.loading = false;
                        return false;
                    }

                    if (!this.isNotChanged) {
                        if (this.userRoleForm.id) {
                            this.$store.dispatch(PUT_HTTP_REQUEST, {
                                server: LOCALHOST,
                                endpoint: format(UPDATE_USER_ROLE_ENDPOINT, {
                                    id: this.userRoleForm.id
                                }),
                                data: {
                                    name: this.userRoleForm.name,
                                    portalPermissions: this.userRoleForm.portalPermissions.map(Number).reduce((a, b) => a + b, 0)
                                },
                                config: getConfiguration()
                            }).then(() => {
                                this.dialogStatus.visible = false;
                                this.$refs.userRoleFormButton.loading = false;

                                this.$notify.success({
                                    title: "User role was updated",
                                    message: "Role changes took effect"
                                });
                            }).catch(error => {
                                this.$refs.userRoleFormButton.loading = false;

                                this.$notify.error({
                                    title: "Failed to update user role",
                                    duration: 10000,
                                    message: renderErrorNotificationMessage(this.$createElement, error.response)
                                });
                            });
                        } else {
                            this.$store.dispatch(POST_HTTP_REQUEST, {
                                server: LOCALHOST,
                                endpoint: ADD_USER_ROLE_ENDPOINT,
                                data: {
                                    name: this.userRoleForm.name,
                                    portalPermissions: this.userRoleForm.portalPermissions.map(Number).reduce((a, b) => a + b, 0)
                                },
                                config: getConfiguration()
                            }).then(() => {
                                this.dialogStatus.visible = false;
                                this.$refs.userRoleFormButton.loading = false;

                                this.$notify.success({
                                    title: "User role was created",
                                    message: "Role changes took effect"
                                });
                            }).catch(error => {
                                this.$refs.userRoleFormButton.loading = false;

                                this.$notify.error({
                                    title: "Failed to create user role",
                                    duration: 10000,
                                    message: renderErrorNotificationMessage(this.$createElement, error.response)
                                });
                            });
                        }
                    } else {
                        this.dialogStatus.visible = false;
                        this.$refs.userRoleFormButton.loading = false;

                        this.$notify.warning({
                            title: "Role wasn't " + (this.userRoleForm.id ? "updated" : "created"),
                            message: "Nothing for " + (this.userRoleForm.id ? "update" : "create")
                        });
                    }
                });
            }
        }
    };
</script>