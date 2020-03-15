<template>
    <div>
        <el-avatar :size="128" style="margin-top: 5px" :style="getAvatarStyle">
            <div style="font-size: 48px">
                {{ getInitials }}
            </div>
        </el-avatar>
        <h2>{{ getFullName }}</h2>
        <h3 style="margin-bottom: 5px">{{ user.email }}</h3>
    </div>
</template>

<script>
    import { getUserFullName, getUserInitials } from "@/extensions/utils";

    export default {
        name: "UserVerticalCard",
        props: {
            user: Object
        },
        computed: {
            getFullName() {
                return getUserFullName(this.user.firstName, this.user.lastName);
            },
            getInitials() {
                return getUserInitials(this.user.firstName, this.user.lastName);
            },
            getAvatarStyle() {
                return {
                    backgroundColor: this.user.color
                };
            }
        }
    };
</script>