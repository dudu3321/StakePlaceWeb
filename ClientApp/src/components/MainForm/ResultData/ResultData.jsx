import React, { PureComponent } from 'react'
import { Table } from 'antd'
import { connect } from 'react-redux'
import { setResultData } from '../../../redux/actions/main/index'
import { withCookies } from 'react-cookie'
import { pendingStatus } from '../../common'
import { tableSchema } from './ResultDataTableSchema'
import moment from 'moment'


class ResultData extends PureComponent {
  componentDidMount = () => {
    this.getResultData();
    this.intervalId = setInterval(this.getResultData, 1000);
  }

  componentWillUnmount = () => {
    clearInterval(this.intervalId);
  }

  getResultData = async () => {
    const { queryParam, cookies, resultData } = this.props;
    
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
        let newResultData = [...resultData];
        if (result) {
          let resultDetailData = {
            added: 0,
            stocks: 0,
            tradeIns: 0,
            latePending: 'None',
            queryTime: ''
          };

          //更新舊有資料
          //沒有舊有資料情況
          if (newResultData.length === 0) {
            newResultData = result;
            resultDetailData.added = result.length;
          }
          else {
            result.forEach(e => {
              let oldElement = newResultData.find(x => x.refNo === e.refNo);
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
            let transDate = latePendingTickets.reduce((a, b) => a < b ? a : b).transDate;
            resultDetailData.latePending = this.getLatePendingString(transDate);
          }

          this.props.setResultData({
            resultData: newResultData,
            resultDetailData: resultDetailData
          });
        }
      });
  }

  getLatePendingString = (transDate) => {
    let subTransDate = transDate.substring(11);
    let dateNow = moment.utc().add(8, 'hour');
    let dateTransDate = moment(transDate);
    let diff = moment.duration(dateNow.diff(dateTransDate));
    return `${subTransDate} [+${diff._data.minutes}m ${diff._data.seconds}s]`;
  }

  reMapParamData = (object, mapFn) => {
    return Object.keys(object).reduce(
      (result, key) => {
        result[key] = mapFn(object[key]);
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
    }
  }
}


export default withCookies(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(ResultData));