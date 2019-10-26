<template>
    <div class="more">
        <el-menu-item index="/admin" v-if="hasPermissionToAdminPanel">
            <fa class="fa-submenu-item" icon="cogs"/>
            <span slot="title" class="el-nav-menu-vertical-item">Admin Panel</span>
        </el-menu-item>
        <el-menu-item index="">
            <fa class="fa-submenu-item" icon="tools"/>
            <span slot="title" class="el-nav-menu-vertical-item">Settings</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { USER_PERMISSION } from "@/constants/permissions";

    export default {
        name: "MoreSubMenu",
        computed: {
            ...mapGetters([
                "hasUserPermission"
            ]),
            hasPermissionToAdminPanel() {
                return this.hasUserPermission(USER_PERMISSION.ACCESS_TO_ADMIN_PANEL);
            }
        }
    };
</script>