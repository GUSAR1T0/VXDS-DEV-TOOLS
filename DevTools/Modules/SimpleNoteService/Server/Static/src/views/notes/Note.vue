<template>
    <div>
        <editor-menu-bar :editor="editor" v-slot="{ commands, isActive }">
            <button :class="{ 'is-active': isActive.bold() }" @click="commands.bold">
                Bold
            </button>
        </editor-menu-bar>
        <editor-content :editor="editor"/>
    </div>
</template>

<script>
    import { Editor, EditorContent, EditorMenuBar } from "tiptap";
    import { Bold, Italic, Link, HardBreak, Heading } from "tiptap-extensions";

    export default {
        name: "Note",
        components: {
            EditorContent,
            EditorMenuBar
        },
        data() {
            return {
                editor: new Editor({
                    content: "<p>This is just a boring paragraph</p>",
                    extensions: [
                        new Bold(),
                        new Italic(),
                        new Link(),
                        new HardBreak(),
                        new Heading()
                    ]
                })
            };
        },
        beforeDestroy() {
            this.editor.destroy();
        }
    };
</script>
