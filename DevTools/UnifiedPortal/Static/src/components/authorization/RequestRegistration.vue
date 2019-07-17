<template>
    <div class="request-registration">
        <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="120px">
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
                <el-button type="danger" class="auth-button" @click="submitForm('ruleForm')">Send Request</el-button>
            </el-row>
        </el-form>
    </div>
</template>

<style scoped src="@/styles/authorization.css">
</style>

<script>
    import RequestRegistrationValidations from "@/extensions/validations";

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
                        {required: true, message: "Please, input first name", trigger: "blur"},
                        {min: 2, max: 30, message: "Length should be 2 to 30", trigger: "blur"}
                    ],
                    lastName: [
                        {required: true, message: "Please, input last name", trigger: "blur"},
                        {min: 2, max: 30, message: "Length should be 2 to 30", trigger: "blur"}
                    ],
                    email: [
                        {required: true, message: "Please, input email address", trigger: "blur"},
                        {type: "email", message: "Please, input correct email address", trigger: "blur"}
                    ],
                    password: [
                        {required: true, validator: validations.validatePassword, trigger: "blur"}
                    ],
                    passwordConfirmation: [
                        {required: true, validator: validations.validatePasswordConfirmation, trigger: "blur"}
                    ]
                }
            };
        },
        methods: {
            submitForm(formName) {
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        return false;
                    }
                });
            }
        }
    };
</script>
