<template>
    <div>
        <div style="width: 100%; text-align: left; padding-bottom: 10px">
            <strong style="text-align: left; margin-bottom: 10px">{{ name }}</strong>
        </div>
        <div style="text-align: center">
            <slot name="field"/>
        </div>
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