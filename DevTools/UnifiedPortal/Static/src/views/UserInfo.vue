<template>
    <el-container class="user"
                  v-loading="loadingIsActive"
                  element-loading-text="Loading"
                  element-loading-spinner="el-icon-loading"
                  element-loading-background="rgba(255, 255, 255, 0.75)"
                  element-loading-custom-class="main-loading-spinner-custom">
        <el-main>
            <UserCard :user="user"/>
            <HorizontalDivider name="Details"/>
            <el-card shadow="hover">
                <div slot="header">
                    <h3>General</h3>
                </div>
                <div style="text-align: left;">
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                </div>
            </el-card>
            <el-card shadow="hover" style="margin-top: 25px">
                <div slot="header">
                    <h3>System</h3>
                </div>
                <div style="text-align: left;">
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                    <el-row>1</el-row>
                </div>
            </el-card>
        </el-main>
    </el-container>
</template>

<style scoped>
    .user {
        margin-top: -17px;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import HttpClient from "@/extensions/httpClient";
    import { GET_FULL_USER_DATA_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration } from "@/extensions/utils";
    import UserCard from "@/components/user/UserCard.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import { LOCALHOST } from "@/constants/servers";

    let user = {
        email: "",
        firstName: "",
        lastName: "",
        color: ""
    };

    export default {
        name: "User",
        components: {
            UserCard,
            HorizontalDivider
        },
        data() {
            return {
                user,
                loadingIsActive: true
            };
        },
        computed: {
            ...mapGetters([
                "getEmail"
            ])
        },
        methods: {
            fillForm(email, user) {
                let emailQueried = email === undefined ? this.getEmail : email;
                let query = `${GET_FULL_USER_DATA_ENDPOINT}?email=${emailQueried}`;
                HttpClient.init().then(client => client.get(LOCALHOST, query, getConfiguration()).then(response => {
                    this.loadingIsActive = false;
                    user.email = response.data.email;
                    user.firstName = response.data.firstName;
                    user.lastName = response.data.lastName;
                    user.color = response.data.color;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$router.back();
                    let message = `Failed to load user info: ${error.response.status === 404 ? `user with email "${emailQueried}" was not found` : "unhandled exception"}`;
                    this.$notify.error({
                        title: "Error",
                        message: message
                    });
                }));
            }
        },
        mounted() {
            this.fillForm(this.$route.params.email, this.user);
        },
        beforeRouteUpdate(to, from, next) {
            this.fillForm(to.params.email, this.user);
            next();
        }
    };
</script>