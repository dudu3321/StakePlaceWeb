import React, { PureComponent } from 'react'
import { Table, Tag } from 'antd';
import { connect } from 'react-redux'
import { setResultData } from '../../../redux/actions/main/index'
import { withCookies, Cookies } from 'react-cookie'
import { pendingStatus } from '../../common'

const columns = [
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


class ResultData extends PureComponent {
  constructor(props) {
    super(props);
  }

  componentDidMount = () => {
    this.getResultData();
    this.intervalId = setInterval(this.getResultData, 1000);
  }

  componentWillUnmount = () => {
    clearInterval(this.intervalId);
  }

  getResultData = async () => {
    const { queryParam, cookies } = this.props;
    if (Object.keys(queryParam).length <= 1) return;
    fetch('ticket', {
      method: 'POST',
      headers: {
        'content-type': 'application/json'
      },
      body: JSON.stringify({
        ...this.reMapParamData(queryParam, (element) => { return element.value }),
        userLevels: cookies.get('userLevels'),
        matchCodes: cookies.get('matchCodes')
      })
    })
      .then(response => response.json())
      .then(result => {
        let newResultData = Object.assign({}, this.resultData);
        if (result) {
          let resultDetailData = {
            added: 0,
            stocks: 0,
            tradeIns: 0,
            latePending: 'None',
            queryTime: ''
          };

          //更新舊有資料
          if (Object.keys(newResultData).length == 0) {
            newResultData = result;
            resultDetailData.added = result.length;
          }
          else {
            result.forEach(e => {
              let oldElement = newResultData.find(x => x.SocTransId == e.SocTransId);
              if (oldElement) {
                oldElement = e;
              }
              else {
                newResultData.push(e);
                resultDetailData.added++;
              }
            });
          }
          //Get Detail Data
          resultDetailData.stocks = result.filter(e => e.isStock).length;
          resultDetailData.tradeIns = result.filter(e => !e.isStock).length;

          //Get latePending
          let latePendingTickets = result.filter(e => pendingStatus.indexOf(e.dangerStatus) >= 0);
          if (latePendingTickets.length > 0) {
            let transDate = result.filter(e => pendingStatus.indexOf(e.dangerStatus) >= 0).reduce((a, b) => a < b ? a : b)[0].transDate;
            let value = transDate.subString(11);
            let diff = Math.abs(Date(Date.UTC()), Date(value));
            resultDetailData.latePending = ` [+${diff / 60}m ${diff % 60}s]`
          }

          this.props.setResultData({
            resultData: newResultData,
            resultDetailData: resultDetailData
          });
        }
      });
  }

  reMapParamData = (object, mapFn) => {
    return Object.keys(object).reduce(function (result, key) {
      result[key] = mapFn(object[key])
      return result
    }, {})
  }

  render() {
    const { resultData } = this.props;
    if (typeof resultData.length === 'undefined') {
      return (<Table columns={columns}></Table>)
    }
    return (<Table columns={columns} dataSource={resultData} rowKey={resultData => resultData.SocTransId} scroll={{ x: 1800 }}></Table>)
  }
}


const mapStateToProps = (state, props) => {
  const { tabIndex } = props;
  let queryParam = Object.assign({}, state.ticketsQueryParamter.queryParam);
  if (typeof queryParam[tabIndex] !== 'undefined') {
    queryParam = queryParam[tabIndex];
  }
  let resultData = Object.assign({}, state.resultData);
  if (Object.keys(resultData).length > 0) {
    if (typeof resultData.resultData[tabIndex] !== 'undefined') {
      resultData = resultData.resultData[tabIndex];
    }
  }
  return {
    queryParam: queryParam,
    resultData: resultData
  }
}

const mapDispatchToProps = (dispatch, props) => {
  const { tabIndex } = props;
  return {
    setResultData: (newState) => {
      dispatch(setResultData(tabIndex, newState));
    }
  }
}


export default withCookies(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(ResultData));