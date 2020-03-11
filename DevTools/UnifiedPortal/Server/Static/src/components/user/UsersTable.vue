<template>
    <el-table :data="users" style="width: 100%" border highlight-current-row>
        <el-table-column label="User ID" min-width="100" align="center">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.id }}</strong>
            </template>
        </el-table-column>
        <el-table-column label="User Name" min-width="400" align="center">
            <template slot-scope="scope">
                <div style="text-align: left">
                    <el-link :href="`/user/${scope.row.id}`" type="primary" :underline="false">
                        <UserAvatarAndFullName
                                :first-name="scope.row.firstName"
                                :last-name="scope.row.lastName"
                                :color="scope.row.color"
                        />
                    </el-link>
                </div>
            </template>
        </el-table-column>
        <el-table-column label="Email Address" min-width="400" align="center">
            <template slot-scope="scope">
                <el-link :href="`mailto:${scope.row.email}`" type="primary" :underline="false">
                    <strong style="font-size: 16px">{{ scope.row.email }}</strong>
                </el-link>
            </template>
        </el-table-column>
        <el-table-column label="User Role" min-width="350" align="center">
            <template slot-scope="scope">
                <el-link :href="`/users/roles?id=${scope.row.userRoleId}`" type="primary" :underline="false"
                         v-if="scope.row.userRoleId">
                    <strong style="font-size: 16px">{{ scope.row.userRole }}</strong>
                </el-link>
                <div v-else>—</div>
            </template>
        </el-table-column>
        <el-table-column label="User Status" min-width="250" align="center">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.isActivated ? "Activated" : "Deactivated" }}</strong>
            </template>
        </el-table-column>
    </el-table>
</template>

<script>
    import UserAvatarAndFullName from "@/components/user/UserAvatarAndFullName";

    export default {
        name: "UsersTable",
        props: {
            users: Array
        },
        components: {
            UserAvatarAndFullName
        }
    };
</script>