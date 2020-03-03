<template>
    <div class="user-avatar-and-name">
        <el-avatar :size="32" :style="getAvatarStyle">
            <div style="font-size: 16px">
                {{ getInitials }}
            </div>
        </el-avatar>
        <strong :style="`font-size: ${big ? 36 : 16}px`">
            {{ getFullName }}
        </strong>
    </div>
</template>

<style scoped src="@/styles/user.css">
</style>

<script>
    import { getUserFullName, getUserInitials } from "@/extensions/utils";

    export default {
        name: "UserAvatarAndFullName",
        props: {
            firstName: String,
            lastName: String,
            color: String,
            big: Boolean
        },
        computed: {
            getInitials() {
                return getUserInitials(this.firstName, this.lastName);
            },
            getFullName() {
                return getUserFullName(this.firstName, this.lastName);
            },
            getAvatarStyle() {
                return {
                    backgroundColor: this.color ? this.color : "#909399",
                    marginRight: "10px"
                };
            }
        }
    };
</script>