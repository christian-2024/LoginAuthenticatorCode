import axios from "axios";
import API_BASE_URL from "./ConfigUrlService";
import { Logout, loginConfirmedAction } from '../store/action/AuthActions';

export async function login(email, password){
    const postData = {
        email: email,
        password: password,
        returnSecureToken: true,
    };
    return await axios.post(`${API_BASE_URL}/api/User/login`, postData);
}

export function saveTokenInLocalStorage(tokenDetails){
    tokenDetails.expireDate = new Date(new Date().getTime() + 8 * 60 * 60 * 1000); /** esse vai colocar 8 horas para frente */

    // JWT para UTF-8
    const base64Payload = tokenDetails.token.split(".")[1];
    const jsonPayload = decodeURIComponent(
        atob(base64Payload)
        .split("")
        .map((c) => {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join("")
    );

    const tokenPayload = JSON.parse(jsonPayload);
    const userRole = tokenPayload.role || "Não tem autorização";
    localStorage.setItem("userDetails",
        JSON.stringify({
            ...tokenDetails,
            role: userRole,
            id: parseInt(tokenPayload.nameid),
            name: tokenPayload.unique_name,
        })
    );
}

export function runLogoutTimer(dispatch, timer, navigate){
    setTimeout(() => {
        dispatch(Logout(navigate));
    }, timer);
}


