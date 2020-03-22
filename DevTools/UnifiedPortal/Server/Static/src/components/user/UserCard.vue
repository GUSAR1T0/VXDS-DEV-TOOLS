<template>
    <div>
        <el-row type="flex" justify="center" align="middle">
            <el-col :md="10" :lg="10" :xl="10" class="hidden-sm-and-down">
                <div style="text-align: right">
                    <el-avatar :size="128" :style="getAvatarStyle">
                        <div style="font-size: 48px">
                            {{ getInitials }}
                        </div>
                    </el-avatar>
                </div>
            </el-col>
            <el-col :md="1" :lg="1" :xl="1" class="hidden-sm-and-down">
                <el-divider direction="vertical"/>
            </el-col>
            <el-col :md="10" :lg="10" :xl="10" class="hidden-sm-and-down">
                <h2 style="text-align: left">{{ getFullName }}</h2>
                <h3 style="text-align: left">{{ user.email }}</h3>
            </el-col>
        </el-row>
        <el-row type="flex" justify="center" align="middle">
            <el-col :xs="24" :sm="24" class="hidden-md-and-up">
                <UserVerticalCard :user="user"/>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>
    .el-divider--vertical {
        height: 150px;
    }
</style>

<script>
    import { getUserFullName, getUserInitials } from "@/extensions/utils";

    import UserVerticalCard from "@/components/user/UserVerticalCard";

    export default {
        name: "UserCard",
        props: {
            user: Object
        },
        components: {
            UserVerticalCard
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