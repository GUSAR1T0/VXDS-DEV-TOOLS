<template>
    <div class="footer">
        Copyright © {{getYears()}} VXDESIGN.STORE: Development Tools | Designed by Roman Mashenkin
    </div>
</template>

<script>
    export default {
        name: "Footer",
        methods: {
            getYears() {
                let firstYear = 2019;
                let currentYear = new Date().getFullYear();
                return `${firstYear + (currentYear !== firstYear ? `-${currentYear}` : "")}`;
            }
        }
    };
</script>