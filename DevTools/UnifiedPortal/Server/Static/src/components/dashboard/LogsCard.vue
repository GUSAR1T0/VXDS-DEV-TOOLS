<template>
    <el-card shadow="never">
        <div slot="header">
            <h3>Logs</h3>
        </div>
        <div class="system-row-content">{{ logsCount }}</div>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "LogsCard",
        props: {
            logsCount: [ String, Number ]
        }
    };
</script>