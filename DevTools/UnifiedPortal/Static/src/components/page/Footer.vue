<template>
    <div class="footer">
        Copyright © {{ year }} VXDESIGN.STORE: Development Tools | Designed by Roman Mashenkin
    </div>
</template>

<script>
    import getYearForFooter from "@/extensions/getYearForFooter";

    export default {
        name: "Footer",
        computed: {
            year: getYearForFooter
        }
    };
</script>