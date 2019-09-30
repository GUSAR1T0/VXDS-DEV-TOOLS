<template>
    <div class="authorization">
        <div class="alert-message">
            <el-alert title="Welcome to the system" type="info" effect="light" center :closable="false" >
                <div class="alert-description">Before we'll start, please log in</div>
            </el-alert>
        </div>
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
    .alert-message {
        padding-bottom: 25px;
    }

    .alert-description {
        font-size: 24px;
        padding-bottom: 5px;
    }
</style>

<style>
    .el-alert__title {
        font-size: 36px;
        line-height: normal !important;
    }

    .el-input__inner:focus {
        border-color: #F56C6C !important;
        outline: none;
    }

    .el-tabs__item.is-active {
        color: #F56C6C !important;
    }
</style>

<script>
    import SignIn from "@/components/authorization/SignIn.vue";
    import SignUp from "@/components/authorization/SignUp.vue";

    export default {
        name: "Authorization",
        components: {
            SignIn,
            SignUp
        }
    };
</script>