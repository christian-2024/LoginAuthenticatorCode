import React from "react";
import { useNavigate } from 'react-router-dom';

const Dashboard = () => {
    const navigate = useNavigate();

    const logout = () => {
     navigate('/login');
}

    return (
        <div>
            <h2> Bem-vindo ao Dashboard!</h2>
            <p>Está é o iniciao da aplicação após login, realizado. Parabéns, login foi bem sucessido!</p>
            <div>
                <button onClick={logout}>
                    Sair
                </button>
            </div>
        </div>
    );
}


export default Dashboard;