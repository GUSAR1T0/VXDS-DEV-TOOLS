<template>
    <el-submenu :index="name">
        <template slot="title">
            <slot name="icon"></slot>
            <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">{{ name }}</span>
        </template>
        <el-menu-item-group v-if="isCollapse">
            <span slot="title" class="el-nav-menu-vertical-group-title">{{ name }}</span>
            <slot name="menu"></slot>
        </el-menu-item-group>
        <template v-if="!isCollapse">
            <slot name="menu"></slot>
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
    import { mapState } from "vuex";

    export default {
        name: "CollapsibleSubMenu",
        props: {
            name: String
        },
        computed: {
            ...mapState({
                isCollapse: state => state.navigationBar.isNavigationBarCollapse
            })
        }
    };
</script>