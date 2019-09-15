<template>
    <el-submenu :index="menuName">
        <template slot="title">
            <slot name="icon"></slot>
            <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">{{ menuName }}</span>
        </template>
        <el-menu-item-group v-if="isCollapse">
            <span slot="title" class="el-nav-menu-vertical-group-title">{{ menuName }}</span>
            <slot name="submenu"></slot>
        </el-menu-item-group>
        <template v-if="!isCollapse">
            <slot name="submenu"></slot>
        </template>
    </el-submenu>
</template>

<style scoped src="@/styles/submenu.css">
    .el-nav-menu-vertical-group-title {
        text-transform: uppercase;
        font-weight: bold;
    }
</style>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "CollapsibleSubMenu",
        props: {
            menuName: String
        },
        computed: {
            ...mapGetters({
                isCollapse: "isNavigationBarCollapse"
            })
        }
    };
</script>