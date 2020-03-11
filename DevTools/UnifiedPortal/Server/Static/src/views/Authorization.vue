<template>
    <div class="authorization">
        <div class="alert-message">
            <el-row type="flex" justify="center" align="middle" style="padding-top: 5px">
                <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png" width="64px"
                     height="64px"/>
            </el-row>
            <el-row class="alert-title">
                Welcome to the system
            </el-row>
            <el-row class="alert-description">Before we'll start, please log in</el-row>
        </div>
        <HorizontalDivider/>
        <el-row class="main-container">
            <el-col :span="24">
                <el-tabs type="border-card" :stretch="true">
                    <el-tab-pane>
                        <span slot="label"><fa icon="sign-in-alt"/> | Sign In</span>
                        <SignIn/>
                    </el-tab-pane>
                    <el-tab-pane>
                        <span slot="label"><fa icon="user-plus"/> | Sign Up</span>
                        <SignUp/>
                    </el-tab-pane>
                </el-tabs>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>
    .alert-title {
        font-size: 36px;
        line-height: normal !important;
    }

    .alert-description {
        font-size: 24px;
        padding-top: 5px;
    }
    
    .alert-message {
        padding: 25px 0;
    }
</style>

<style>
    .el-input__inner:focus {
        outline: none;
    }
</style>

<script>
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import SignIn from "@/components/authorization/SignIn.vue";
    import SignUp from "@/components/authorization/SignUp.vue";

    export default {
        name: "Authorization",
        components: {
            HorizontalDivider,
            SignIn,
            SignUp
        }
    };
</script>