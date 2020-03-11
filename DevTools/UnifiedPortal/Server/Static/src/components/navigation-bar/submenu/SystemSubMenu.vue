<template>
    <div class="more">
        <el-menu-item index="/system/settings" v-if="hasPermissionToManageSettings">
            <fa class="fa-submenu-item" icon="tools"/>
            <span slot="title" class="el-nav-menu-vertical-item">Settings</span>
        </el-menu-item>
        <el-menu-item index="/system/operations">
            <fa class="fa-submenu-item" icon="file-alt"/>
            <span slot="title" class="el-nav-menu-vertical-item">Operations</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    export default {
        name: "SystemSubMenu",
        computed: {
            ...mapGetters([
                "hasPortalPermission"
            ]),
            hasPermissionToManageSettings() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_SETTINGS);
            }
        }
    };
</script>