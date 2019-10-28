<template>
    <el-card shadow="hover">
        <div slot="header">
            <h3>Logs</h3>
        </div>
        <el-badge v-if="logsCount > 9999999" value="✱">
            <div class="system-row-content">9999999</div>
        </el-badge>
        <div v-else class="system-row-content">{{ logsCount }}</div>
        <el-button type="danger" plain class="system-row-button">
            <span><fa icon="clipboard-list"/> | See logs</span>
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