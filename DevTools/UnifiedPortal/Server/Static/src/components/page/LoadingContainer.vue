<template>
    <el-container v-loading.fullscreen.lock="loadingState"
                  element-loading-text="Loading"
                  element-loading-spinner="el-icon-loading"
                  element-loading-background="rgba(255, 255, 255, 0.75)"
                  element-loading-custom-class="main-loading-spinner-custom"
                  class="loading-container">
        <el-main>
            <slot name="content"></slot>
        </el-main>
    </el-container>
</template>

<style scoped>
    .loading-container {
        height: 100%;
        width: 100%;
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
