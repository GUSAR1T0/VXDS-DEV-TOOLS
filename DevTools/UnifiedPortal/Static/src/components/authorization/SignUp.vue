<template>
    <div class="sign-up">
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
                    <el-form-item>
                        <el-button type="danger" class="auth-button" native-type="submit">
                            Sign Up
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

    let signUpForm = {
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        passwordConfirmation: ""
    };
    let validations = new SignUpValidations(signUpForm);

    export default {
        name: "RequestRegistration",
        data() {
            return {
                signUpForm,
                signUpRules: {
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
                "getFullName"
            ])
        },
        methods: {
            submitForm(formName) {
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        return false;
                    }

                    this.$store.dispatch(SIGN_UP_REQUEST, signUpForm).then(() => {
                        this.$router.push("/");
                        this.$notify.info({
                            title: "Info",
                            message: `You are registered as ${this.getFullName}`
                        });
                    }).catch(error => {
                        this.$notify.error({
                            title: "Error",
                            message: `Failed to sign up: ${error.response.data}`
                        });
                    });
                });
            }
        }
    };
</script>
