import React, { Redirect, useReducer } from 'react'
import { useCookies } from 'react-cookie'
import { Spin, Alert } from 'antd'

const UserStatus = (props) => {
    return (
        <div>
            <HubState hubConnection={props.hubConnection}></HubState>
            <CookieState userLevels={useCookies(['userLevels'])}></CookieState>
        </div>
    );
}

const HubState = (props) => {
    const { hubConnection } = props;
    let hubIsConnected = () => hubConnection.ConnectionState === 'Connected' && !!!hubConnection.conectionId;

    if (!hubIsConnected) {
        return (
            <Spin tip='Service Connecting...'>
                <Alert type="info"></Alert>
            </Spin>
        );
    }

    hubConnection.on('userLogout', () => {
        return logout;
    });

    
    return (<div></div>);
}

const CookieState = (props) => {
    const { userLevels } = props;
    if (!userLevels && window.location.pathname !== '/') {
        return logout;
    }
    return (<div></div>);
}

const logout = () => <Redirect to='/'></Redirect>;

export default UserStatus;