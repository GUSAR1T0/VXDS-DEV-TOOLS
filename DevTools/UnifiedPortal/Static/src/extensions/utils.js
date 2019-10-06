import { getTokens } from "@/extensions/tokens";

export function getYearsForFooter() {
    let firstYear = 2019;
    let currentYear = new Date().getFullYear();
    return `${firstYear + (currentYear !== firstYear ? `-${currentYear}` : "")}`;
}

export function getConfiguration(notSavedAccessToken = null) {
    let headers = {};

    const setToken = (token) => {
        headers["Authorization"] = `Bearer ${token}`;
    };
    if (notSavedAccessToken) {
        setToken(notSavedAccessToken);
    } else {
        let {accessToken} = getTokens();
        if (accessToken) {
            setToken(accessToken);
        }
    }

    return {headers};
}
