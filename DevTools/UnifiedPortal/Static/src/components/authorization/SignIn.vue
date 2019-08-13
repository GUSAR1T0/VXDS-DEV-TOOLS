<template>
    <div class="sign-in">
        <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="120px"
                 @submit.native.prevent="submitForm('ruleForm')">
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
                <el-form-item>
                    <el-button type="danger" class="auth-button" native-type="submit">Log In</el-button>
                </el-form-item>
            </el-row>
        </el-form>
    </div>
</template>

<style scoped src="@/styles/authorization.css">
</style>

<script>
    import { mapMutations } from "vuex";
    import axios from "axios";
    import apis from "@/constants/apis";

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
                        {required: true, message: "Email address is required", trigger: "change"},
                        {type: "email", message: "Please, input correct email address", trigger: "change"}
                    ],
                    password: [
                        {required: true, message: "Password is required", trigger: "change"}
                    ]
                }
            };
        },
        computed: {
            ...mapMutations([
                "login",
                "logout"
            ])
        },
        methods: {
            submitForm(formName) {
                this.$refs[formName].validate(valid => {
                    if (!valid) {
                        return false;
                    }

                    axios.post(apis.GenerateToken, ruleForm).then(response => {
                        this.$store.commit("login", {
                            accessToken: response.data.accessToken,
                            refreshToken: response.data.refreshToken,
                            complete: fullName => {
                                this.$router.push("/");
                                this.$notify.info({
                                    title: "Info",
                                    message: "You are logged in as " + fullName
                                });
                            }
                        });
                    }).catch(error => {
                        this.$store.commit("logout");
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
