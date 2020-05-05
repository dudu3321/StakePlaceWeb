import React from 'react'
export const userStatus = (hubConnection) => {
    hubConnection.on('userLogout', ()=>{
        return logout;
    });

    if (!useCookies(['userLevels']) && window.location.pathname !== '/') {
        return logout;
    }

    return (<div></div>);
}

const logout = <Redirect to='/'></Redirect>;