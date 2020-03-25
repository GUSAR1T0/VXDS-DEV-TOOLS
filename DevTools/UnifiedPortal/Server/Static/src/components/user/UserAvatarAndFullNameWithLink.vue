<template>
    <div>
        <el-link :href="`/user/${userId}`" type="primary" :underline="false" v-if="userId">
            <UserAvatarAndFullName
                    :first-name="firstName"
                    :last-name="lastName"
                    :color="color"
                    :big="big"
            />
        </el-link>
        <div v-else>
            <UserAvatarAndFullName
                    :first-name="firstName"
                    :last-name="lastName"
                    :color="color"
                    :big="big"
            />
        </div>
    </div>
</template>

<script>
    import UserAvatarAndFullName from "@/components/user/UserAvatarAndFullName";

    export default {
        name: "UserAvatarAndFullNameWithLink",
        props: {
            firstName: String,
            lastName: String,
            color: String,
            userId: Number,
            big: Boolean
        },
        components: {
            UserAvatarAndFullName
        }
    };
</script>