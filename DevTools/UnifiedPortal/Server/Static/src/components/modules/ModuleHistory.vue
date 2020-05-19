<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed"
               @open="prepareModal">
        <span slot="title" class="modal-title">Module History</span>
        <el-container>
            <el-main v-loading="loadingIsActive">
                <el-timeline :reverse="true">
                    <el-timeline-item v-for="item in history" :key="item" :timestamp="item.time" placement="top"
                                      style="text-align: left">
                        <div style="font-size: 16px">
                            {{ item.change }}
                        </div>
                    </el-timeline-item>
                </el-timeline>
            </el-main>
        </el-container>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { GET_MODULE_HISTORY_RECORDS } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    export default {
        name: "ModuleHistory",
        props: {
            dialogStatus: Object,
            moduleId: Number,
            closed: Function
        },
        data() {
            return {
                loadingIsActive: true,
                history: []
            };
        },
        methods: {
            prepareModal() {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_MODULE_HISTORY_RECORDS, {
                        id: this.moduleId
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.history = response.data;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load module history",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            close() {
                this.history = [];
                this.closed();
            }
        }
    };
</script>