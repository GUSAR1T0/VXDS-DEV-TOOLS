<template>
    <div class="editable-row">
        <el-row type="flex" justify="center" align="middle">
            <el-col :span="6" style="text-align: left;">
                <b style="font-size: 18px">{{ name }}</b>
            </el-col>
            <el-col :span="18" style="text-align: left;">
                <div v-if="value !== undefined" style="font-size: 20px">
                    {{ value }}
                </div>
                <div style="padding-top: 20px" v-if="hasDescription || !!$slots['description']">
                    <el-divider v-if="hasDescription" class="divider-editable-row"/>
                    <slot name="description"/>
                </div>
            </el-col>
        </el-row>
        <el-divider class="divider-editable-row"/>
    </div>
</template>

<style scoped>
    .divider-editable-row {
        background-color: rgba(220, 223, 230, .50) !important;
    }
</style>

<script>
    export default {
        name: "UserInfoRow",
        props: {
            name: String,
            value: String,
            hasDescription: Boolean
        }
    };
</script>