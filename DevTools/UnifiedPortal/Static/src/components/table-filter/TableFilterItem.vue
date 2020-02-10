<template>
    <div>
        <div style="width: 100%; text-align: left; padding-bottom: 10px">
            <strong style="text-align: left; margin-bottom: 10px">{{ name }}</strong>
        </div>
        <slot name="field"/>
    </div>
</template>

<script>
    export default {
        name: "TableFilterItem",
        props: {
            name: String
        }
    };
</script>