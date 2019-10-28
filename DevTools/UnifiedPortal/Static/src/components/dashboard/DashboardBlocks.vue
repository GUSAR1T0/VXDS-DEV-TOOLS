<template>
    <div>
        <el-row :gutter="20">
            <el-col :xs="24" :sm="24" :md="24" :lg="getColumnSize" :xl="getColumnSize">
                <slot name="first"></slot>
            </el-col>
            <el-col v-if="hasSecondSlot" :lg="getColumnSize" :xl="getColumnSize" class="hidden-md-and-down">
                <slot name="second"></slot>
            </el-col>
            <el-col v-if="hasThirdSlot" :lg="getColumnSize" :xl="getColumnSize" class="hidden-md-and-down">
                <slot name="third"></slot>
            </el-col>
        </el-row>
        <el-row v-if="hasSecondSlot" class="small-display-row hidden-lg-and-up">
            <el-col :xs="24" :sm="24" :md="24">
                <slot name="second"></slot>
            </el-col>
        </el-row>
        <el-row v-if="hasThirdSlot" class="small-display-row hidden-lg-and-up">
            <el-col :xs="24" :sm="24" :md="24">
                <slot name="third"></slot>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>
    .small-display-row {
        margin-top: 20px;
    }
</style>

<script>
    export default {
        name: "DashboardBlocks",
        computed: {
            hasSecondSlot() {
                return !!this.$slots["second"];
            },
            hasThirdSlot() {
                return !!this.$slots["third"];
            },
            getColumnSize() {
                if (this.hasSecondSlot && this.hasThirdSlot) {
                    return 8;
                }

                if (this.hasSecondSlot || this.hasThirdSlot) {
                    return 12;
                }

                return 24;
            }
        }
    };
</script>
