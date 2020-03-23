import { getTokens } from "@/extensions/tokens";
import randomColor from "randomcolor";

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

export function generateColor() {
    return randomColor({
        luminosity: "dark",
        format: "rgba",
        alpha: 0.75
    });
}

export function getUserFullName(firstName, lastName) {
    return `${firstName ? firstName : "—"} ${lastName ? lastName : ""}`;
}

export function getUserInitials(firstName, lastName) {
    let first = firstName ? firstName.substr(0, 1) : "—";
    let last = lastName ? lastName.substr(0, 1) : "";
    return (first + last).toUpperCase();
}

function showOperationBlock(h, operationId) {
    if (!isNaN(operationId)) {
        return [
            h("el-link", {
                props: {
                    href: `/system/operations?id=${operationId}`,
                    type: "primary"
                }
            }, [
                h("strong", null, [
                    operationId,
                    h("i", {
                        class: [ "el-icon-top-right" ]
                    }, null)
                ])
            ])
        ];
    } else h("strong", null, operationId);
}

export function renderErrorNotificationMessage(h, response) {
    if (response && response.data) {
        return h("div", null, [
            "Response (", h("strong", null, response.status), "): ",
            h("div", {style: "margin-top: 5px; margin-left: 5px"}, [
                h("strong", null, response.data.message)
            ]),
            h("div", {style: "margin-top: 5px"}, "Code of incident:"),
            h("div", {style: "margin-top: 5px; margin-left: 5px"}, [
                showOperationBlock(h, response.data.operationId)
            ])
        ]);
    } else {
        return "Unhandled exception";
    }
}

export function getOnlyNumbers(array) {
    return array.map(String).filter(value => !isNaN(value)).map(value => parseInt(value.replace(/\D/g, ""))).map(Number);
}

export function getDate(date) {
    let transformer = (value) => ("0" + value).slice(-2);
    if (date) {
        let year = date.getFullYear();
        let month = date.getMonth() + 1;
        let day = date.getDate();
        let hour = date.getHours();
        let minute = date.getMinutes();
        let second = date.getSeconds();
        return `${year}-${transformer(month)}-${transformer(day)}T${transformer(hour)}:${transformer(minute)}:${transformer(second)}`;
    } else {
        return null;
    }
}
