<template>
    <div>
        <div style="font-size: 20px">
            <fa :icon="['fab', getOperationSystemIcon(operationSystem)]"/>
        </div>
        <div style="font-size: 22px; padding: 5px">
            <strong>{{ name }}</strong>
        </div>
        <div style="font-size: 16px">
            <strong>Domain:</strong> {{ domain }}
        </div>
    </div>
</template>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "HostFullName",
        props: {
            name: String,
            domain: String,
            operationSystem: [ Number, String ]
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            getOperationSystemIcon(osId) {
                let osList = this.getLookupValues("hostOperationSystems").filter(os => parseInt(os.value) === parseInt(osId));
                if (osList && osList.length > 0) {
                    let os = osList[0];
                    if (os.value === "1") {
                        return "windows";
                    } else if (os.value === "2") {
                        return "linux";
                    } else if (os.value === "3") {
                        return "apple";
                    }
                } else {
                    return "question-circle";
                }
            }
        }
    };
</script>