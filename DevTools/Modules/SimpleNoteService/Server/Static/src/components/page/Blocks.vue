<template>
    <div>
        <el-row type="flex" justify="center" align="middle" :gutter="20">
            <el-col :xs="24" :sm="24" :md="24" :lg="getColumnSize" :xl="getColumnSize">
                <slot name="first"/>
            </el-col>
            <el-col v-if="hasSecondSlot" :lg="getColumnSize" :xl="getColumnSize" class="hidden-md-and-down">
                <slot name="second"/>
            </el-col>
            <el-col v-if="hasThirdSlot" :lg="getColumnSize" :xl="getColumnSize" class="hidden-md-and-down">
                <slot name="third"/>
            </el-col>
        </el-row>
        <el-row v-if="hasSecondSlot" type="flex" justify="center" align="middle"
                class="small-display-row hidden-lg-and-up">
            <el-col :xs="24" :sm="24" :md="24">
                <slot name="second"/>
            </el-col>
        </el-row>
        <el-row v-if="hasThirdSlot" type="flex" justify="center" align="middle"
                class="small-display-row hidden-lg-and-up">
            <el-col :xs="24" :sm="24" :md="24">
                <slot name="third"/>
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
        name: "Blocks",
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
