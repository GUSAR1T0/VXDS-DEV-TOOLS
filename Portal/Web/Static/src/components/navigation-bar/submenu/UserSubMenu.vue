<template>
    <div class="user">
        <div v-if="this.$store.getters.isAuthorized">
            <el-menu-item>
                <fa class="fa-submenu-item" icon="user-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">{{ this.$store.getters.fullname }}</span>
            </el-menu-item>
            <el-menu-item>
                <fa class="fa-submenu-item" icon="sign-out-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">Sign Out</span>
            </el-menu-item>
        </div>
        <div v-else>
            <el-menu-item>
                <fa class="fa-submenu-item" icon="sign-in-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">Sign In</span>
            </el-menu-item>
            <el-menu-item>
                <fa class="fa-submenu-item" icon="user-plus"/>
                <span slot="title" class="el-nav-menu-vertical-item">Sign Up</span>
            </el-menu-item>
        </div>
    </div>
</template>

<style scoped src="./css/submenu.css">
</style>

<script>
    export default {
        name: "UserSubMenu"
    };
</script>