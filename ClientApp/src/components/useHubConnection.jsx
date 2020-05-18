import React, { useState, useEffect } from 'react'
import * as signalR from "@microsoft/signalr";

export const useHubConnection = (hubUrl) => {
    const [hubConnection, setHubConnection] = useState(createConnection(hubUrl));

    return hubConnection;
}

const createConnection = (hubUrl) => {
    const hubConnect = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect({
            nextRetryDelayInMilliseconds: retryContext => {
                if (retryContext.elapsedMilliseconds < 60000) {
                    return Math.random() * 10000;
                } else {
                    return null;
                }
            }
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();
    hubConnect.start();
    return hubConnect;
}