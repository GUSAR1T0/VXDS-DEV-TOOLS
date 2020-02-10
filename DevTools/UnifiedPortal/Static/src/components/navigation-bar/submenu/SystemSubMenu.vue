<template>
    <div class="more">
        <el-menu-item index="/system/settings">
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
    export default {
        name: "SystemSubMenu"
    };
</script>