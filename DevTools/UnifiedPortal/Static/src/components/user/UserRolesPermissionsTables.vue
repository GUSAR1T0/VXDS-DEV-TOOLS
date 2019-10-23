<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <div style="margin-top: -25px;"></div>
            <el-card shadow="hover" style="margin-top: 25px;" v-for="item in userRoles" :key="item.id">
                <div slot="header" style="text-align: center">
                    <h3>{{ item.name }}</h3>
                </div>
                <UserRolePermissionsTable :user-role="item"/>
            </el-card>
        </template>
    </LoadingContainer>
</template>

<script>
    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserRolePermissionsTable from "@/components/user/UserRolePermissionsTable";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { GET_USER_ROLES_FULL_INFO_ENDPOINT } from "@/constants/endpoints";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    export default {
        name: "UserRolesPermissionsTables",
        components: {
            LoadingContainer,
            UserRolePermissionsTable
        },
        data() {
            return {
                loadingIsActive: true,
                userRoles: []
            };
        },
        methods: {
            loadUserRoles() {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLES_FULL_INFO_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.userRoles = response.data;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$router.back();
                    this.$notify.error({
                        title: "Failed to load list of user roles",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.loadUserRoles();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadUserRoles();
            next();
        }
    };
</script>
