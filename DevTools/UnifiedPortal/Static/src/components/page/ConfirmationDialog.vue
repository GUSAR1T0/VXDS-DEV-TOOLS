<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">Confirmation</span>
        <h1 class="confirmation-dialog-header">{{ confirmationText }}</h1>
        <el-row type="flex" justify="center" align="middle" :gutter="20">
            <el-col :span="12">
                <el-button type="danger" @click="cancelClickAction" plain style="width: 100%">
                    Cancel
                </el-button>
            </el-col>
            <el-col :span="12">
                <el-button type="danger" ref="dialogButton" @click="submitClickAction($refs.dialogButton)" style="width: 100%">
                    Submit
                </el-button>
            </el-col>
        </el-row>
    </el-dialog>
</template>

<style scoped>
    .confirmation-dialog-header {
        text-align: center;
        margin-bottom: 55px;
    }
</style>

<script>
    export default {
        name: "ConfirmationDialog",
        props: {
            dialogStatus: Object,
            confirmationText: String,
            cancelClickAction: Function,
            submitClickAction: Function,
            closed: Function
        }
    };
</script>