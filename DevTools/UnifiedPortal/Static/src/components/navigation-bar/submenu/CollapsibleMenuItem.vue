<template>
    <el-menu-item :index="index">
        <slot name="icon"></slot>
        <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">{{ name }}</span>
    </el-menu-item>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapState } from "vuex";

    export default {
        name: "CollapsibleMenuItem",
        props: {
            name: String,
            index: String
        },
        computed: {
            ...mapState({
                isCollapse: state => state.navigationBar.isNavigationBarCollapse
            })
        }
    };
</script>