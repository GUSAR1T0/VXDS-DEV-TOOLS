<template>
    <UserRolesPermissionsTables is-editable/>
</template>

<script>
    import UserRolesPermissionsTables from "@/components/user-role/UserRolesPermissionsTables";

    export default {
        name: "UserRoles",
        components: {
            UserRolesPermissionsTables
        }
    };
</script>