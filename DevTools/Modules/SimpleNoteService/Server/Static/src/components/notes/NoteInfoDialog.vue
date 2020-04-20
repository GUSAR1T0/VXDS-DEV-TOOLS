<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center">
        <span slot="title" class="modal-title">Note description</span>
        <div style="margin: 20px">
            <Row name="Title" :value="note.title"/>
            <Row name="Folder" :value="note.folderName"/>
            <Row name="Creator">
                <template slot="description">
                    <UserAvatarAndFullNameWithLink
                            :first-name="note.firstName"
                            :last-name="note.lastName"
                            :color="note.color"
                            :user-id="note.userId"
                    />
                </template>
            </Row>
            <Row name="Edit Date / Time" :value="note.editTime"/>
            <Row name="Projects">
                <template slot="description">
                    <div v-for="item in note.projects" :key="item.id">
                        <el-link
                                type="primary" :underline="false"
                                :href="`/project/${item.projectId}`"
                        >
                            <strong style="font-size: 16px">
                                {{ item.projectName }} ({{ item.projectAlias }})
                            </strong>
                        </el-link>
                    </div>
                    <div v-if="hasNoProjects">
                        <strong style="font-size: 16px">—</strong>
                    </div>
                </template>
            </Row>
        </div>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import Row from "@/components/page/Row";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";

    export default {
        name: "NoteInfoDialog",
        props: {
            dialogStatus: Object,
            note: Object
        },
        components: {
            Row,
            UserAvatarAndFullNameWithLink
        },
        computed: {
            hasNoProjects() {
                return !this.note.projects || this.note.projects.length <= 0;
            }
        }
    };
</script>