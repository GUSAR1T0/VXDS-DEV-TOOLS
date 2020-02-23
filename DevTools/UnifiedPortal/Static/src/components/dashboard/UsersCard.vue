<template>
    <el-card shadow="hover">
        <div slot="header">
            <h3>Users</h3>
        </div>
        <div class="system-row-content">{{ usersCount }}</div>
        <el-button type="primary" plain class="system-row-button" @click="$router.push('/users')">
            <fa icon="users"/><strong> | See Users</strong>
        </el-button>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "UsersCard",
        props: {
            usersCount: [String, Number]
        }
    };
</script>