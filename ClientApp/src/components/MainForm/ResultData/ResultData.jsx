import React, { Component } from 'react'
import { Table } from 'antd'
import { connect } from 'react-redux'
import { setResultData, setQueryParam } from '../../../redux/actions/main/index'
import { withCookies } from 'react-cookie'
import { pendingStatus } from '../../common'
import { tableSchema } from './ResultDataTableSchema'
import * as signalR from "@microsoft/signalr";
import moment from 'moment'


//資料更新方式
//1. 開啟及條件修改 -> WebApi
//2. 每秒資料有更改時Server會以WebSocket推送至Client
class ResultData extends Component {
  constructor(props) {
    super(props);
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/TicketConnectionHub')
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.hubConnection
      .start()
      .then(() => {
       console.log('Connection started!');
        this.connectionId = this.hubConnection.connectionId;
        this.getResultData();
        this.intervalId = setInterval(() => this.intervalEventHandle(), 1000);
      }) 
      .catch(err => console.log('Error while establishing connection :('));
    this.hubConnection.on('updateResultData', (result) => {
      console.log('data received!');
      this.updateResultData(JSON.parse(result));
    });
  }

  //從API取得全部資料
  getResultData = async () => {
    const { queryParam, cookies } = this.props;
    if (Object.keys(queryParam).length <= 1 || !cookies.get('userLevels')|| !cookies.get('matchCodes')) return;
    fetch('ticket', {
      method: 'POST',
      headers: {
        'content-type': 'application/json'
      },
      body: JSON.stringify({
        connectionId: this.connectionId,
        ...this.reMapParamData(queryParam),
        userLevels: cookies.get('userLevels'),
        matchCodes: cookies.get('matchCodes')
      })
    })
      .then(response => response.json())
      .then(result => {
        this.updateResultData(result)
      });
  }

  //將資料更新至Status 
  updateResultData = (result) => {
    if (result) {
      const { added, stakePlaceTickets } = result;
      let resultDetailData = {
        added: added,
        stocks: 0,
        tradeIns: 0,
        latePending: 'None',
        queryTime: ''
      };
      
      //Get Detail Data
      resultDetailData.stocks = stakePlaceTickets.filter(e => e.isStock).length;
      resultDetailData.tradeIns = stakePlaceTickets.filter(e => !e.isStock).length;

      //Get latePending
      resultDetailData = this.getLatePending(resultDetailData, stakePlaceTickets);

      this.props.setResultData({
        resultData: stakePlaceTickets,
        resultDetailData: resultDetailData
      });
    }
  }

  //interval on tick event handle
  //1. 確認查詢條件是否有變更 ==yes==> 重新查詢資料
  //2. 更新latePending 時間
  intervalEventHandle = () => {
    const { queryParam, setQueryParam } = this.props;
    if (queryParam.filtersOnChange) {
      this.getResultData();
      let newQueryParam = { ...queryParam };
      newQueryParam.filtersOnChange = false;
      setQueryParam(newQueryParam);
    }
  }

  //取得LatePendging資料
  //(Param)resultDetailData: 要更新Status的物件
  //(Param)result: 查詢出的資料
  getLatePending = (resultDetailData, result) => {
    let latePendingTickets = result.filter(e => pendingStatus.indexOf(e.dangerStatus) >= 0);
    if (latePendingTickets.length > 0) {
      let transDate = latePendingTickets.reduce((a, b) => a < b ? a : b).transDate;
      resultDetailData.latePending = this.getLatePendingString(transDate);
    }
    return resultDetailData;
  }

  //取得並回傳LatePending字串
  getLatePendingString = (transDate) => {
    let subTransDate = transDate.substring(11);
    let dateNow = moment.utc().add(8, 'hour');
    let dateTransDate = moment(transDate);
    let diff = moment.duration(dateNow.diff(dateTransDate));
    return `${subTransDate} [+${diff._data.minutes}m ${diff._data.seconds}s]`;
  }
c

  componentWillUnmount = () => {
    this.hubConnection.stop();
  }


  reMapParamData = (object) => {
    return Object.keys(object).reduce(
      (result, key) => {
        let value = object[key];
        switch (typeof value) {
          case 'object':
            result[key] = value.value;
            break;
          case 'string':
            result[key] = value
            break;
        }
        return result;
      }, {})
  }

  render() {
    const { resultData } = this.props;
    if (resultData === 'undefined') {
      return (<Table columns={tableSchema}></Table>)
    }
    return (<Table columns={tableSchema} dataSource={resultData} scroll={{ x: 1800 }} rowKey="refNo"></Table>)
  }
}


const mapStateToProps = (state, props) => {
  const { tabIndex } = props;
  const { resultData } = state;
  let queryParam = Object.assign({}, state.ticketsQueryParamter.queryParam);
  let data = resultData.resultData;
  return {
    queryParam: (typeof queryParam[tabIndex] !== 'undefined') ? queryParam[tabIndex] : {},
    resultData: (typeof data[tabIndex] !== 'undefined') ? data[tabIndex] : []
  }
}

const mapDispatchToProps = (dispatch, props) => {
  const { tabIndex } = props;
  return {
    setResultData: (newState) => {
      dispatch(setResultData(tabIndex, newState));
    },
    setQueryParam: (newState) => {
      dispatch(setQueryParam(tabIndex, newState));
    }
  }
}


export default withCookies(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(ResultData));