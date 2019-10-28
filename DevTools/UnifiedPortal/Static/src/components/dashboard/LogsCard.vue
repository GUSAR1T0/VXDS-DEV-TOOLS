<template>
    <el-card shadow="hover">
        <div slot="header">
            <h3>Logs</h3>
        </div>
        <div class="system-row-content">{{ logsCount }}</div>
        <el-button type="danger" plain class="system-row-button">
            <span>See logs</span>
        </el-button>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "LogsCard",
        props: {
            logsCount: Number
        }
    };
</script>