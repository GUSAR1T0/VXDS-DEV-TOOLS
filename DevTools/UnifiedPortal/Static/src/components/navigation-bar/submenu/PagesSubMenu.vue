<template>
    <div class="pages">
        <el-menu-item index="/pages/about">
            <fa class="fa-submenu-item" icon="info-circle"/>
            <span slot="title" class="el-nav-menu-vertical-item">About</span>
        </el-menu-item>
        <el-menu-item index="/pages/projects">
            <fa class="fa-submenu-item" icon="code"/>
            <span slot="title" class="el-nav-menu-vertical-item">Projects</span>
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