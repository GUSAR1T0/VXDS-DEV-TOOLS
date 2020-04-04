<template>
    <DashboardCard
            :entityName="entityName"
            :entityIcon="entityIcon"
            :linkToEntity="linkToEntity"
            :load="load"
    >
        <template v-slot="{ state }">
            <el-col :span="5">
                <div>
                    <div class="dashboard-total-key">Total:</div>
                    <div class="dashboard-total-value">
                        <strong>{{ total === null ? "—" : total }}</strong>
                    </div>
                </div>
            </el-col>
            <el-col span="1">
                <el-divider direction="vertical"/>
            </el-col>
            <el-col :span="18">
                <slot v-bind:state="state"/>
            </el-col>
        </template>
    </DashboardCard>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<style scoped>
    .el-divider--vertical {
        height: auto !important;
        min-height: 150px !important;
    }
</style>

<script>
    import DashboardCard from "@/components/dashboard/DashboardCard";

    export default {
        name: "DashboardCardWithTotal",
        props: {
            total: Number,
            entityName: String,
            entityIcon: String,
            linkToEntity: String,
            load: Function
        },
        components: {
            DashboardCard
        }
    };
</script>