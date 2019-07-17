<template>
    <div class="header-divider">
        <el-divider>
            <div class="page-name">{{ name }}</div>
        </el-divider>
    </div>
</template>

<style scoped>
    .page-name {
        text-transform: uppercase;
        font-size: 16px;
    }
</style>

<script>
    export default {
        name: "HeaderDivider",
        props: {
            name: String
        }
    };
</script>
