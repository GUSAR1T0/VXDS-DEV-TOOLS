<template>
    <el-dialog title="Update form" :visible.sync="pageStatus.userUpdateFormDialogVisible" width="75%" @closed="closed">
        <UserCard :user="userUpdateForm" style="padding-bottom: 25px"/>
        <el-form :model="userUpdateForm" :rules="userUpdateRules" ref="userUpdateForm" label-width="120px"
                 @submit.native.prevent="submitForm('userUpdateForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="firstName" label="First Name">
                        <el-input v-model="userUpdateForm.firstName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="lastName" label="Last Name">
                        <el-input v-model="userUpdateForm.lastName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="email" label="Email Address">
                        <el-input v-model="userUpdateForm.email" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="location" label="Location">
                        <el-input v-model="userUpdateForm.location" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="bio" label="Bio">
                        <el-input type="textarea" autosize v-model="userUpdateForm.bio" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="avatar" label="Color">
                        <el-button ref="colorButton" style="width: 100%" @click="generateColor">
                            Generate new color
                        </el-button>
                    </el-form-item>
                </el-col>
            </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item>
                            <el-button type="danger" @click="cancel" plain>Cancel</el-button>
                            <el-button type="danger" ref="updateFormButton" native-type="submit">Submit</el-button>
                        </el-form-item>
                    </el-col>
                </el-row>
        </el-form>
    </el-dialog>
</template>

<script>
    import { generateColor, getConfiguration } from "@/extensions/utils";
    import HttpClient from "@/extensions/httpClient";
    import { LOCALHOST } from "@/constants/servers";
    import { UPDATE_PROFILE_ENDPOINT } from "@/constants/endpoints";

    import UserCard from "@/components/user/UserCard.vue";

    export default {
        name: "UserUpdateForm",
        props: {
            user: Object,
            userUpdateForm: Object,
            pageStatus: Object,
            closed: Function
        },
        components: {
            UserCard
        },
        data() {
            return {
                userUpdateRules: {
                    firstName: [
                        {required: true, message: "Please, input first name", trigger: "change"},
                        {min: 2, max: 30, message: "Length should be 2 to 30", trigger: "change"}
                    ],
                    lastName: [
                        {required: true, message: "Please, input last name", trigger: "change"},
                        {min: 2, max: 30, message: "Length should be 2 to 30", trigger: "change"}
                    ],
                    email: [
                        {required: true, message: "Please, input email address", trigger: "change"},
                        {type: "email", message: "Please, input correct email address", trigger: "change"}
                    ],
                }
            };
        },
        computed: {
            isNotChanged() {
                return this.user.firstName === this.userUpdateForm.firstName &&
                    this.user.lastName === this.userUpdateForm.lastName &&
                    this.user.email === this.userUpdateForm.email &&
                    this.user.color === this.userUpdateForm.color &&
                    this.user.location === this.userUpdateForm.location &&
                    this.user.bio === this.userUpdateForm.bio;
            }
        },
        methods: {
            generateColor() {
                this.userUpdateForm.color = generateColor();
            },
            cancel() {
                this.pageStatus.userUpdateFormDialogVisible = false;
            },
            submitForm(formName) {
                this.$refs.updateFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.updateFormButton.loading = false;
                        return false;
                    }

                    if (!this.isNotChanged) {
                        HttpClient.init().then(client => {
                            client.put(LOCALHOST, UPDATE_PROFILE_ENDPOINT, this.userUpdateForm, getConfiguration()).then(() => {
                                this.pageStatus.userUpdateFormDialogVisible = false;
                                this.$refs.updateFormButton.loading = false;

                                this.$notify.info({
                                    title: "Info",
                                    message: "Your profile was updated successfully"
                                });
                            }).catch(error => {
                                this.$refs.updateFormButton.loading = false;

                                this.$notify.error({
                                    title: "Error",
                                    message: `Your profile was not updated: ${error.response.data.message}`
                                });
                            });
                        }).catch(() => {
                        });
                    } else {
                        this.pageStatus.userUpdateFormDialogVisible = false;
                        this.$refs.updateFormButton.loading = false;

                        this.$notify.warning({
                            title: "Warning",
                            message: "Your profile was not updated due to lack of changes"
                        });
                    }
                });
            }
        }
    };
</script>