<template>
    <div class="sign-in">
        <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="120px">
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
                <el-button type="danger" class="auth-button" @click="submitForm('ruleForm')">Log In</el-button>
            </el-row>
        </el-form>
    </div>
</template>

<style scoped src="./css/authorization.css">
</style>

<script>
    let ruleForm = {
        email: "",
        password: ""
    };

    export default {
        name: "SignIn",
        data() {
            return {
                ruleForm: ruleForm,
                rules: {
                    email: [
                        {required: true, message: "Email address is required", trigger: "blur"}
                    ],
                    password: [
                        {required: true, message: "Password is required", trigger: "blur"}
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
