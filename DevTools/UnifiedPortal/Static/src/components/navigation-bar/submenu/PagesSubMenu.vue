<template>
    <div class="pages">
        <el-menu-item index="/about">
            <fa class="fa-submenu-item" icon="info-circle"/>
            <span slot="title" class="el-nav-menu-vertical-item">About</span>
        </el-menu-item>
        <el-menu-item index="/users">
            <fa class="fa-submenu-item" icon="users"/>
            <span slot="title" class="el-nav-menu-vertical-item">Users</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    export default {
        name: "PagesSubMenu"
    };
</script>