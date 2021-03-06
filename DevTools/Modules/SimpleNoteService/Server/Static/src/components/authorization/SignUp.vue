<template>
    <div class="sign-up">
        <UserCard :user="signUpForm" style="padding-bottom: 5px"/>
        <el-form :model="signUpForm" :rules="signUpRules" ref="signUpForm" label-width="120px"
                 @submit.native.prevent="submitForm('signUpForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="firstName" label="First Name">
                        <el-input v-model="signUpForm.firstName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="lastName" label="Last Name">
                        <el-input v-model="signUpForm.lastName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="email" label="Email Address">
                        <el-input v-model="signUpForm.email" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="password" label="Password">
                        <el-input v-model="signUpForm.password" show-password clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="passwordConfirmation" label="Confirmation">
                        <el-input v-model="signUpForm.passwordConfirmation" show-password clearable></el-input>
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
                    <el-form-item>
                        <el-button type="primary" ref="signUpButton" class="auth-button" native-type="submit">
                            <strong>Sign Up</strong>
                        </el-button>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
    </div>
</template>

<style scoped src="@/styles/authorization.css">
</style>

<script>
    import SignUpValidations from "@/extensions/validations";
    import { mapGetters } from "vuex";
    import { SIGN_UP_REQUEST } from "@/constants/actions";
    import { generateColor, renderErrorNotificationMessage } from "@/extensions/utils";

    import UserCard from "@/components/user/UserCard.vue";

    let signUpForm = {
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        passwordConfirmation: "",
        color: generateColor()
    };
    let validations = new SignUpValidations(signUpForm);

    export default {
        name: "RequestRegistration",
        components: {
            UserCard
        },
        data() {
            return {
                signUpForm,
                signUpRules: {
                    firstName: [
                        {required: true, message: "Please, input first name", trigger: "change"},
                        {min: 2, max: 50, message: "Length should be from 2 to 50", trigger: "change"}
                    ],
                    lastName: [
                        {required: true, message: "Please, input last name", trigger: "change"},
                        {min: 2, max: 50, message: "Length should be from 2 to 50", trigger: "change"}
                    ],
                    email: [
                        {required: true, message: "Please, input email address", trigger: "change"},
                        {type: "email", message: "Please, input correct email address", trigger: "change"},
                        {min: 3, max: 255, message: "Length should be from 3 to 255", trigger: "change"}
                    ],
                    password: [
                        {required: true, validator: validations.validatePassword, trigger: "change"}
                    ],
                    passwordConfirmation: [
                        {required: true, validator: validations.validatePasswordConfirmation, trigger: "change"}
                    ]
                }
            };
        },
        computed: {
            ...mapGetters([
                "getFullName",
                "getUnifiedPortalHost"
            ])
        },
        methods: {
            generateColor() {
                this.signUpForm.color = generateColor();
            },
            submitForm(formName) {
                this.$refs.signUpButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.signUpButton.loading = false;
                        return false;
                    }

                    this.$store.dispatch(SIGN_UP_REQUEST, this.signUpForm).then(() => {
                        this.$refs.signUpButton.loading = false;
                        this.$router.push("/");
                        const h = this.$createElement;
                        this.$notify.success({
                            title: "You are registered",
                            message: h("div", null, [
                                "Welcome to the system, ",
                                h("strong", null, this.getFullName)
                            ])
                        });
                    }).catch(error => {
                        this.$refs.signUpButton.loading = false;
                        this.$notify.error({
                            title: "Failed to sign up",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                        });
                    });
                });
            }
        }
    };
</script>
