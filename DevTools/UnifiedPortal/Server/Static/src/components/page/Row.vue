<template>
    <div class="editable-row">
        <el-row type="flex" justify="center" align="middle" style="text-align: left">
            <el-col :span="12" class="hidden-lg-and-up">
                <b style="font-size: 18px">{{ name }}</b>
            </el-col>
            <el-col :span="!half ? 6 : 12" class="hidden-md-and-down">
                <b style="font-size: 18px">{{ name }}</b>
            </el-col>
            <el-col :span="12" class="hidden-lg-and-up">
                <div v-if="value !== undefined" style="font-size: 20px">
                    {{ value }}
                </div>
                <div style="padding-top: 20px" v-if="hasDescription || !!$slots['description']">
                    <el-divider v-if="hasDescription" class="divider-editable-row"/>
                    <slot name="description"/>
                </div>
            </el-col>
            <el-col :span="!half ? 18 : 12" class="hidden-md-and-down">
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
        name: "Row",
        props: {
            name: String,
            value: String,
            hasDescription: Boolean,
            half: Boolean
        }
    };
</script>