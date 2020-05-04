import Vue from "vue";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
    faUserCircle,
    faUserAlt,
    faInfoCircle,
    faSignOutAlt,
    faSignInAlt,
    faUserPlus,
    faAlignJustify,
    faEdit,
    faTools,
    faUsers,
    faPlusCircle,
    faUsersCog,
    faFileAlt,
    faServer,
    faCodeBranch,
    faCode,
    faLock,
    faLockOpen,
    faTrashAlt,
    faAddressCard,
    faFilter,
    faTable,
    faStar,
    faEye,
    faTasks,
    faCopy,
    faCubes,
    faExclamationCircle,
    faExclamationTriangle,
    faCheckCircle,
    faTimesCircle,
    faFireAlt,
    faHistory,
    faBell,
    faHeartbeat,
    faCircle,
    faDatabase,
    faNetworkWired,
    faQuestionCircle,
    faPlug,
    faUpload,
    faCog,
    faBackspace,
    faPause,
    faForward
} from "@fortawesome/free-solid-svg-icons";
import { faGithub, faWindows, faLinux, faApple } from "@fortawesome/free-brands-svg-icons";
import {
    faCheckCircle as farCheckCircle,
    faTimesCircle as farTimesCircle
} from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(
    faUserCircle, faUserAlt, faInfoCircle, faSignOutAlt, faSignInAlt, faUserPlus, faAlignJustify, faEdit, faTools,
    faUsers, faPlusCircle, faUsersCog, faFileAlt, faServer, faGithub, faCodeBranch, faCode, faLock, faLockOpen,
    faTrashAlt, faAddressCard, faFilter, faTable, faStar, faEye, faTasks, faCopy, faCubes, faExclamationCircle,
    faExclamationTriangle, faFireAlt, faCheckCircle, faTimesCircle, faHistory, faBell, farCheckCircle, farTimesCircle,
    faHeartbeat, faCircle, faDatabase, faNetworkWired, faQuestionCircle, faWindows, faLinux, faApple, faPlug,
    faUpload, faCog, faBackspace, faPause, faForward
);

Vue.component("fa", FontAwesomeIcon);