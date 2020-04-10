import React, { Component, PureComponent } from 'react'
import { connect } from 'react-redux'
import { Select, Input } from 'antd';
const { Option } = Select;

class TabFilter extends PureComponent {

  
  constructor(props) {
    super(props);
    this.state = {
      selectedFilters: {},
      filtersData: this.getFiltersData()
    }
  }

  getFiltersData = async () => {
    await fetch('Filter')
      .then(response => response.json())
      .then(result => {
        this.setState({ filtersData: result });
        if (result) {
          for (var element in result) {
            this.state.selectedFilters[element] = result[element][0].value;
          };
        }
      });;
  }

  getSelectOptions = (key) => {
    if (this.state.filtersData[key]) {
      return this.state.filtersData[key].map(i => <Option value={i.value}>{i.description}</Option>);
    }
    return;
  }

  onchangeHandle = () => {
  }
  render() {
    return (
      < div >
        <Select defaultValue={this.state.selectedFilters.viewLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('viewLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.marketLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('marketLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.recordLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('recordLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.sportLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('sportLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.transactionLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('transactionLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.vipLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('vipLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.specialLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('specialLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.ticketLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('ticketLines')}</Select>
        <Select defaultValue={this.state.selectedFilters.statusLines} onchangeHandle={this.onchangeHandle}>{this.getSelectOptions('statusLines')}</Select>
      </div >

    )
  }
}

const mapStateToProps = (state) => ({

})

const mapDispatchToProps = {

}

export default connect(mapStateToProps, mapDispatchToProps)(TabFilter)
