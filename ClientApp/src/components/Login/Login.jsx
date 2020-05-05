import React, { useState } from 'react'
import { Input, Button, Checkbox, Alert } from 'antd'
import { CheckOutlined, CloseOutlined } from '@ant-design/icons'
import { instanceOf } from 'prop-types'
import { withCookies, Cookies } from 'react-cookie'
import { Redirect } from 'react-router-dom'
import './Login.styles.scss';

function Login(props) {
  const { cookies, hubConnection } = props;
  const [moLoginResponse, setMoLoginResponse] = useState();  
  const [moLogin, setMoLogin] = useState(cookies.get('moLogin'));
  const [password, setPassword] = useState(cookies.get('password'));
  const [rememberMe, setRememberMe] = useState(cookies.get('moLogin') ? true : false);
  

  const loginOnClick = async () => {
    hubConnection.on('userLogin', (response)=>{
      let jsonResponse = response.json();
      if(jsonResponse.moLoginStatus === 1){
        setLoginCookie(jsonResponse);
      }
      setMoLoginResponse(jsonResponse);
    });
  };

  const setLoginCookie = (result) => {
    const { cookies } = props;
    cookies.set('moLogin', rememberMe ? moLogin : '', { path: '/' });
    cookies.set('password', rememberMe ? password : '', { path: '/' });
    cookies.set('matchCodes', result.matchCodes, { path: '/' });
    cookies.set('userLevels', result.userLevels, { path: '/' });
  };

  const cancelOnClick = () => {
    window.close();
  };

  const alertOnClose = () => {
    setMoLoginResponse(null);
  };

  const handleChange = event => {
    switch (event.target.id) {
      case 'moLogin':
        setMoLogin(event.target.value);
        break;
      case 'password':
        setPassword(event.target.value);
        break;
      case 'rememberMe':
        setRememberMe(event.target.checked);
        break;
      default:
        break;
    }
  };

  let errorAlert;
  if (moLoginResponse) {
    if (moLoginResponse.moLoginStatus === 1) {
      return <Redirect to='/Main'></Redirect>
    }
    errorAlert = (
      <div className="container-box">
        <Alert
          className="alert"
          message={moLoginResponse.title}
          description={moLoginResponse.message}
          type="error"
          closable
          showIcon
          onClose={alertOnClose}
        ></Alert>
      </div>
    );
  }
  return (
    <div className="content">
      <div className="container">
        {errorAlert}
        <div className="container-box">
          <Input
            id="moLogin"
            placeholder="Login"
            size="large"
            value={moLogin}
            onChange={handleChange}
          />
        </div>
        <div className="container-box">
          <Input.Password
            id="password"
            placeholder="Password"
            size="large"
            onChange={handleChange}
            value={password}
          />
        </div>
        <div className="container-box">
          <Checkbox id="rememberMe" onChange={handleChange} checked={rememberMe}>
            Remember my login
            </Checkbox>
        </div>
        <div className="container-box">
          <Button onClick={loginOnClick}>
            <CheckOutlined style={{ color: '#76A797' }} />
              Login
            </Button>
          <Button onClick={cancelOnClick}>
            <CloseOutlined
              style={{ color: '#D86344' }}
              twoToneColor="#D86344"
            />
              Cancel
            </Button>
        </div>
      </div>
    </div>
  );
}
Login.propTypes = {
  cookies: instanceOf(Cookies).isRequired
};

Login.defaultProps = {
  // bla: 'test',
};

export default withCookies(Login);
