import React from 'react'
import useUserHub from './useHubConnection'
export const userStatus = () => {
    const hubConnection = useHubConnection('/ConnectionHub');

    hubConnection.on('userLogout', ()=>{
        return logout;
    });


    if (!useCookies(['userLevels']) && window.location.pathname !== '/') {
        return logout;
    }

    return (<div></div>);
}

const logout = <Redirect to='/'></Redirect>;