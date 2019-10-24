<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <el-table :data="users" style="width: 100%" border highlight-current-row>
                <el-table-column label="User ID" min-width="75" align="center">
                    <template slot-scope="scope">
                        <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                    </template>
                </el-table-column>
                <el-table-column prop="type" label="User Name" min-width="300" align="center">
                    <template slot-scope="scope">
                        <div class="user-avatar-and-name">
                            <el-avatar :size="32" :style="scope.row.style">
                                <div style="font-size: 16px">
                                    {{ scope.row.initials }}
                                </div>
                            </el-avatar>
                            <strong style="font-size: 16px">{{ scope.row.userName }}</strong>
                        </div>
                    </template>
                </el-table-column>
                <el-table-column label="Email Address" min-width="300" align="center">
                    <template slot-scope="scope">
                        <strong style="font-size: 16px">{{ scope.row.email }}</strong>
                    </template>
                </el-table-column>
                <el-table-column label="User Role" min-width="200" align="center">
                    <template slot-scope="scope">
                        <strong style="font-size: 16px">{{ scope.row.userRole }}</strong>
                    </template>
                </el-table-column>
                <el-table-column label="Operations" min-width="150" align="center" fixed="right">
                    <template slot-scope="scope">
                        <el-button @click="$router.push(`/user/${scope.row.id}`)" circle type="danger" style="margin-right: 10px">
                            <span><fa icon="external-link-alt"/></span>
                        </el-button>
                        <a :href="`mailto:${scope.row.email}`">
                            <el-button circle type="danger">
                                <span><fa icon="envelope"/></span>
                            </el-button>
                        </a>
                    </template>
                </el-table-column>
            </el-table>
        </template>
    </LoadingContainer>
</template>

<style scoped>
    .user-avatar-and-name {
        display: flex;
        align-items: center;
    }
</style>

<script>
    import {
        getConfiguration,
        getUserFullName,
        getUserInitials,
        renderErrorNotificationMessage
    } from "@/extensions/utils";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_USERS_ENDPOINT } from "@/constants/endpoints";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";

    export default {
        name: "Users",
        components: {
            LoadingContainer
        },
        data() {
            return {
                loadingIsActive: true,
                users: []
            };
        },
        methods: {
            getUsersTable(users) {
                let userTable = [];
                for (let i = 0; i < users.length; i++) {
                    let user = {
                        id: users[i].id,
                        userName: getUserFullName(users[i].firstName, users[i].lastName),
                        initials: getUserInitials(users[i].firstName, users[i].lastName),
                        userRole: users[i].userRole,
                        style: {
                            backgroundColor: users[i].color,
                            marginRight: "10px"
                        },
                        email: users[i].email
                    };
                    userTable.push(user);
                }
                return userTable;
            },
            loadUsers() {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USERS_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.users = this.getUsersTable(response.data);
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of users",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.loadUsers();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadUsers();
            next();
        },
    };
</script>