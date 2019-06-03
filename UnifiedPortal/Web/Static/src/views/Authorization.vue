<template>
    <div class="authorization">
        <el-row class="main-container">
            <el-col :span="24">
                <Welcome msg="Authorization"/>
                <SignIn/>
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
</style>

<script>
    import Welcome from "@/components/Welcome.vue";
    import SignIn from "@/components/authorization/SignIn.vue";

    export default {
        name: "Home",
        components: {
            Welcome: Welcome,
            SignIn: SignIn
        }
    };
</script>