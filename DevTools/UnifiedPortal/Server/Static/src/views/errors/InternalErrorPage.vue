<template>
    <ErrorPage
            :error-number="500"
            problem-title="Some issue was happened on client-side"
            description="Please, inform your administrator about that."
    />
</template>

<script>
    import ErrorPage from "@/components/errors/ErrorPage";

    export default {
        name: "InternalErrorPage",
        components: {
            ErrorPage
        }
    };
</script>