<template>
    <div class="account">
        <div v-if="isAuthenticated">
            <el-submenu index="">
                <span slot="title">Users</span>
                <el-menu-item index="/users">
                    <fa class="fa-submenu-item" icon="users"/>
                    <span slot="title" class="el-nav-menu-vertical-item">Users</span>
                </el-menu-item>
                <el-menu-item index="/users/roles">
                    <fa class="fa-submenu-item" icon="users-cog"/>
                    <span slot="title" class="el-nav-menu-vertical-item">User Roles</span>
                </el-menu-item>
            </el-submenu>
            <el-menu-item index="/user">
                <fa class="fa-submenu-item" icon="user-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">{{ getFullName }}</span>
            </el-menu-item>
            <el-menu-item index="" @click="logoutDialogStatus.visible = true">
                <fa class="fa-submenu-item" icon="sign-out-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">Sign Out</span>
            </el-menu-item>
        </div>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "AccountSubMenu",
        props: {
            logoutDialogStatus: Object
        },
        computed: {
            ...mapGetters([
                "isAuthenticated",
                "getFullName",
            ])
        }
    };
</script>