<template>
    <el-card shadow="never">
        <div slot="header">
            <h3>Operations</h3>
        </div>
        <div class="system-row-content">{{ operationsCount }}</div>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "OperationsCard",
        props: {
            operationsCount: [ String, Number ]
        }
    };
</script>