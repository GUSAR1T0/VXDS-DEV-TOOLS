<template>
    <el-menu-item :index="index">
        <slot name="icon"></slot>
        <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">{{ menuName }}</span>
    </el-menu-item>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "CollapsibleMenuItem",
        props: {
            menuName: String,
            index: String
        },
        computed: {
            ...mapGetters({
                isCollapse: "isNavigationBarCollapse"
            })
        }
    };
</script>