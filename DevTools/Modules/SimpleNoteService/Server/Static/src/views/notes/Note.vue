<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile :header="getHeaderName">
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To Folder Notes
                        </div>
                        <el-button type="info" plain circle @click="$router.push(`/?folderId=${folderId}`)"
                                   class="rounded-button">
                            <span><fa icon="sticky-note"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" v-if="noteId">
                        <div slot="content">
                            To Note Info
                        </div>
                        <el-button type="info" plain circle class="rounded-button"
                                   @click="dialogNoteInfoStatus.visible = true">
                            <span><fa icon="info-circle"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" v-if="noteId">
                        <div slot="content">
                            Move To Another Folder
                        </div>
                        <el-button type="primary" plain circle class="rounded-button"
                                   @click="openNoteChangeFolderDialog">
                            <span><fa icon="exchange-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" v-if="noteId">
                        <div slot="content">
                            Notify Users About This Note
                        </div>
                        <el-button type="primary" plain circle class="rounded-button"
                                   @click="notificationDialog.visible = true">
                            <span><fa icon="bell"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" v-if="noteId">
                        <div slot="content">
                            Delete This Note
                        </div>
                        <el-button type="danger" plain circle @click="openDeleteNoteDialog"
                                   class="rounded-button">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="profile-content">
                    <ProfileBlock header="Editor">
                        <template slot="profile-block-content">
                            <div class="editor">
                                <editor-menu-bar class="editor__menubar" :editor="editor"
                                                 v-slot="{ commands, isActive }">
                                    <div class="menubar">
                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.bold() }"
                                                @click="commands.bold"
                                        >
                                            Bold
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.italic() }"
                                                @click="commands.italic"
                                        >
                                            Italic
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.strike() }"
                                                @click="commands.strike"
                                        >
                                            Strike
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.underline() }"
                                                @click="commands.underline"
                                        >
                                            Underline
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.paragraph() }"
                                                @click="commands.paragraph"
                                        >
                                            Paragraph
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.heading({ level: 1 }) }"
                                                @click="commands.heading({ level: 1 })"
                                        >
                                            H1
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.heading({ level: 2 }) }"
                                                @click="commands.heading({ level: 2 })"
                                        >
                                            H2
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.heading({ level: 3 }) }"
                                                @click="commands.heading({ level: 3 })"
                                        >
                                            H3
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.heading({ level: 4 }) }"
                                                @click="commands.heading({ level: 4 })"
                                        >
                                            H4
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.heading({ level: 5 }) }"
                                                @click="commands.heading({ level: 5 })"
                                        >
                                            H5
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.heading({ level: 6 }) }"
                                                @click="commands.heading({ level: 6 })"
                                        >
                                            H6
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.bullet_list() }"
                                                @click="commands.bullet_list"
                                        >
                                            Bullet List
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                :class="{ 'is-active': isActive.ordered_list() }"
                                                @click="commands.ordered_list"
                                        >
                                            Ordered List
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                @click="commands.horizontal_rule"
                                        >
                                            Horizontal Rule
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                @click="commands.undo"
                                        >
                                            Undo
                                        </el-button>

                                        <el-button
                                                class="menubar__button"
                                                @click="commands.redo"
                                        >
                                            Redo
                                        </el-button>
                                    </div>
                                </editor-menu-bar>
                                <editor-content class="editor__content" :editor="editor"/>
                            </div>
                        </template>
                    </ProfileBlock>

                    <ProfileBlock header="Tags">
                        <template slot="profile-block-content">
                            <Row name="Projects">
                                <template slot="description">
                                    <el-select v-model="projectIds" multiple filterable remote reserve-keyword
                                               :remote-method="filterByProjectIds" style="width: 100%">
                                        <el-option v-for="item in projectIdOptions" :key="item.id"
                                                   :label="`${item.name} (${item.alias})`" :value="item.id"/>
                                    </el-select>
                                </template>
                            </Row>
                        </template>
                    </ProfileBlock>

                    <el-button type="primary" @click="saveNote" ref="saveNoteButton"
                               style="width: 100%; margin-top: 20px">
                        <strong>Save</strong>
                    </el-button>
                </template>
            </Profile>

            <NoteNotificationDialog
                    :dialog-status="notificationDialog"
                    :folder-id="folderId"
                    :note-id="noteId"
                    :closed="loadNote"
            />

            <NoteInfoDialog
                    :dialog-status="dialogNoteInfoStatus"
                    :note="note"
            />

            <ChangeFolderDialog
                    :folder-id="folderId"
                    :dialog-status="dialogNoteChangeFolderStatus"
                    :submit="submitNoteChangeFolder"
            />

            <ConfirmationDialog
                    :dialog-status="dialogNoteDeleteStatus"
                    :confirmation-text="dialogNoteDeleteStatus.confirmationText"
                    :cancel-click-action="() => dialogNoteDeleteStatus.visible = false"
                    :submit-click-action="deleteNote"/>
        </template>
    </LoadingContainer>
</template>

<style>
    .editor *.is-empty:nth-child(1)::before,
    .editor *.is-empty:nth-child(2)::before {
        content: attr(data-empty-text);
        color: #909399;
    }
</style>

<style scoped src="@/styles/button.css">
</style>

<style scoped>
    .menubar__button {
        margin-bottom: 10px;
    }

    .editor__menubar {
        text-align: center;
    }

    .editor__content {
        text-align: left;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { Doc, Editor, EditorContent, EditorMenuBar, Node } from "tiptap";
    import {
        Bold,
        BulletList,
        HardBreak,
        Heading,
        History,
        HorizontalRule,
        Italic,
        Link,
        ListItem,
        OrderedList,
        Placeholder,
        Strike,
        TodoItem,
        TodoList,
        Underline
    } from "tiptap-extensions";
    import format from "string-format";
    import { LOCALHOST, UNIFIED_PORTAL } from "@/constants/servers";
    import { DELETE_HTTP_REQUEST, GET_HTTP_REQUEST, POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import {
        CHANGE_NOTE_FOLDER_ENDPOINT,
        CREATE_NOTE_ENDPOINT,
        DELETE_NOTE_ENDPOINT,
        GET_FOLDER_LIST_ENDPOINT,
        GET_FOLDER_NAME_ENDPOINT,
        GET_NOTE_ENDPOINT,
        SEARCH_PROJECTS_ENDPOINT,
        UPDATE_NOTE_ENDPOINT
    } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import NoteNotificationDialog from "@/components/notes/NoteNotificationDialog";
    import NoteInfoDialog from "@/components/notes/NoteInfoDialog";
    import ChangeFolderDialog from "@/components/notes/ChangeFolderDialog";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import Row from "@/components/page/Row";

    class CustomDoc extends Doc {
        get schema() {
            return {
                content: "title block+"
            };
        }
    }

    class Title extends Node {
        get name() {
            return "title";
        }

        get schema() {
            return {
                content: "inline*",
                parseDOM: [ {
                    tag: "h1",
                    id: "title"
                } ],
                toDOM: () => [ "h1", {
                    id: "title"
                }, 0 ]
            };
        }
    }

    export default {
        name: "Note",
        components: {
            EditorContent,
            EditorMenuBar,
            LoadingContainer,
            NoteInfoDialog,
            NoteNotificationDialog,
            ChangeFolderDialog,
            ConfirmationDialog,
            Profile,
            ProfileBlock,
            Row
        },
        data() {
            return {
                editor: new Editor({
                    autoFocus: true,
                    extensions: [
                        new CustomDoc(),
                        new Title(),
                        new Placeholder({
                            showOnlyCurrent: false,
                            emptyNodeText: node => {
                                if (node.type.name === "title") {
                                    return "Write a note title... ";
                                }

                                return "Write your note text... ";
                            }
                        }),
                        new BulletList(),
                        new HardBreak(),
                        new Heading(),
                        new HorizontalRule(),
                        new ListItem(),
                        new OrderedList(),
                        new TodoItem(),
                        new TodoList(),
                        new Link(),
                        new Bold(),
                        new Italic(),
                        new Strike(),
                        new Underline(),
                        new History()
                    ],
                    onUpdate: ({getHTML}) => {
                        this.content = getHTML();
                    }
                }),
                loadingIsActive: false,
                folderId: null,
                noteId: null,
                note: {
                    title: null,
                    folderName: null
                },
                folderName: null,
                content: "<h1 id='title'></h1><p></p>",
                notificationDialog: {
                    visible: false
                },
                dialogNoteInfoStatus: {
                    visible: false
                },
                dialogNoteChangeFolderStatus: {
                    visible: false,
                    text: "",
                    folderId: null,
                    folders: [],
                    foldersLoading: false
                },
                dialogNoteDeleteStatus: {
                    visible: false,
                    confirmationText: null
                },
                projectIds: [],
                projectIdOptions: [],
                projectIdsSearchLoading: false
            };
        },
        computed: {
            ...mapGetters([
                "getUnifiedPortalHost"
            ]),
            getHeaderName() {
                if (!this.folderName) {
                    return "...";
                }
                return `Folder: "${this.folderName}" > Note: "${(this.noteId ? this.note.title : "Draft")}"`;
            }
        },
        methods: {
            loadNote() {
                if (this.noteId) {
                    this.loadingIsActive = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(GET_NOTE_ENDPOINT, {
                            folderId: this.folderId,
                            noteId: this.noteId
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.loadingIsActive = false;
                        this.note = response.data;
                        this.folderName = response.data.folderName;
                        this.content = "<h1 id='title'>" + this.note.title + "</h1>" + this.note.text;

                        this.projectIds = [];
                        this.projectIdOptions = [];
                        for (let i = 0; i < this.note.projects.length; i++) {
                            this.projectIds.push(this.note.projects[i].projectId);
                            this.projectIdOptions.push({
                                id: this.note.projects[i].projectId,
                                name: this.note.projects[i].projectName,
                                alias: this.note.projects[i].projectAlias
                            });
                        }

                        this.editor.setContent(this.content);
                    }).catch(error => {
                        this.loadingIsActive = false;
                        this.$notify.error({
                            title: "Failed to load note",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                        });
                    });
                } else {
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(GET_FOLDER_NAME_ENDPOINT, {
                            folderId: this.folderId
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.loadingIsActive = false;
                        this.folderName = response.data;
                    }).catch(error => {
                        this.loadingIsActive = false;
                        this.$notify.error({
                            title: "Failed to load folder name",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                        });
                    });
                }
            },
            openNoteChangeFolderDialog() {
                this.dialogNoteChangeFolderStatus.visible = true;
                this.dialogNoteChangeFolderStatus.text = `Note ID: "${this.note.id}", title: "${this.note.title}", current folder: "${this.folderName}"`;
                this.dialogNoteChangeFolderStatus.foldersLoading = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_FOLDER_LIST_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.dialogNoteChangeFolderStatus.foldersLoading = false;
                    this.dialogNoteChangeFolderStatus.folders = [ response.data ];
                }).catch(error => {
                    this.dialogNoteChangeFolderStatus.foldersLoading = false;
                    this.$notify.error({
                        title: "Failed to load list of folders",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            submitNoteChangeFolder(button) {
                button.loading = true;
                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(CHANGE_NOTE_FOLDER_ENDPOINT, {
                        folderId: this.folderId,
                        noteId: this.noteId,
                        newFolderId: this.dialogNoteChangeFolderStatus.folderId
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogNoteChangeFolderStatus.visible = false;
                    this.dialogNoteChangeFolderStatus.folders = [];
                    this.$notify.success({
                        title: "Folder was changed"
                    });

                    this.$router.replace({
                        name: "note",
                        params: {
                            folderId: this.dialogNoteChangeFolderStatus.folderId,
                            noteId: this.noteId
                        }
                    });
                }).catch(error => {
                    button.loading = false;
                    this.$notify.error({
                        title: "Failed to change note folder",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            openDeleteNoteDialog() {
                this.dialogNoteDeleteStatus.confirmationText = `Are you sure that you want to delete note "${this.note.title}"?`;
                this.dialogNoteDeleteStatus.visible = true;
            },
            deleteNote(button) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_NOTE_ENDPOINT, {
                        folderId: this.folderId,
                        noteId: this.noteId
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogNoteDeleteStatus.visible = false;

                    this.$notify.success({
                        title: "Note was deleted"
                    });

                    this.$router.push(`/?folderId=${this.folderId}`);
                }).catch(error => {
                    button.loading = false;
                    this.dialogNoteDeleteStatus.visible = false;

                    this.$notify.error({
                        title: "Failed to delete note",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            saveNote() {
                let parser = new DOMParser();
                let doc = parser.parseFromString(this.content, "text/html");

                let titleNode = doc.getElementById("title");
                let title = titleNode.innerText;

                if (!title) {
                    this.$notify.error({
                        title: "Enter the title of note",
                        duration: 10000
                    });
                } else {
                    titleNode.remove();
                    let text = doc.body.innerHTML;

                    this.$refs.saveNoteButton.loading = true;
                    if (this.noteId) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_NOTE_ENDPOINT, {
                                folderId: this.folderId,
                                noteId: this.noteId
                            }),
                            data: {
                                title: title,
                                text: text,
                                projectIds: this.projectIds
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.$refs.saveNoteButton.loading = false;

                            this.$notify.success({
                                title: "Note was updated"
                            });

                            this.loadNote();
                        }).catch(error => {
                            this.$refs.saveNoteButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update note",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                            });
                        });
                    } else {
                        this.$store.dispatch(POST_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(CREATE_NOTE_ENDPOINT, {
                                folderId: this.folderId
                            }),
                            data: {
                                title: title,
                                text: text,
                                projectIds: this.projectIds
                            },
                            config: getConfiguration()
                        }).then(response => {
                            this.$refs.saveNoteButton.loading = false;

                            this.$notify.success({
                                title: "Note was created"
                            });

                            this.$router.replace({
                                name: "note",
                                params: {
                                    folderId: this.folderId,
                                    noteId: response.data
                                }
                            });
                        }).catch(error => {
                            this.$refs.saveNoteButton.loading = false;

                            this.$notify.error({
                                title: "Failed to create note",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                            });
                        });
                    }
                }
            },
            filterByProjectIds(query) {
                if (query !== "") {
                    this.projectIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: UNIFIED_PORTAL,
                        endpoint: format(SEARCH_PROJECTS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.projectIdsSearchLoading = false;
                        this.projectIdOptions = response.data;
                    }).catch(error => {
                        this.projectIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of projects",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                        });
                    });
                } else {
                    this.projectIdOptions = [];
                }
            }
        },
        mounted() {
            this.folderId = this.$route.params.folderId;
            this.noteId = this.$route.params.noteId;
            this.loadNote();
        },
        beforeRouteUpdate(to, from, next) {
            this.folderId = to.params.folderId;
            this.noteId = to.params.noteId;
            this.loadNote();
            next();
        },
        beforeDestroy() {
            this.editor.destroy();
        }
    };
</script>
