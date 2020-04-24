import React, { PureComponent } from 'react'
import { connect } from 'react-redux'
import * as moment from 'moment'
import './Info.styles.scss';

class Info extends PureComponent {
  componentDidMount = () => {
    this.intervalId = setInterval(() => { this.forceUpdate(); }, 10000);
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
    if (typeof resultData.resultDetailData[tabIndex] !== 'undefined') {
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