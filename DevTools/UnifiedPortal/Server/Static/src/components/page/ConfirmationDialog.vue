<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">Confirmation</span>
        <h1>{{ confirmationText }}</h1>
        <h3 class="confirmation-dialog-header">{{ additionalText }}</h3>
        <el-row type="flex" justify="center" align="middle" :gutter="20">
            <el-col :span="12">
                <el-button @click="cancelClickAction" class="confirmation-button">
                    <strong>Cancel</strong>
                </el-button>
            </el-col>
            <el-col :span="12">
                <el-button type="danger" plain ref="dialogButton" @click="submitClickAction($refs.dialogButton)"
                           class="confirmation-button">
                    <strong>Submit</strong>
                </el-button>
            </el-col>
        </el-row>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .confirmation-dialog-header {
        text-align: center;
        margin-bottom: 55px;
    }

    .confirmation-button {
        width: 100%;
    }
</style>

<script>
    export default {
        name: "ConfirmationDialog",
        props: {
            dialogStatus: Object,
            confirmationText: String,
            additionalText: String,
            cancelClickAction: Function,
            submitClickAction: Function,
            closed: Function
        }
    };
</script>