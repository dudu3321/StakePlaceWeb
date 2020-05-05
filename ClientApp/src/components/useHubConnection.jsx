import React, { useState, useEffect } from 'react'
import * as signalR from "@microsoft/signalr";

export default function useHubConnection(hubUrl) {
    const [hubConnection, setHubConnection] = useState();

    useEffect(() => {
        if (hubConnection) {
            setHubConnection(createConnection(hubUrl));
        }
    });

    return hubConnection;
}

const createConnection = (hubUrl) => {
    const hubConnect = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .configureLogging(signalR.LogLevel.Information)
        .build();
    hubConnect.start();
    return hubConnect;
}