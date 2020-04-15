import React, { PureComponent } from 'react'
import './TabInfo.styles.scss';

export default class TabInfo extends PureComponent {
  constructor(props) {
    super(props)

    this.state = {
      Added: 0,
      Stocks: 0,
      TradeIns: 0,
      LatePending: '',
      UpdateTime: ''
    }
  }

  render() {
    return (
      <div>
        <label>Added: {this.state.Added}</label>
        <label>Stocks: {this.state.Stocks}</label>
        <label>Trade Ins: {this.state.TradeIns}</label>
        <label>Late pending: {this.state.LatePending}</label>
        <label>{this.state.Stocks}</label>
      </div>
    )
  }
}
