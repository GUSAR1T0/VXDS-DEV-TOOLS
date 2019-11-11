<template>
    <UserRolesPermissionsTables is-editable/>
</template>

<script>
    import UserRolesPermissionsTables from "@/components/userRole/UserRolesPermissionsTables";

    export default {
        name: "UserRoles",
        components: {
            UserRolesPermissionsTables
        }
    };
</script>