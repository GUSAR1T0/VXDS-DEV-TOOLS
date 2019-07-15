<template>
    <div class="more">
        <el-menu-item @click="changeState">
            <fa v-if="isCollapse" class="fa-submenu-item" icon="angle-right"/>
            <fa v-else class="fa-submenu-item" icon="angle-left"/>
            <span v-if="isCollapse" slot="title" class="el-nav-menu-vertical-item">Expand</span>
            <span v-else slot="title" class="el-nav-menu-vertical-item">Collapse</span>
        </el-menu-item>
    </div>
</template>

<style scoped src="./css/submenu.css">
</style>

<script>
    import { mapState } from "vuex";

    export default {
        name: "MoreSubMenu",
        computed: {
            ...mapState({
                isCollapse: state => state.navigationBar.isNavigationBarCollapse
            })
        },
        methods: {
            changeState() {
                this.$store.commit("collapseOrExpandNavigationBar");
            }
        }
    };
</script>