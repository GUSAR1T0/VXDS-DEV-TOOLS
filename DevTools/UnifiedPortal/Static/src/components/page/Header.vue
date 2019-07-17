<template>
    <div class="header">
        <el-row type="flex" justify="center" align="middle">
            <el-col :sm="11" :md="11" :lg="11" :xl="11" class="hidden-xs-only">
                <h3 class="header-text">VXDESIGN.STORE</h3>
                <h5 class="header-text">DEVELOPMENT TOOLS</h5>
            </el-col>
            <el-col :sm="2" :md="2" :lg="2" :xl="2" class="hidden-xs-only">
                <el-divider direction="vertical"></el-divider>
            </el-col>
            <el-col :xs="24" :sm="11" :md="11" :lg="11" :xl="11">
                <h4 class="header-text">UNIFIED PORTAL</h4>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>
    .header {
        text-align: center;
    }

    .header-text {
        margin-block-start: 0 !important;
        margin-block-end: 0 !important;
    }

    .el-divider--vertical {
        height: auto !important;
        min-height: 48px !important;
    }
</style>

<script>
    export default {
        name: "Header"
    };
</script>