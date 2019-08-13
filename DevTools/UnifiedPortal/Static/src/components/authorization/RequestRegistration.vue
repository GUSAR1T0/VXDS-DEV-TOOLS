<template>
    <div class="request-registration">
        <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="120px"
                 @submit.native.prevent="submitForm('ruleForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="firstName" label="First Name">
                        <el-input v-model="ruleForm.firstName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="lastName" label="Last Name">
                        <el-input v-model="ruleForm.lastName" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="email" label="Email Address">
                        <el-input v-model="ruleForm.email" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="password" label="Password">
                        <el-input v-model="ruleForm.password" show-password clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="passwordConfirmation" label="Confirmation">
                        <el-input v-model="ruleForm.passwordConfirmation" show-password clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-form-item>
                    <el-button type="danger" class="auth-button" native-type="submit">
                        Send Request
                    </el-button>
                </el-form-item>
            </el-row>
        </el-form>
    </div>
</template>

<style scoped src="@/styles/authorization.css">
</style>

<script>
    import RequestRegistrationValidations from "@/extensions/validations";
    import axios from "axios";
    import apis from "@/constants/apis";
    import { mapMutations } from "vuex";

    let ruleForm = {
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        passwordConfirmation: ""
    };
    let validations = new RequestRegistrationValidations(ruleForm);

    export default {
        name: "RequestRegistration",
        data() {
            return {
                ruleForm: ruleForm,
                rules: {
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
            ...mapMutations([
                "login"
            ])
        },
        methods: {
            submitForm(formName) {
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        return false;
                    }

                    axios.post(apis.RegisterUser, ruleForm).then(response => {
                        this.$store.commit("login", {
                            accessToken: response.data.accessToken,
                            refreshToken: response.data.refreshToken,
                            complete: fullName => {
                                this.$router.push("/");
                                this.$notify.info({
                                    title: "Info",
                                    message: "You are registered as " + fullName
                                });
                            }
                        });
                    }).catch(error => {
                        this.$notify.error({
                            title: "Error",
                            message: error.response.data
                        });
                    });
                });
            }
        }
    };
</script>
