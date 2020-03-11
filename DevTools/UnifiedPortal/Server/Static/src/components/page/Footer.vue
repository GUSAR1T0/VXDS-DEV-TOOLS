<template>
    <div class="footer">
        Copyright © {{ year }} VXDESIGN.STORE: Development Tools | Designed by Roman Mashenkin
    </div>
</template>

<script>
    import { getYearsForFooter } from "@/extensions/utils";

    export default {
        name: "Footer",
        computed: {
            year: getYearsForFooter
        }
    };
</script>