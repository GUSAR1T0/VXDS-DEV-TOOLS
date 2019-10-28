<template>
    <el-dialog :visible.sync="pageStatus.visible" width="75%" @closed="closed">
        <span slot="title" class="modal-title">Update User General Info</span>
        <UserCard :user="userGeneralInfoUpdateForm" style="padding-bottom: 25px"/>
        <el-form :model="userGeneralInfoUpdateForm" :rules="userGeneralInfoUpdateRules" ref="userGeneralInfoUpdateForm"
                 label-width="120px"
                 @submit.native.prevent="submitForm('userGeneralInfoUpdateForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="firstName" label="First Name">
                        <el-input v-model="userGeneralInfoUpdateForm.firstName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="lastName" label="Last Name">
                        <el-input v-model="userGeneralInfoUpdateForm.lastName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="email" label="Email Address">
                        <el-input v-model="userGeneralInfoUpdateForm.email" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="location" label="Location">
                        <el-input v-model="userGeneralInfoUpdateForm.location" clearable maxlength="255"
                                  show-word-limit></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="bio" label="Bio">
                        <el-input type="textarea" :rows="2" v-model="userGeneralInfoUpdateForm.bio" clearable
                                  maxlength="1000" show-word-limit></el-input>
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
                        <el-row type="flex" justify="center" align="middle" :gutter="20">
                            <el-col :span="12">
                                <el-button type="danger" @click="cancel" plain style="width: 100%">
                                    Cancel
                                </el-button>
                            </el-col>
                            <el-col :span="12">
                                <el-button type="danger" ref="userGeneralInfoUpdateFormButton" native-type="submit" style="width: 100%">
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
    import { generateColor, getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { LOCALHOST } from "@/constants/servers";
    import { UPDATE_PROFILE_GENERAL_INFO_ENDPOINT } from "@/constants/endpoints";
    import { PUT_HTTP_REQUEST } from "@/constants/actions";
    import format from "string-format";

    import UserCard from "@/components/user/UserCard.vue";

    export default {
        name: "UserGeneralInfoUpdateForm",
        props: {
            user: Object,
            userGeneralInfoUpdateForm: Object,
            pageStatus: Boolean,
            closed: Function
        },
        components: {
            UserCard
        },
        data() {
            return {
                userGeneralInfoUpdateRules: {
                    firstName: [
                        {required: true, message: "Please, input first name", trigger: "change"},
                        {min: 2, max: 30, message: "Length should be from 2 to 30", trigger: "change"}
                    ],
                    lastName: [
                        {required: true, message: "Please, input last name", trigger: "change"},
                        {min: 2, max: 30, message: "Length should be from 2 to 30", trigger: "change"}
                    ],
                    email: [
                        {required: true, message: "Please, input email address", trigger: "change"},
                        {type: "email", message: "Please, input correct email address", trigger: "change"},
                        {min: 3, max: 254, message: "Length should be from 3 to 254", trigger: "change"}
                    ]
                }
            };
        },
        computed: {
            isNotChanged() {
                return this.user.firstName === this.userGeneralInfoUpdateForm.firstName &&
                    this.user.lastName === this.userGeneralInfoUpdateForm.lastName &&
                    this.user.email === this.userGeneralInfoUpdateForm.email &&
                    this.user.color === this.userGeneralInfoUpdateForm.color &&
                    this.user.location === this.userGeneralInfoUpdateForm.location &&
                    this.user.bio === this.userGeneralInfoUpdateForm.bio;
            }
        },
        methods: {
            generateColor() {
                this.userGeneralInfoUpdateForm.color = generateColor();
            },
            cancel() {
                this.pageStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.userGeneralInfoUpdateFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.userGeneralInfoUpdateFormButton.loading = false;
                        return false;
                    }

                    if (!this.isNotChanged) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_PROFILE_GENERAL_INFO_ENDPOINT, {
                                id: this.user.id
                            }),
                            data: {
                                firstName: this.userGeneralInfoUpdateForm.firstName,
                                lastName: this.userGeneralInfoUpdateForm.lastName,
                                email: this.userGeneralInfoUpdateForm.email,
                                color: this.userGeneralInfoUpdateForm.color,
                                location: this.userGeneralInfoUpdateForm.location,
                                bio: this.userGeneralInfoUpdateForm.bio
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.pageStatus.visible = false;
                            this.$refs.userGeneralInfoUpdateFormButton.loading = false;

                            this.$notify.info({
                                title: "Profile was updated",
                                message: "Your profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.userGeneralInfoUpdateFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update profile",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    } else {
                        this.pageStatus.visible = false;
                        this.$refs.userGeneralInfoUpdateFormButton.loading = false;

                        this.$notify.warning({
                            title: "Profile wasn't updated",
                            message: "No change for update of your profile"
                        });
                    }
                });
            }
        }
    };
</script>