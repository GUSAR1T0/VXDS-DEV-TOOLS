<template>
    <div>
        <el-collapse value="system">
            <el-collapse-item name="system">
                <template slot="title">
                    <h3>System</h3>
                </template>
                <el-row :gutter="20">
                    <el-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
                        <UserRolesCard/>
                    </el-col>
                    <el-col :lg="8" :xl="8" class="hidden-md-and-down">
                        <LogsCard/>
                    </el-col>
                    <el-col :lg="8" :xl="8" class="hidden-md-and-down">
                        <el-card shadow="hover">
                            <div slot="header">
                                <h3>OMT</h3>
                            </div>
                            ...
                        </el-card>
                    </el-col>
                </el-row>
                <el-row class="row hidden-lg-and-up">
                    <el-col :xs="24" :sm="24" :md="24">
                        <LogsCard/>
                    </el-col>
                </el-row>
                <el-row class="row hidden-lg-and-up">
                    <el-col :xs="24" :sm="24" :md="24">
                        <el-card shadow="hover">
                            <div slot="header">
                                <h3>OMT</h3>
                            </div>
                            ...
                        </el-card>
                    </el-col>
                </el-row>
            </el-collapse-item>
        </el-collapse>
    </div>
</template>

<style scoped>
    .row {
        margin-top: 20px;
    }
</style>

<script>
    import UserRolesCard from "@/components/admin/UserRolesCard";
    import LogsCard from "@/components/admin/LogsCard";

    export default {
        name: "AdminPanel",
        components: {
            UserRolesCard,
            LogsCard
        }
    };
</script>