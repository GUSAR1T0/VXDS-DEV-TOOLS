import { getTokens } from "@/extensions/tokens";

export function getYearsForFooter() {
    let firstYear = 2019;
    let currentYear = new Date().getFullYear();
    return `${firstYear + (currentYear !== firstYear ? `-${currentYear}` : "")}`;
}

export function getHeaders(accessToken = null) {
    let headers = {};

    const setToken = (token) => {
        headers["Authorization"] = `Bearer ${token}`;
    };
    if (accessToken) {
        setToken(accessToken);
    } else {
        let {savedAccessToken} = getTokens();
        if (savedAccessToken) {
            setToken(savedAccessToken);
        }
    }

    return {headers};
}
