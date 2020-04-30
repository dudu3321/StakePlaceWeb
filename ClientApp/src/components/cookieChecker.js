import React from 'react'

export default function cookieChecker() {
    if (!useCookies(['userLevels']) && window.location.pathname !== '/') {
        return (
            <Redirect to='/'></Redirect>
        )
    }
}
