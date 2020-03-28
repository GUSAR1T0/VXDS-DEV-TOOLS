import { PREPARE_NOTIFICATION_FORM, RESET_NOTIFICATION_STORE_STATE, STORE_NOTIFICATION_DATA_REQUEST } from "@/constants/actions";

export default {
    state: {
        notification: {
            id: null,
            message: "",
            level: "1",
            startDateTime: "",
            stopDateTime: ""
        },
        notificationForm: {}
    },
    getters: {
        getNotification: state => {
            return state.notification;
        },
        getNotificationForm: state => {
            return state.notificationForm;
        }
    },
    mutations: {
        [STORE_NOTIFICATION_DATA_REQUEST]: (state, notification) => {
            state.notification.id = notification ? notification.id : null;
            state.notification.message = notification ? notification.message : "";
            state.notification.level = notification ? `${notification.level}` : "1";
            state.notification.startDateTime = notification ? notification.startDateTime : "";
            state.notification.stopDateTime = notification ? notification.stopDateTime : "";
        },
        [PREPARE_NOTIFICATION_FORM]: state => {
            state.notificationForm = JSON.parse(JSON.stringify({
                id: state.notification.id,
                message: state.notification.message,
                level: state.notification.level,
                startDateTime: state.notification.startDateTime,
                stopDateTime: state.notification.stopDateTime
            }));
        },
        [RESET_NOTIFICATION_STORE_STATE]: state => {
            state.notification = {
                id: null,
                message: "",
                level: "1",
                startDateTime: "",
                stopDateTime: ""
            };
            state.notificationForm = {};
        }
    }
};
