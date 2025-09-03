import React, { useEffect, useRef, useState } from "react";
import { Eye, EyeOff, LogIn } from "lucide-react";
import { connect, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import '../../assets/css/style.css';
import { loadingToggleAction, loginAction } from "../../store/action/AuthActions"; 


const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  let errorsObj = { email: '', password: '' };
  const [errors, setErrors] = useState(errorsObj);
  const dispatch = useDispatch(); 

  const navigate = useNavigate();

  const toast = ({ title, description }) => {
    alert(`${title}\n${description}`);
  }

  const handleSubmitLogin = async (e) => {
    e.preventDefault();
      try{
            let error = false;
            const errorObj = { ...errorsObj };

            if (email === ''){
              errorObj.email = 'É necessário digitar o email';
              error = true;
            }
            if (password === ''){
              errorObj.password = 'É necessário digitar a senha';
              erro = true;
            }
            setErrors(errorObj);
            if(error) {
              return;
            }
            dispatch(loadingToggleAction(true));
            dispatch(loginAction(email, password, navigate)); 
      } catch(err) {
          setError("Verifique E-mail e senha e tente novamente.");
      }
  };


  return (
    <div className="login-container">
      {/* Todo o conteúdo do formulário deve estar aqui dentro */}
      <div className="login-form-box">
        <header className="login-header">
          <h1 id="page-title">Login</h1>
          <p>Acesse sua conta para continuar</p>
        </header>

        <form onSubmit={handleSubmitLogin} className="login-form">
          <div className="form-group">
            <label htmlFor="email" className="text-label">E-mail</label>
            <input 
              id="email" 
              type="email" 
              placeholder="email@exemplo.com" 
              value={email}
              onChange={(e) => setEmail(e.target.value)} 
              className="form-input"
            />
          </div>

          <div className="form-group">
              <label htmlFor="password" className="text-label">Senha</label>
              <div className="password-input-container">
              <input
                id="password"
                type={showPassword ? "text" : "password"}
                placeholder="******"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="form-input"
              />
              <div className="option-eye">
              <button
                type="button"
                onClick={() => setShowPassword((v) => !v)}
                className="password-button"
              >
                {showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
              </button>
              </div>
            </div>
          </div>

          {error && (
            <p className="error-message">{error}</p>
          )}

          <div className="options-group">
            <label className="check">
              <input type="checkbox"/>
              Lembrar-me
            </label>
            <a href="#">Esqueceu a senha?</a>
          </div>

          <button type="submit" className="submit-button">
            <LogIn className="mr-2 h-4 w-4" /> Entrar
          </button>

          <p className="register-text">
            Novo por aqui? <a href="#">Criar conta</a>
          </p>
        </form>
      </div>
    </div>
      
  );
};

export default Login;
