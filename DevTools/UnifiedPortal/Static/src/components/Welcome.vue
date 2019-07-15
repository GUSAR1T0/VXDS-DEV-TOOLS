<template>
    <div class="welcome">
        <el-row type="flex" justify="center" align="middle">
            <el-col :sm="11" :md="11" :lg="11" :xl="11" class="hidden-xs-only">
                <h1 class="header-text">VXDESIGN.STORE</h1>
                <h3 class="header-text">DEVELOPMENT TOOLS</h3>
            </el-col>
            <el-col :sm="2" :md="2" :lg="2" :xl="2" class="hidden-xs-only">
                <el-divider direction="vertical"></el-divider>
            </el-col>
            <el-col :xs="24" :sm="11" :md="11" :lg="11" :xl="11">
                <h2 class="header-text">UNIFIED PORTAL</h2>
            </el-col>
        </el-row>
        <el-divider>
            <div class="page-header">{{ msg }}</div>
        </el-divider>
    </div>
</template>

<style scoped>
    .page-header {
        text-transform: uppercase;
        font-size: 16px;
    }

    .header-text {
        margin-block-start: 0 !important;
        margin-block-end: 0 !important;
    }

    .el-divider--vertical {
        height: auto !important;
        min-height: 64px !important;
    }
</style>

<script>
    export default {
        name: "Welcome",
        props: {
            msg: String
        }
    };
</script>
