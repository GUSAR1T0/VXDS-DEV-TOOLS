<template>
    <div class="horizontal-divider">
        <el-divider>
            <div class="divider-content">{{ name }}</div>
        </el-divider>
    </div>
</template>

<style scoped src="@/styles/custom-divider.css">
</style>

<script>
    export default {
        name: "HorizontalDivider",
        props: {
            name: String
        }
    };
</script>
