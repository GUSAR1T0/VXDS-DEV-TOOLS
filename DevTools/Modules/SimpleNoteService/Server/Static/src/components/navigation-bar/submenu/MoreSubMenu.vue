<template>
    <div class="more">
        <el-menu-item :index="`/unifiedPortal?host=${getUnifiedPortalHost}`">
                <fa class="fa-submenu-item" icon="server"/>
                <span slot="title" class="el-nav-menu-vertical-item">Unified Portal</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "MoreSubMenu",
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getUnifiedPortalHost"
            ])
        }
    };
</script>