<template>
    <ErrorPage
            :error-number="403"
            :problem-title="getProblemTitle"
            description="If you have a problem, please, inform your administrator about that."
    />
</template>

<script>
    import ErrorPage from "@/components/errors/ErrorPage";

    export default {
        name: "ForbiddenErrorPage",
        components: {
            ErrorPage
        },
        data() {
            return {
                link: null
            };
        },
        computed: {
            getProblemTitle() {
                return `We couldn't give you access to ${this.link ? `"${this.link}"` : "the page"}`;
            }
        },
        mounted() {
            this.link = this.$route.query.from;
        },
        beforeRouteUpdate(to, from, next) {
            this.link = to.query.query.from;
            next();
        }
    };
</script>