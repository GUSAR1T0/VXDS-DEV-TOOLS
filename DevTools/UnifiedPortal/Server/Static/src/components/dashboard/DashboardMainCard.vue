<template>
    <el-card shadow="hover">
        <el-row type="flex" justify="center" align="middle" class="dashboard-main-item">
            <el-col :span="12">
                <div style="font-size: 72px">
                    <fa :icon="icon"/>
                </div>
            </el-col>
            <el-col :span="12" v-loading="state.loadingIsActive">
                <div v-if="!state.loadingIsActive">
                    <slot/>
                </div>
            </el-col>
        </el-row>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "DashboardMainCard",
        props: {
            icon: String,
            load: Function
        },
        data() {
            return {
                state: {
                    loadingIsActive: true
                }
            };
        },
        computed: {},
        methods: {
            loadContent() {
                this.state.loadingIsActive = true;
                this.load(this.state);
            }
        },
        mounted() {
            this.loadContent();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadContent();
            next();
        }
    };
</script>