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
export const UPDATE_PROFILE_GENERAL_INFO_ENDPOINT = "user/{id}/general";
export const UPDATE_PROFILE_ACCOUNT_SPECIFIC_INFO_ENDPOINT = "user/{id}/accountSpecific";

// User roles endpoints
export const GET_USER_ROLES_SHORT_INFO_ENDPOINT = "userRole/short/list";
export const GET_USER_ROLES_FULL_INFO_ENDPOINT = "userRole/full/list";
