import { PREPARE_INCIDENT_FORM, RESET_INCIDENT_STORE_STATE, STORE_INCIDENT_DATA_REQUEST } from "@/constants/actions";
import { UNASSIGNED } from "@/constants/formatPattern";
import { getUserFullName } from "@/extensions/utils";

export default {
    state: {
        incident: {
            operation: {
                id: -1,
                scope: "",
                contextName: "",
                isSuccessful: null,
                firstName: "",
                lastName: "",
                color: "",
                userId: null,
                startTime: "",
                stopTime: "",
                logs: []
            },
            exists: false,
            authorId: 0,
            authorColor: "",
            authorFirstName: "",
            authorLastName: "",
            assigneeId: 0,
            assigneeColor: "",
            assigneeFirstName: UNASSIGNED,
            assigneeLastName: "",
            initialTime: "",
            status: "1",
            comment: "",
            history: []
        },
        incidentForm: {}
    },
    getters: {
        getIncident: state => {
            return state.incident;
        },
        getIncidentForm: state => {
            return state.incidentForm;
        },
        getIncidentAuthorOptions: state => {
            return state.incident.authorId > -1 ? [ {
                id: state.incident.authorId,
                fullName: getUserFullName(state.incident.authorFirstName, state.incident.authorLastName)
            } ] : [];
        },
        getIncidentAssigneeOptions: state =>  {
            return state.incident.assigneeId > -1 ? [ {
                id: state.incident.assigneeId,
                fullName: getUserFullName(state.incident.assigneeFirstName, state.incident.assigneeLastName)
            } ] : [];
        }
    },
    mutations: {
        [STORE_INCIDENT_DATA_REQUEST]: (state, data) => {
            state.incident.operation.id = data.operation ? data.operation.id : -1;
            state.incident.operation.scope = data.operation ? data.operation.scope : "";
            state.incident.operation.contextName = data.operation ? data.operation.contextName : "";
            state.incident.operation.isSuccessful = data.operation ? data.operation.isSuccessful : null;
            state.incident.operation.firstName = data.operation ? data.operation.firstName : "";
            state.incident.operation.lastName = data.operation ? data.operation.lastName : "";
            state.incident.operation.color = data.operation ? data.operation.color : "";
            state.incident.operation.userId = data.operation ? data.operation.userId : null;
            state.incident.operation.startTime = data.operation ? data.operation.startTime : "";
            state.incident.operation.stopTime = data.operation ? data.operation.stopTime : "";
            state.incident.operation.logs = data.history ? data.operation.logs : [];
            state.incident.exists = data.exists;
            state.incident.authorId = data.authorId;
            state.incident.authorColor = data.authorColor;
            state.incident.authorFirstName = data.authorFirstName;
            state.incident.authorLastName = data.authorLastName;
            state.incident.assigneeId = data.assigneeId ? data.assigneeId : 0;
            state.incident.assigneeColor = data.assigneeColor;
            state.incident.assigneeFirstName = data.assigneeFirstName ? data.assigneeFirstName : UNASSIGNED;
            state.incident.assigneeLastName = data.assigneeLastName;
            state.incident.initialTime = data.initialTime;
            state.incident.status = data.status ? data.status : "1";
            state.incident.comment = data.comment ? data.comment : "";
            state.incident.history = data.history ? data.history : [];
        },
        [PREPARE_INCIDENT_FORM]: state => {
            state.incidentForm = JSON.parse(JSON.stringify({
                operation: state.incident.operation,
                authorId: state.incident.authorId,
                assigneeId: state.incident.assigneeId,
                status: state.incident.status,
                comment: state.incident.comment
            }));
        },
        [RESET_INCIDENT_STORE_STATE]: state => {
            state.incident = {
                operation: {
                    id: -1,
                    scope: "",
                    contextName: "",
                    isSuccessful: null,
                    firstName: "",
                    lastName: "",
                    color: "",
                    userId: null,
                    startTime: "",
                    stopTime: "",
                    logs: []
                },
                exists: false,
                authorId: 0,
                authorColor: "",
                authorFirstName: "",
                authorLastName: "",
                assigneeId: 0,
                assigneeColor: "",
                assigneeFirstName: UNASSIGNED,
                assigneeLastName: "",
                initialTime: "",
                status: "1",
                comment: "",
                history: []
            };
            state.incidentForm = {};
        }
    }
};
