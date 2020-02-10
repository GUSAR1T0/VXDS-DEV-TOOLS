<template>
    <el-card shadow="hover">
        <div slot="header">
            <h3>User Roles</h3>
        </div>
        <div class="system-row-content">{{ rolesCount }}</div>
        <el-button type="primary" plain class="system-row-button" @click="$router.push('/users/roles')">
            <span>See User Roles</span>
        </el-button>
    </el-card>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    export default {
        name: "UserRolesCard",
        props: {
            rolesCount: [String, Number]
        }
    };
</script>