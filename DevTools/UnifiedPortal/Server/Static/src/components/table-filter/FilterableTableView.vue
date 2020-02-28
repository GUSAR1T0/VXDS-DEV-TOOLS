<template>
    <div>
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
        <slot name="table"/>
        <el-pagination style="width: 100%; margin-top: 20px"
                       @size-change="changePageSize"
                       @current-change="changePageNo"
                       :current-page.sync="settings.pageNo"
                       :page-sizes="[10, 25, 50, 100, 250, 500]"
                       :page-size="settings.pageSize"
                       :total="settings.total"
                       layout="total, sizes, prev, pager, next"
        />
    </div>
</template>

<script>
    export default {
        name: "FilterableTable",
        props: {
            applyFilters: Function,
            resetFilters: Function,
            reload: Function,
            settings: Object
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