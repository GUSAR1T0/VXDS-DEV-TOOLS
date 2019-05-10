<template>
    <div class="portal">
        <el-menu-item index="/">
            <fa class="fa-submenu-item" icon="home"/>
            <span slot="title" class="el-nav-menu-vertical-item">Home</span>
        </el-menu-item>
        <el-menu-item index="/about">
            <fa class="fa-submenu-item" icon="info-circle"/>
            <span slot="title" class="el-nav-menu-vertical-item">About</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="./css/submenu.css">
</style>

<script>
    export default {
        name: "PagesSubMenu"
    };
</script>