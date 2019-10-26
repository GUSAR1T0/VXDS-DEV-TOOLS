<template>
    <el-card shadow="hover">
        <div slot="header">
            <h3>User Roles</h3>
        </div>
        
        <el-button class="user-container-card-button" type="danger" plain style="width: 100%">
            <span><fa icon="edit"/> | Edit User Roles</span>
        </el-button>
    </el-card>
</template>

<script>
    export default {
        name: "UserRolesCard"
    };
</script>

<style scoped>

</style>