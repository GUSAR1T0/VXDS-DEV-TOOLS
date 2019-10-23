<template>
    <div class="user">
        <div v-if="isAuthenticated">
            <el-menu-item index="/user">
                <fa class="fa-submenu-item" icon="user-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">{{ getFullName }}</span>
            </el-menu-item>
            <el-menu-item @click="pageStatus.logoutDialogVisible = true">
                <fa class="fa-submenu-item" icon="sign-out-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">Sign Out</span>
            </el-menu-item>
        </div>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "UserSubMenu",
        props: {
            pageStatus: Object
        },
        computed: {
            ...mapGetters([
                "isAuthenticated",
                "getFullName",
            ])
        }
    };
</script>