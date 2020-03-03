<template>
    <div>
        <Profile :header="table">
            <template slot="profile-buttons">
                <slot name="buttons"/>
            </template>
            <template slot="profile-content">
                <ProfileBlock icon="filter" header="Filters">
                    <template slot="profile-block-content">
                        <slot name="filters"/>
                        <el-row type="flex" justify="center" align="middle" :gutter="20" style="margin: 20px 0">
                            <el-col :span="12">
                                <el-button type="primary" style="width: 100%" @click="applyFiltersComplex">
                                    <strong>Apply</strong>
                                </el-button>
                            </el-col>
                            <el-col :span="12">
                                <el-button style="width: 100%" @click="resetFiltersComplex">
                                    <strong>Reset</strong>
                                </el-button>
                            </el-col>
                        </el-row>
                    </template>
                </ProfileBlock>
                <ProfileBlock icon="table" header="Table">
                    <template slot="profile-block-content">
                        <slot name="table"/>
                        <el-pagination style="width: 100%; margin-top: 20px; text-align: center"
                                       @size-change="changePageSize"
                                       @current-change="changePageNo"
                                       :current-page.sync="settings.pageNo"
                                       :page-sizes="[10, 25, 50, 100, 250, 500]"
                                       :page-size="settings.pageSize"
                                       :total="settings.total"
                                       layout="total, sizes, prev, pager, next"
                        />
                    </template>
                </ProfileBlock>
            </template>
        </Profile>
    </div>
</template>

<script>
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";

    export default {
        name: "FilterableTable",
        props: {
            table: String,
            applyFilters: Function,
            resetFilters: Function,
            reload: Function,
            settings: Object
        },
        components: {
            Profile,
            ProfileBlock
        },
        methods: {
            applyFiltersComplex() {
                this.settings.pageNo = 1;
                if (this.applyFilters) this.applyFilters();
                this.reload();
            },
            resetFiltersComplex() {
                this.settings.pageNo = 1;
                if (this.resetFilters) this.resetFilters();
                this.reload();
            },
            changePageSize(value) {
                this.settings.pageSize = value;
                this.settings.pageNo = 1;
                this.reload();
            },
            changePageNo(value) {
                this.settings.pageNo = value;
                this.reload();
            }
        }
    };
</script>