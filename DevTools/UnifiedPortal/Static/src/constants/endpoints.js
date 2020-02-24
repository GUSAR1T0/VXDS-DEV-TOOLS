// Account endpoints
export const SIGN_IN_ENDPOINT = "account/sign-in";
export const SIGN_UP_ENDPOINT = "account/sign-up";
export const REFRESH_ENDPOINT = "account/refresh";
export const LOGOUT_ENDPOINT = "account/logout";
export const GET_USER_DATA_ENDPOINT = "account";

// Lookup endpoints
export const GET_LOOKUP_VALUES_ENDPOINT = "lookup/values";

// User endpoints
export const GET_USERS_ENDPOINT = "user/list";
export const GET_PROFILE_ENDPOINT = "user/{id}";
export const ACTIVATE_USER_ENDPOINT = "user/{id}/activate";
export const DEACTIVATE_USER_ENDPOINT = "user/{id}/deactivate";
export const UPDATE_PROFILE_GENERAL_INFO_ENDPOINT = "user/{id}/general";
export const UPDATE_PROFILE_ACCOUNT_SPECIFIC_INFO_ENDPOINT = "user/{id}/accountSpecific";
export const SEARCH_USERS_ENDPOINT = "user/search?p={pattern}";

// User roles endpoints
export const GET_USER_ROLES_SHORT_INFO_ENDPOINT = "userRole/short/list";
export const GET_USER_ROLES_FULL_INFO_ENDPOINT = "userRole/full/list";
export const ADD_USER_ROLE_ENDPOINT = "userRole";
export const UPDATE_USER_ROLE_ENDPOINT = "userRole/{id}";
export const DELETE_USER_ROLE_ENDPOINT = "userRole/{id}";
export const GET_AFFECTED_USERS_COUNT_BY_USER_ROLE_ENDPOINT = "userRole/{id}/affectedUsers/count";
export const SEARCH_USER_ROLES_ENDPOINT = "userRole/search?p={pattern}";

// Admin panel endpoints
export const GET_DATA_FOR_DASHBOARD_ENDPOINT = "dashboard";
export const GET_OPERATION_LIST_ENDPOINT = "operation/list";

// Settings endpoints
export const LOAD_SETTINGS_ENDPOINT = "settings";
export const SETUP_GITHUB_TOKEN_ENDPOINT = "settings/codeService/github?t={token}";

// Projects endpoints
export const GET_PROJECTS_ENDPOINT = "project/list";
export const SEARCH_GITHUB_REPOSITORIES_ENDPOINT = "project/github/search?p={pattern}";
