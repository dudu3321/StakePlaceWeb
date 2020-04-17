import React, { PureComponent } from 'react';
import { Input, Button, Checkbox, Alert } from 'antd';
import { CheckOutlined, CloseOutlined } from '@ant-design/icons';
import { instanceOf } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import './Login.styles.scss';
import { Redirect } from 'react-router-dom';

class Login extends PureComponent {
  constructor(props) {
    super(props);

    const { cookies } = props;

    this.state = {
      onLoading: false,
      hasError: false,
      moLogin: cookies.get('moLogin') || '',
      password: cookies.get('password') || '',
      rememberMe: cookies.get('moLogin') ? true : false,
      moLoginResponse: null
    };
  }

  loginOnClick = async () => {
    
    await fetch('login', {
      method: 'POST',
      headers: {
        'content-type': 'application/json'
      },
      body: JSON.stringify({
        MoLogin: this.state.moLogin,
        Password: this.state.password
      })
    })
      .then(response => response.json())
      .then(result => {
        if (result.moLoginStatus === 1) {
          this.setLoginCookie(result);
        } 
        this.setState({ moLoginResponse: result });
      });
  };

  setLoginCookie = (result) => {
    const { cookies } = this.props;
    cookies.set('moLogin', this.state.rememberMe ? this.state.moLogin : '', { path: '/' });
    cookies.set('password', this.state.rememberMe ? this.state.password : '', { path: '/' });
    cookies.set('view', result.view, { path: '/' });
    cookies.set('userLevels', result.view, { path: '/' });
  };

  cancelOnClick = () => {
    window.close();
  };

  alertOnClose = () => {
    this.setState({ moLoginResponse: null });
  };

  handleChange = event => {
    switch (event.target.id) {
      case 'moLogin':
        this.setState({ moLogin: event.target.value });
        break;
      case 'password':
        this.setState({ password: event.target.value });
        break;
      case 'rememberMe':
        this.setState({ rememberMe: event.target.checked });
        break;
      default: 
        break;
    }
  };

  render() {
    let errorAlert;
    if (this.state.hasError) {
      return <h1>Something went wrong.</h1>;
    }
    if (this.state.moLoginResponse) {
      if (this.state.moLoginResponse.moLoginStatus === 1) {
        return <Redirect to='/Main'></Redirect>
      } 
      errorAlert = (
        <div className="container-box">
          <Alert
            className="alert"
            message={this.state.moLoginResponse.title}
            description={this.state.moLoginResponse.message}
            type="error"
            closable
            showIcon
            onClose={this.alertOnClose}
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
              value={this.state.moLogin}
              onChange={this.handleChange}
            />
          </div>
          <div className="container-box">
            <Input.Password
              id="password"
              placeholder="Password"
              size="large"
              onChange={this.handleChange}
              value={this.state.password}
            />
          </div>
          <div className="container-box">
            <Checkbox id="rememberMe" onChange={this.handleChange} checked={this.state.rememberMe}>
              Remember my login
            </Checkbox>
          </div>
          <div className="container-box">
            <Button onClick={this.loginOnClick}>
              <CheckOutlined style={{ color: '#76A797' }} />
              Login
            </Button>
            <Button onClick={this.cancelOnClick}>
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
}

Login.propTypes = {
  cookies: instanceOf(Cookies).isRequired
};

Login.defaultProps = {
  // bla: 'test',
};

export default withCookies(Login);
