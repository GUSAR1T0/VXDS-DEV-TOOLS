<template>
    <div class="more">
        <el-menu-item @click="changeNavigationBarState">
            <fa v-if="isCollapse" class="fa-submenu-item" icon="angle-right"/>
            <fa v-else class="fa-submenu-item" icon="angle-left"/>
            <span v-if="isCollapse" slot="title" class="el-nav-menu-vertical-item">Expand</span>
            <span v-else slot="title" class="el-nav-menu-vertical-item">Collapse</span>
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
            ...mapGetters({
                isCollapse: "isNavigationBarCollapse"
            })
        },
        methods: {
            changeNavigationBarState() {
                this.$store.commit("collapseOrExpandNavigationBar");
            }
        }
    };
</script>