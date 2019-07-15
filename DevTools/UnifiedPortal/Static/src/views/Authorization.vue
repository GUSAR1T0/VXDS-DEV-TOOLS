<template>
    <div class="authorization">
        <el-row class="main-container">
            <el-col :span="24">
                <Welcome msg="Authorization"/>
                <el-tabs type="border-card" :stretch="isStretch">
                    <el-tab-pane>
                        <span slot="label"><fa icon="sign-in-alt"/> | Sign In</span>
                        <SignIn/>
                    </el-tab-pane>
                    <el-tab-pane>
                        <span slot="label"><fa icon="user-plus"/> | Request Registration</span>
                        <RequestRegistration/>
                    </el-tab-pane>
                </el-tabs>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>
    .authorization {
        text-align: center;
    }
</style>

<style>
    .el-input__inner:focus {
        border-color: #F56C6C !important;
        outline: none;
    }

    .el-tabs__item.is-active {
        color: #F56C6C !important;
    }
</style>

<script>
    import Welcome from "@/components/Welcome.vue";
    import SignIn from "@/components/authorization/SignIn.vue";
    import RequestRegistration from "@/components/authorization/RequestRegistration.vue";

    export default {
        name: "Home",
        components: {
            Welcome: Welcome,
            SignIn: SignIn,
            RequestRegistration: RequestRegistration
        },
        data() {
            return {
                isStretch: true
            };
        }
    };
</script>