import React, { PureComponent } from 'react'
import { Select, Input, Checkbox } from 'antd';
import { connect } from 'react-redux'
import { setFiltersData, setQueryParam } from '../../../redux/actions/main/index'
import './Filter.styles.scss';
const { Option } = Select;

const defaultQueryParam = {
  Amount: '',
  Ip: '',
  Account: '',
  scrollEnd: false
}

class Filter extends PureComponent {
  constructor(props) {
    super(props);
    this.getFiltersData();
  }

  getFiltersData = () => {
    fetch('Filter')
      .then(response => response.json())
      .then(result => {
        if (result) {
          const { queryParam } = this.props;
          let newState = Object.assign({}, queryParam);
          for (let key in result) {
            newState[key] = result[key][0];
          }
          this.props.setFiltersData(result);
          this.props.setQueryParam({ ...newState, ...defaultQueryParam });
        }
      });;
  }

  getSelectOptions = (key) => {
    let { filtersData } = Object.assign({}, this.props);
    if (typeof filtersData[key] !== 'undefined') {
      return filtersData[key].map(i => <Option className="options" key={i.value} id={key}>{i.description}</Option>);
    }
    return;
  }

  selectHandleChange = (eventVal, eventElement) => {
    const { id, value } = eventElement;
    const { queryParam, filtersData } = this.props;
    let newState = Object.assign({}, queryParam);
    newState[id] = filtersData[id].find(i => i.value === parseInt(value));
    this.props.setQueryParam(newState);
  }

  inputHandleChange = (event) => {
    const { value, id } = event.target;
    const { queryParam } = this.props;
    let newState = Object.assign({}, queryParam);

    newState[id] = value;
    this.props.setQueryParam(newState);
  }

  checkboxHandleChange = () => {
    const { queryParam } = this.props;
    let newState = Object.assign({}, queryParam);

    newState.scrollEnd = !newState.scrollEnd;
    this.props.setQueryParam(newState);
  }

  render() {
    let { queryParam } = Object.assign({}, this.props);
    // if (typeof queryParam[tabIndex] !== 'undefined') {
    //   queryParam = queryParam[tabIndex];
      return (
        < div >
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={queryParam.viewLines ? queryParam.viewLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('viewLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={queryParam.marketLines ? queryParam.marketLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('marketLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={queryParam.recordLines ? queryParam.recordLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('recordLines')}</Select>
          <Input className="input" id="Amount" placeholder="Bet >=" value={queryParam.Amount} onChange={this.inputHandleChange}></Input>
          <Input className="input" id="Account" placeholder="Account" value={queryParam.Account} onChange={this.inputHandleChange}></Input>
          <Input className="input" id="Ip" placeholder="IP" value={queryParam.Ip} onChange={this.inputHandleChange}></Input>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={queryParam.sportLines ? queryParam.sportLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('sportLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_2" value={queryParam.transactionLines ? queryParam.transactionLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('transactionLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_4" value={queryParam.vipLines ? queryParam.vipLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('vipLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_3" value={queryParam.specialLines ? queryParam.specialLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('specialLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={queryParam.ticketLines ? queryParam.ticketLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('ticketLines')}</Select>
          <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={queryParam.statusLines ? queryParam.statusLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('statusLines')}</Select>
          <Checkbox onChange={this.checkboxHandleChange} checked={queryParam.scrollEnd}>Scroll to end</Checkbox>
        </div >
      )
    // }
    // return (<div></div>);

  }
}

const mapStateToProps = (state, props) => {
  const {tabIndex} = props;
  let queryParam = Object.assign({}, state.ticketsQueryParamter.queryParam);
  if(typeof queryParam[tabIndex] !== 'undefined'){
    queryParam = queryParam[tabIndex];
  }
  return {
    filtersData: state.filtersData.filtersData,
    queryParam: queryParam
  }
}

const mapDispatchToProps = (dispatch, props) => {
  return {
    setQueryParam: (newState) => {
      const { tabIndex } = props;
      dispatch(setQueryParam(tabIndex, newState));
    },
    setFiltersData: (newState) => {
      dispatch(setFiltersData(newState));
    }
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Filter);