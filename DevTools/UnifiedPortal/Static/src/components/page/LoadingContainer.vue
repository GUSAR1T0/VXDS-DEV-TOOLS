<template>
    <el-container v-loading="loadingState"
                  element-loading-text="Loading"
                  element-loading-spinner="el-icon-loading"
                  element-loading-background="rgba(255, 255, 255, 0.75)"
                  element-loading-custom-class="main-loading-spinner-custom">
        <el-main>
            <slot name="content"></slot>
        </el-main>
    </el-container>
</template>

<style scoped>
    .user-container {
        margin-top: -17px;
    }
</style>

<script>
    export default {
        name: "LoadingContainer",
        props: {
            loadingState: Boolean
        }
    };
</script>
