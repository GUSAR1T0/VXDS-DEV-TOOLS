<template>
    <el-card shadow="hover" style="margin: 10px">
        <div slot="header">
            <h3>{{ entityName }}</h3>
        </div>
        <el-row type="flex" justify="center" align="middle" style="padding: 10px 0"
                v-loading="state.loadingIsActive"
                element-loading-text="Loading"
                element-loading-spinner="el-icon-loading"
                element-loading-background="rgba(255, 255, 255, 0.75)"
                element-loading-custom-class="main-loading-spinner-custom"
        >
            <slot v-bind:state="state"/>
        </el-row>
        <el-button v-if="linkToEntity" type="primary" plain class="dashboard-button"
                   @click="$router.push(linkToEntity)"
        >
            <fa :icon="entityIcon"/>
            <strong> | See {{ entityName }}</strong>
        </el-button>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "DashboardCard",
        props: {
            entityName: String,
            entityIcon: String,
            linkToEntity: String,
            load: Function
        },
        data() {
            return {
                state: {
                    loadingIsActive: true
                }
            };
        },
        methods: {
            loadEntity() {
                this.state.loadingIsActive = true;
                this.load(this.state);
            }
        },
        mounted() {
            this.loadEntity();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadEntity();
            next();
        }
    };
</script>