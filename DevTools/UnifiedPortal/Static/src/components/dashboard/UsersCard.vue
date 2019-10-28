<template>
    <el-card shadow="hover">
        <div slot="header">
            <h3>Users</h3>
        </div>
        <div class="system-row-content">{{ usersCount }}</div>
        <el-button type="danger" plain class="system-row-button" @click="$router.push('/users')">
            <span>See Users</span>
        </el-button>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "UsersCard",
        props: {
            usersCount: Number
        }
    };
</script>