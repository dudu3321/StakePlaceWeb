import React from 'react'
import { Redirect } from 'react-router'
import { withCookies } from 'react-cookie'
import { Spin, Alert } from 'antd'

const UserStatus = (props) => {
    const { cookies } = props;
    return (
        <div>
            <HubState hubConnection={props.hubConnection}></HubState>
            <CookieState userLevels={cookies.get('userLevels')}></CookieState>
        </div>
    );
}


function Logout() {
    return (
        <Redirect to='/'></Redirect>
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
        return <Logout />;
    });


    return <div></div>;
}

const CookieState = (props) => {
    const { userLevels } = props;
    if (!!!userLevels && window.location.pathname !== '/') {
        return <Logout></Logout>;
    }
    return (<div></div>);
}



export default withCookies(UserStatus);