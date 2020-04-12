<template>
    <div>
        <el-table :data="notes" style="width: 100%" border>
            <el-table-column label="Note ID" min-width="150" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="Manage Note" min-width="200" align="center">
                <template slot-scope="scope">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To This Note
                        </div>
                        <el-button type="info" plain circle @click="$router.push(`/note/${scope.row.id}`)"
                                   class="rounded-button">
                            <span><fa icon="sticky-note"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Delete This Note
                        </div>
                        <el-button type="danger" plain circle @click="openDeleteNoteDialog(scope.row)"
                                   class="rounded-button" :ref="`deleteNoteButton-${scope.row.id}`">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
            </el-table-column>
            <el-table-column label="Title" min-width="500" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 20px">
                        {{ scope.row.title }}
                    </strong>
                    <div style="font-size: 16px">
                        {{ scope.row.text }}
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Projects" min-width="500" align="center">
                <template slot-scope="scope">
                    <el-link
                            type="primary" :underline="false"
                            v-for="item in scope.row.projects" :key="item.id"
                            :href="`${getUnifiedPortalHost}/components/project/${item.projectId}`"
                    >
                        <strong style="font-size: 16px">
                            {{ item.projectName }} ({{ item.projectAlias }})
                        </strong>
                    </el-link>
                    <div v-if="hasNoProjects(scope.row)">
                        <strong style="font-size: 16px">—</strong>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Created by User" min-width="300" align="center">
                <template slot-scope="scope">
                    <UserAvatarAndFullNameWithLink
                            style="text-align: left"
                            :first-name="scope.row.firstName"
                            :last-name="scope.row.lastName"
                            :color="scope.row.color"
                            :user-id="scope.row.userId"
                    />
                </template>
            </el-table-column>
            <el-table-column label="Edit Date / Time" min-width="400" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        {{ scope.row.editTime }}
                    </div>
                </template>
            </el-table-column>
        </el-table>

        <ConfirmationDialog
                :dialog-status="dialogNoteDeleteStatus"
                :confirmation-text="dialogNoteDeleteStatus.confirmationText"
                :additional-text="dialogNoteDeleteStatus.additionalText"
                :cancel-click-action="() => dialogNoteDeleteStatus.visible = false"
                :submit-click-action="deleteNote"
                :closed="reload"/>
    </div>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { LOCALHOST } from "@/constants/servers";
    import { DELETE_HTTP_REQUEST } from "@/constants/actions";
    import { DELETE_NOTE_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import ConfirmationDialog from "@/components/page/ConfirmationDialog";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";

    export default {
        name: "NotesTable",
        props: {
            notes: Array,
            reload: Function
        },
        components: {
            ConfirmationDialog,
            UserAvatarAndFullNameWithLink
        },
        data() {
            return {
                dialogNoteDeleteStatus: {
                    id: null,
                    confirmationText: "",
                    additionalText: "",
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "getUnifiedPortalHost"
            ])
        },
        methods: {
            openDeleteNoteDialog(note) {
                this.dialogNoteDeleteStatus.id = note.id;
                this.dialogNoteDeleteStatus.confirmationText = "Are you sure that you want to delete the note?";
                this.dialogNoteDeleteStatus.additionalText = `ID: "${note.id}", title: "${note.title}"`;
                this.dialogNoteDeleteStatus.visible = true;
            },
            deleteNote(button) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_NOTE_ENDPOINT, {
                        id: this.dialogNoteDeleteStatus.id
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogNoteDeleteStatus.visible = false;
                    this.reload();

                    this.$notify.success({
                        title: "Note was deleted",
                        message: `Note with ID "${this.dialogNoteDeleteStatus.id}" was removed`
                    });
                    this.dialogNoteDeleteStatus.id = null;
                    this.dialogNoteDeleteStatus.confirmationText = "";
                    this.dialogNoteDeleteStatus.additionalText = "";
                }).catch(error => {
                    button.loading = false;
                    this.dialogNoteDeleteStatus.visible = false;

                    this.$notify.error({
                        title: "Failed to delete notification",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            hasNoProjects(row) {
                return !row.projects || row.projects.length <= 0;
            }
        }
    };
</script>