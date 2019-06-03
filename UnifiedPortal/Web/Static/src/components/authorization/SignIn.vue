<template>
    <div class="sign-in">
        <el-row class="sign-in-element" type="flex" justify="center">
            <el-col :xs="24" :sm="16" :md="12" :lg="8" :xl="6">
                <el-input placeholder="Username" v-model="login" clearable></el-input>
            </el-col>
        </el-row>
        <el-row class="sign-in-element" type="flex" justify="center">
            <el-col :xs="24" :sm="16" :md="12" :lg="8" :xl="6">
                <el-input placeholder="Password" v-model="password" show-password clearable></el-input>
            </el-col>
        </el-row>
        <el-row class="sign-in-element" type="flex" justify="center">
            <el-col :xs="10" :sm="8" :md="6" :lg="3" :xl="1">
                <el-button type="danger" class="button"><fa icon="sign-in-alt"/> | Sign In</el-button>
            </el-col>
            <el-col :xs="10" :sm="8" :md="6" :lg="3" :xl="1">
                <el-button type="danger" class="button"><fa icon="user-plus"/> | Request</el-button>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>
    .sign-in-element {
        padding: 10px;
    }
    
    .button {
        width: 130px;
    }
</style>

<script>
    export default {
        name: "SignIn",
        data() {
            return {
                login: "",
                password: ""
            };
        }
    };
</script>
