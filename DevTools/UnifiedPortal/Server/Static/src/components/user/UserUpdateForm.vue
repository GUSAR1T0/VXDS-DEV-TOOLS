<template>
    <el-dialog :visible.sync="pageStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">Update User Profile</span>
        <UserCard :user="userUpdateForm" style="padding-bottom: 25px"/>
        <el-form :model="userUpdateForm" :rules="userUpdateRules" ref="userUpdateForm"
                 label-width="120px" @submit.native.prevent="submitForm('userUpdateForm')">
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
                        <el-input v-model="userUpdateForm.location" clearable maxlength="255"
                                  show-word-limit></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="bio" label="Bio">
                        <el-input type="textarea" :rows="2" v-model="userUpdateForm.bio" clearable
                                  maxlength="1000" show-word-limit></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="avatar" label="Color">
                        <el-button type="info" plain ref="colorButton" style="width: 100%" @click="generateColor">
                            <strong>Generate new color</strong>
                        </el-button>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="userRole" label="User Status">
                        <el-select v-model="userUpdateForm.isActivated" :value="userUpdateForm.isActivated"
                                   placeholder="Select" style="width: 100%" :disabled="isAboutMe(getUserId)">
                            <el-option label="Activated" :value="true"/>
                            <el-option label="Deactivated" :value="false"/>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="userRole" label="User Role">
                        <el-row type="flex" justify="center">
                            <el-col :span="20">
                                <el-select v-model="userUpdateForm.userRole.id" placeholder="Select"
                                           :value="userUpdateForm.userRole.id" clearable
                                           style="width: 100%" :disabled="isAboutMe(getUserId)">
                                    <el-option v-for="item in userRoles" :key="item.value" :label="item.label"
                                               :value="item.value"/>
                                </el-select>
                            </el-col>
                            <el-col :span="4">
                                <el-button type="info" plain circle @click="openUserRolesTableView">
                                    <fa icon="info-circle"/>
                                </el-button>
                            </el-col>
                        </el-row>
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
                                <el-button type="primary" ref="userUpdateFormButton" native-type="submit"
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
    import { mapGetters } from "vuex";
    import { generateColor, getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_USER_ROLES_SHORT_INFO_ENDPOINT, UPDATE_USER_PROFILE_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import format from "string-format";

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
                },
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
                return this.user.firstName === this.userUpdateForm.firstName &&
                    this.user.lastName === this.userUpdateForm.lastName &&
                    this.user.email === this.userUpdateForm.email &&
                    this.user.color === this.userUpdateForm.color &&
                    this.user.location === this.userUpdateForm.location &&
                    this.user.bio === this.userUpdateForm.bio &&
                    this.user.userRole.id === this.userUpdateForm.userRole.id &&
                    this.user.isActivated === this.userUpdateForm.isActivated;
            }
        },
        methods: {
            generateColor() {
                this.userUpdateForm.color = generateColor();
            },
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
                this.$refs.userUpdateFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.userUpdateFormButton.loading = false;
                        return false;
                    }

                    if (!this.isNotChanged) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_USER_PROFILE_ENDPOINT, {
                                id: this.user.id
                            }),
                            data: {
                                firstName: this.userUpdateForm.firstName,
                                lastName: this.userUpdateForm.lastName,
                                email: this.userUpdateForm.email,
                                color: this.userUpdateForm.color,
                                location: this.userUpdateForm.location,
                                bio: this.userUpdateForm.bio,
                                userRoleId: this.userUpdateForm.userRole.id ? this.userUpdateForm.userRole.id : null,
                                isActivated: this.userUpdateForm.isActivated
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.pageStatus.visible = false;
                            this.$refs.userUpdateFormButton.loading = false;

                            this.$notify.success({
                                title: "Profile was updated",
                                message: "Your profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.userUpdateFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update profile",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    } else {
                        this.pageStatus.visible = false;
                        this.$refs.userUpdateFormButton.loading = false;

                        this.$notify.info({
                            title: "Profile wasn't updated",
                            message: "No change for update of your profile"
                        });
                    }
                });
            },
            openUserRolesTableView() {
                const route = this.$router.resolve({
                    path: "/users/roles",
                    query: {
                        id: this.userUpdateForm.userRole.id
                    }
                });
                if (route) {
                    window.open(route.href, "_blank");
                }
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