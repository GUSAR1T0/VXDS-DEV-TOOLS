<template>
    <div class="horizontal-divider">
        <el-divider>
            <div class="divider-content">{{ name }}</div>
        </el-divider>
    </div>
</template>

<style scoped>
    .divider-content {
        text-transform: uppercase;
        font-size: 16px;
    }
</style>

<script>
    export default {
        name: "HorizontalDivider",
        props: {
            name: String
        }
    };
</script>
