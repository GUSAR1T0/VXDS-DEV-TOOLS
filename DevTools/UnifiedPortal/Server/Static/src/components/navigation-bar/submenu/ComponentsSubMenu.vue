<template>
    <div class="components">
        <el-menu-item index="/components/projects">
            <fa class="fa-submenu-item" icon="code"/>
            <span slot="title" class="el-nav-menu-vertical-item">Projects</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    export default {
        name: "ComponentsSubMenu"
    };
</script>