<template>
    <div class="sign-in">
        <el-form :model="signInForm" :rules="signInRules" ref="signInForm" label-width="120px"
                 @submit.native.prevent="submitForm('signInForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="email" label="Email Address">
                        <el-input v-model="signInForm.email" clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="password" label="Password">
                        <el-input v-model="signInForm.password" show-password clearable></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-button type="primary" ref="signInButton" class="auth-button" native-type="submit">
                            <strong>Log In</strong>
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
    import { mapGetters } from "vuex";
    import { SIGN_IN_REQUEST } from "@/constants/actions";
    import { renderErrorNotificationMessage } from "@/extensions/utils";

    let signInForm = {
        email: "",
        password: ""
    };

    export default {
        name: "SignIn",
        data() {
            return {
                signInForm,
                signInRules: {
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
            ...mapGetters([
                "getFullName"
            ])
        },
        methods: {
            submitForm(formName) {
                this.$refs.signInButton.loading = true;
                this.$refs[formName].validate(valid => {
                    if (!valid) {
                        this.$refs.signInButton.loading = false;
                        return false;
                    }

                    this.$store.dispatch(SIGN_IN_REQUEST, this.signInForm).then(() => {
                        this.$refs.signInButton.loading = false;
                        this.$router.push("/");
                        const h = this.$createElement;
                        this.$notify.success({
                            title: "You are logged in",
                            message: h("div", null, [
                                "Welcome back, ",
                                h("strong", null, this.getFullName)
                            ])
                        });
                    }).catch(error => {
                        this.$refs.signInButton.loading = false;
                        this.$notify.error({
                            title: "Failed to sign in",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                });
            }
        }
    };
</script>
