import React, { PureComponent } from 'react'
import { connect } from 'react-redux'
import './Info.styles.scss';

class Info extends PureComponent {
  constructor(props) {
    super(props)
  }

  componentDidMount = () => {
    this.intervalId = setInterval(() => {this.forceUpdate();}, 10000);
  }

  componentWillUnmount = () => {
    clearInterval(this.intervalId);
  }

  render() {
    const { resultDetailData } = this.props;
    return (
      <div>
        <label>Added: {resultDetailData.added ?? 0}</label>
        <label>Stocks: {resultDetailData.stocks ?? 0}</label>
        <label>Trade Ins: {resultDetailData.tradeIns ?? 0}</label>
        <label>Late pending: {resultDetailData.latePending ?? 0}</label>
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