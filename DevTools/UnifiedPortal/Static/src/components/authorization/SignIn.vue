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
                        <el-button type="danger" ref="signInButton" class="auth-button" native-type="submit">
                            Log In
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
                        this.$notify.info({
                            title: "Info",
                            message: `You are logged in as ${this.getFullName}`
                        });
                    }).catch(error => {
                        this.$refs.signInButton.loading = false;
                        this.$notify.error({
                            title: "Error",
                            message: `Failed to sign in: ${error.response.data}`
                        });
                    });
                });
            }
        }
    };
</script>
