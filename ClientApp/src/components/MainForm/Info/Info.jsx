import React, { PureComponent } from 'react'
import './Info.styles.scss';

export default class Info extends PureComponent {
  constructor(props) {
    super(props)
    const {LatePending} = this.props;
    this.state = {
      Added: 0,
      Stocks: 0,
      TradeIns: 0,
      LatePending: LatePending,
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
