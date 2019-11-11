<template>
    <el-table :data="getPermissionsTable" style="width: 100%" border>
        <el-table-column prop="type" label="Permissions types">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.type }}</strong>
            </template>
        </el-table-column>
        <el-table-column label="Lists of permissions">
            <template slot-scope="scope">
                <div v-for="permission in scope.row.list" v-bind:key="permission"
                     style="display: inline-block; padding: 5px">
                    <el-tag type="primary" :effect="permission.type" hit>
                        <strong>{{ permission.name }}</strong>
                    </el-tag>
                </div>
            </template>
        </el-table-column>
    </el-table>
</template>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "UserRolePermissionsTable",
        props: {
            userRole: Object
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ]),
            getPermissionsTable() {
                let getPermissionTagType = (permissionsValue, permissionValue) => {
                    return (permissionsValue & permissionValue) === 0 ? "plain" : "dark";
                };
                return [
                    {
                        type: "User Management",
                        list: this.getLookupValues("userPermissions").map(permission => {
                            return {
                                type: getPermissionTagType(this.userRole.userPermissions, permission.value),
                                name: permission.name
                            };
                        })
                    }
                ];
            }
        }
    };
</script>