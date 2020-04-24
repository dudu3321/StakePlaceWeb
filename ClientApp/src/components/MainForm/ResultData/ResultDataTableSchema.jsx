import React from 'react'
import { Tag } from 'antd';

export const tableSchema = [
    {
        title: 'Account',
        dataIndex: 'account',
        key: 'account',
    },
    {
        title: 'League',
        dataIndex: 'league',
        key: 'league',
        render: (text, record) => {
            const { leagueColor } = record;
            return <span style={{ color: `rgb(${leagueColor.r},${leagueColor.g},${leagueColor.b})` }}>{text}</span>
        }
    },
    {
        title: 'Home',
        dataIndex: 'home',
        key: 'home',
        render: (text, record) => {
            const { homeColor } = record;
            return <span style={{ color: `rgb(${homeColor.r},${homeColor.g},${homeColor.b})` }}>{text}</span>
        }
    },
    {
        title: 'Away',
        dataIndex: 'away',
        key: 'away',
        render: (text, record) => {
            const { awayColor } = record;
            return <span style={{ color: `rgb(${awayColor.r},${awayColor.g},${awayColor.b})` }}>{text}</span>
        }
    },
    {
        title: 'Type',
        dataIndex: 'transType',
        key: 'transType',
        render: (text, record) => {
            const { transTypeColor } = record;
            return <Tag color={`rgb(${transTypeColor.r},${transTypeColor.g},${transTypeColor.b})`}>{text}</Tag>
        }
    },
    {
        title: 'Run',
        dataIndex: 'run',
        key: 'run',
    },
    {
        title: 'HDP',
        dataIndex: 'hdp',
        key: 'hdp',
        render: (text, record) => {
            const { hdpColor } = record;
            return <span style={{ color: `rgba(${hdpColor.r},${hdpColor.g},${hdpColor.b},${hdpColor.a})` }}>{text}</span>
        }
    },
    {
        title: 'MMR',
        dataIndex: 'mmrOdds',
        key: 'mmrOdds',
        render: (text, record) => {
            const { mmrOddsColor } = record;
            return <span style={{ color: `rgba(${mmrOddsColor.r},${mmrOddsColor.g},${mmrOddsColor.b},${mmrOddsColor.a})` }}>{text}</span>
        }
    },
    {
        title: 'Odds',
        dataIndex: 'odds',
        key: 'odds',
        render: (text, record) => {
            const { oddsColor } = record;
            return <span style={{ color: `rgba(${oddsColor.r},${oddsColor.g},${oddsColor.b},${oddsColor.a})` }}>{text}</span>
        }
    },
    {
        title: 'MY',
        dataIndex: 'myOdds',
        key: 'myOdds',
        render: (text, record) => {
            const { myOddsColor } = record;
            return <span style={{ color: `rgba(${myOddsColor.r},${myOddsColor.g},${myOddsColor.b},${myOddsColor.a})` }}>{text}</span>
        }
    },
    {
        title: 'Amt',
        dataIndex: 'amount',
        key: 'amount',
    },
    {
        title: 'D',
        dataIndex: 'dangerStatus',
        key: 'dangerStatus',
        render: (text, record) => {
            const { dangerStatusColor } = record;
            return <Tag color={`rgb(${dangerStatusColor.r},${dangerStatusColor.g},${dangerStatusColor.b})`}>{text}</Tag>
        }
    },
    {
        title: 'Time',
        dataIndex: 'betTime',
        key: 'betTime',
    },
    {
        title: 'Date',
        dataIndex: 'transDate',
        key: 'transDate',
        render: (text, record) => {
            let dateArr = text.slice(0, 10).split('/');
            return <span>{`${dateArr[2]}/${dateArr[1]}`}</span>
        }
    },
    {
        title: 'IP',
        dataIndex: 'betIp',
        key: 'betIp',
    },
    {
        title: 'Ref',
        dataIndex: 'refNo',
        key: 'refNo',
        render: (text, record) => {
            const { refNoColor } = record;
            return <span style={{ color: `rgba(${refNoColor.r},${refNoColor.g},${refNoColor.b},${refNoColor.a})` }}>{text}</span>
        }
    },
    {
        title: 'Operated',
        dataIndex: 'operated',
        key: 'operated',
    },
    {
        title: 'Updated',
        dataIndex: 'updated',
        key: 'updated',
    },
    {
        title: 'Code',
        dataIndex: 'matchCode',
        key: 'matchCode',
    }
];