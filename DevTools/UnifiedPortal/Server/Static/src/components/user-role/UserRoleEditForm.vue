<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">{{ userRoleForm.id ? "Update" : "Create" }} User Role</span>
        <el-form :model="userRoleForm" :rules="userRoleRules" ref="userRoleForm"
                 label-width="200px" @submit.native.prevent="submitForm('userRoleForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="name" label="Role Name">
                        <el-input v-model="userRoleForm.name" clearable/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center"
                    v-for="permissionGroup in userRolePermissions" :key="permissionGroup.id">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item :prop="`permissions-${permissionGroup.id}`" :label="permissionGroup.name">
                        <el-select v-model="permissionsForUserRole[permissionGroup.id]" multiple
                                   clearable placeholder="Select" style="width: 100%">
                            <el-option v-for="item in permissionGroup.permissions" :key="item.id"
                                       :label="item.name" :value="item.id"/>
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
                                <el-button type="primary" ref="userRoleFormButton" native-type="submit"
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
    import { LOCALHOST } from "@/constants/servers";
    import { POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { ADD_USER_ROLE_ENDPOINT, UPDATE_USER_ROLE_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    export default {
        name: "UserRoleEditForm",
        props: {
            userRolePermissions: Array,
            permissionsForUserRole: Object,
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

                    let permissions = [];
                    for (let key in this.permissionsForUserRole) {
                        permissions.push({
                            permissionGroupId: parseInt(key),
                            permissions: this.permissionsForUserRole[key]
                        });
                    }

                    if (this.userRoleForm.id) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_USER_ROLE_ENDPOINT, {
                                id: this.userRoleForm.id
                            }),
                            data: {
                                name: this.userRoleForm.name,
                                permissions: permissions
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
                                permissions: permissions
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
                });
            }
        }
    };
</script>