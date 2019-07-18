<template>
    <div class="authorization">
        <el-row class="main-container">
            <el-col :span="24">
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
    import SignIn from "@/components/authorization/SignIn.vue";
    import RequestRegistration from "@/components/authorization/RequestRegistration.vue";

    export default {
        name: "Authorization",
        components: {
            SignIn,
            RequestRegistration
        },
        data() {
            return {
                isStretch: true
            };
        }
    };
</script>