<template>
    <DashboardCardWithTotal
            :total="modulesTotal"
            entityName="Modules"
            entityIcon="cubes"
            linkToEntity="/components/modules"
            :load="load"
    >
        <template slot>
            <div style="font-size: 18px">
                <strong>Not implemented</strong>
            </div>
        </template>
    </DashboardCardWithTotal>
</template>

<script>
    import DashboardCardWithTotal from "@/components/dashboard/DashboardCardWithTotal";

    export default {
        name: "ModulesCard",
        components: {
            DashboardCardWithTotal
        },
        computed: {
            modulesTotal() {
                return 0;
            }
        },
        methods: {
            load(state) {
                state.loadingIsActive = true;

                setTimeout(() => {
                    state.loadingIsActive = false;
                }, 1000);
            }
        }
    };
</script>