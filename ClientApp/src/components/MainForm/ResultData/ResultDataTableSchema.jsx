import React from 'react'
import { Tag } from 'antd';

export const tableSchema = [
    {
        title: 'Account',
        dataIndex: 'account',
        key: 'account',
        render: (text, record) => {
            const { accountForeColor, accountBackColor } = record;
            return <span style={getFontColor(accountForeColor), {backgroundColor: getTagStyle(accountBackColor)}}>{text}</span>
        }
    },
    {
        title: 'League',
        dataIndex: 'league',
        key: 'league',
        render: (text, record) => {
            const { leagueColor } = record;
            return <span style={getFontColor(leagueColor)}>{text}</span>
        }
    },
    {
        title: 'Home',
        dataIndex: 'home',
        key: 'home',
        render: (text, record) => {
            const { homeColor } = record;
            return <span style={getFontColor(homeColor)}>{text}</span>
        }
    },
    {
        title: 'Away',
        dataIndex: 'away',
        key: 'away',
        render: (text, record) => {
            const { awayColor } = record;
            return <span style={getFontColor(awayColor)}>{text}</span>
        }
    },
    {
        title: 'Type',
        dataIndex: 'transType',
        key: 'transType',
        render: (text, record) => {
            const { transTypeColor } = record;
            return <Tag color={getTagStyle(transTypeColor)}>{text}</Tag>
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
            return <span style={getFontColor(hdpColor)}>{text}</span>
        }
    },
    {
        title: 'MMR',
        dataIndex: 'mmrOdds',
        key: 'mmrOdds',
        render: (text, record) => {
            const { mmrOddsColor } = record;
            return <span style={getFontColor(mmrOddsColor)}>{text}</span>
        }
    },
    {
        title: 'Odds',
        dataIndex: 'odds',
        key: 'odds',
        render: (text, record) => {
            const { oddsColor } = record;
            return <span style={getFontColor(oddsColor)}>{text}</span>
        }
    },
    {
        title: 'MY',
        dataIndex: 'myOdds',
        key: 'myOdds',
        render: (text, record) => {
            const { myOddsColor } = record;
            return <span style={getFontColor(myOddsColor)}>{text}</span>
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
            return <Tag color={getTagStyle(dangerStatusColor)}>{text}</Tag>
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
            return <span style={getFontColor(refNoColor)}>{text}</span>
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

const getFontColor = (colorObj) => {
    switch (typeof colorObj) {
        case 'object':
            if (colorObj.r === 0 && colorObj.g === 0 && colorObj.b === 0 && colorObj.a === 0) {
                return { color: 'Black' };
            }
            return { color: `rgba(${colorObj.r},${colorObj.g},${colorObj.b},${colorObj.a})` };
        case 'string':
            return { color: colorObj };
        default:
            return { color: 'Black' };
    }
}

const getTagStyle = (colorObj) => {
    switch (typeof colorObj) {
        case 'object':
            return `rgb(${colorObj.r},${colorObj.g},${colorObj.b})`;
        case 'string':
            return colorObj;
        default:
            return 'Black';
    }
}