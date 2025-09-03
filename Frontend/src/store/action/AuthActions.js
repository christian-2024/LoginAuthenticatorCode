import React from "react";
import { saveTokenInLocalStorage, login, runLogoutTimer } from "../../services/AuthService";

export const SIGNUP_CONFIRMED_ACTION = "[signup action] confirmed signup";
export const SIGNUP_FAILED_ACTION = "[signup action] failed signup";
export const LOADING_TOGGLE_ACTION = "[Loading action] toggle loading";
export const LOGIN_CONFIRMED_ACTION = "[Login action] confirmed login";
export const LOGOUT_ACTION = "[Logout action] logout action";
export const LOGIN_FAILED_ACTION = "[login action] failed login";

export function Logout(navigate){
    localStorage.removeItem("userDetails");
    navigate("/login");
    return{
        type: LOGOUT_ACTION,
    };
}

export function loginAction(email, password, navigate){
    return (dispatch) => {
        login(email, password) // esse seria o axios que vai fazer a conexão ao banco de dados
        .then((response) => {
            saveTokenInLocalStorage(response.data);
            runLogoutTimer(dispatch, 8 * 60 * 60 * 1000, navigate);
            dispatch(loginConfirmedAction(response.data));

            const userDetailsString = localStorage.getItem("userDetails");
            if(userDetailsString){
              const userDetails = JSON.parse(userDetailsString);
              const role = userDetails.role;
                if (role === "Admin"){
                    navigate("/dashboard");
                }
            } else {
                navigate("/login")
            }
        })
        .catch((error) => {
          console.error('Login filed: ', error.response.data);
          //teste de servidor
          if(error.response && error.response.data){
            const errorMessage = formatError(error.response.data);
            dispatch(loginFailedAction(errorMessage));
          } else {
            dispatch(loginFailedAction("Ocorreu um erro na requisição."));
          }
            
        
      });
    };
}
export function loginConfirmedAction(data){
    return {
        type: LOGIN_CONFIRMED_ACTION,
        payload: data,
    }
}

export function confirmedSignupAction(payload) {
  return {
    type: SIGNUP_CONFIRMED_ACTION,
    payload,
  };
}

export function signupFailedAction(message) {
  return {
    type: SIGNUP_FAILED_ACTION,
    payload: message,
  };
}

export function loadingToggleAction(status) {
    return {
        type: LOADING_TOGGLE_ACTION,
        payload: status,
    };
}
export function loginFailedAction(data) {
  return {
    type: LOGIN_FAILED_ACTION,
    payload: data,
  };
}