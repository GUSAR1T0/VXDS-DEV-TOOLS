<template>
    <div class="more">
        <el-menu-item>
            <fa class="fa-submenu-item" icon="ellipsis-h"/>
            <span slot="title" class="el-nav-menu-vertical-item">One more thing...</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    export default {
        name: "MoreSubMenu"
    };
</script>