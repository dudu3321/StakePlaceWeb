import React, { PureComponent } from 'react'
import { connect } from 'react-redux'
import * as moment from 'moment'
import './Info.styles.scss';

//每秒從redux status內撈resultDetailData
//resultDetailData由ResultData component更新
class Info extends PureComponent {
  constructor(props) {
    super(props)
    this.intervalId = setInterval(() => { this.forceUpdate(); }, 1000);
  }

  componentWillUnmount = () => {
    clearInterval(this.intervalId);
  }

  render() {
    const { resultDetailData } = this.props;
    return (
      <div>
        <label className='label_Info'>Added: {resultDetailData.added ?? 0}</label>
        <label className='label_Info'>Stocks: {resultDetailData.stocks ?? 0}</label>
        <label className='label_Info'>Trade Ins: {resultDetailData.tradeIns ?? 0}</label>
        <label className='label_Info'>Late pending: {resultDetailData.latePending ?? 0}</label>
        <label className='label_Time'>{moment().format('hh:MM:ss')}</label>
      </div>
    )
  }
}


const mapStateToProps = (state, props) => {
  const { tabIndex } = props;
  let resultData = Object.assign({}, state.resultData);
  if (Object.keys(resultData).length > 0) {
    if (!!resultData.resultDetailData[tabIndex]) {
      resultData = resultData.resultDetailData[tabIndex];
    }
  }
  return {
    resultDetailData: resultData
  }
};
const mapDispatchToProps = () => {
  return {}
};
export default connect(mapStateToProps,
  mapDispatchToProps)(Info) 