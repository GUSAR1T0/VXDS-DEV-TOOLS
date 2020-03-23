<template>
    <div>
        <el-row type="flex" justify="center" align="middle" :gutter="20">
            <el-col :xs="24" :sm="24" :md="24" :lg="getColumnSize" :xl="getColumnSize">
                <slot name="first"/>
            </el-col>
            <el-col v-if="hasSecondSlot" :lg="getColumnSize" :xl="getColumnSize" class="hidden-md-and-down">
                <slot name="second"/>
            </el-col>
        </el-row>
        <el-row v-if="hasSecondSlot" type="flex" justify="center" align="middle"
                class="hidden-lg-and-up">
            <el-col :xs="24" :sm="24" :md="24">
                <slot name="second"/>
            </el-col>
        </el-row>
    </div>
</template>

<script>
    export default {
        name: "DoubleRow",
        computed: {
            hasSecondSlot() {
                return !!this.$slots["second"];
            },
            getColumnSize() {
                return this.hasSecondSlot ? 12 : 24;
            }
        }
    };
</script>
