import React, { PureComponent } from 'react'
import { Select, Input, Checkbox } from 'antd';
import './TabFilter.styles.scss';
const { Option } = Select;

class TabFilter extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      filtersData: {},
      selectedFilters: {
        Amount: '',
        Ip: '',
        Account: '',
        scrollEnd: false
      }
    }
    this.getFiltersData();
  }

  getFiltersData = () => {
    const { updateEventHandler } = this.props;
    fetch('Filter')
      .then(response => response.json())
      .then(result => {
        this.setState({ filtersData: result });
        if (result) {
          for (let key in result) {
            let newState = Object.assign({}, this.state.selectedFilters);
            newState[key] = result[key][0];
            this.setState({ selectedFilters: newState });
          }
        }
      });;
    updateEventHandler(this.state);
  }

  getSelectOptions = (key) => {
    const { filtersData } = this.state;
    if (filtersData[key]) {
      return filtersData[key].map(i => <Option className="options" key={i.value} id={key}>{i.description}</Option>);
    }
    return;
  }

  selectHandleChange = (eventVal, eventElement) => {
    const { id, value } = eventElement;
    const { updateEventHandler } = this.props;
    let newState = Object.assign({}, this.state.selectedFilters);
    newState[id] = this.state.filtersData[id].find(i => i.value == value);
    this.setState({ selectedFilters: newState });
    updateEventHandler(newState);
  }

  inputHandleChange = (event) => {
    const { value, id } = event.target;
    const { updateEventHandler } = this.props;
    let newState = Object.assign({}, this.state.selectedFilters);

    newState[id] = value;
    this.setState({ selectedFilters: newState });
    updateEventHandler(newState);
  }

  checkboxHandleChange = () => {
    const { updateEventHandler } = this.props;
    let newState = Object.assign({}, this.state.selectedFilters);

    newState.scrollEnd = !newState.scrollEnd;
    this.setState({ selectedFilters: newState });
    updateEventHandler(newState);
  }

  render() {
    const { selectedFilters } = this.state;

    return (
      < div >
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={selectedFilters.viewLines ? selectedFilters.viewLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('viewLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={selectedFilters.marketLines ? selectedFilters.marketLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('marketLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={selectedFilters.recordLines ? selectedFilters.recordLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('recordLines')}</Select>
        <Input className="input" id="Amount" placeholder="Bet >=" value={this.state.selectedFilters.Amount} onChange={this.inputHandleChange}></Input>
        <Input className="input" id="Account" placeholder="Account" value={this.state.selectedFilters.Account} onChange={this.inputHandleChange}></Input>
        <Input className="input" id="Ip" placeholder="IP" value={this.state.selectedFilters.Ip} onChange={this.inputHandleChange}></Input>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={selectedFilters.sportLines ? selectedFilters.sportLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('sportLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_2" value={selectedFilters.transactionLines ? selectedFilters.transactionLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('transactionLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_4" value={selectedFilters.vipLines ? selectedFilters.vipLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('vipLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_3" value={selectedFilters.specialLines ? selectedFilters.specialLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('specialLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={selectedFilters.ticketLines ? selectedFilters.ticketLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('ticketLines')}</Select>
        <Select className="selector_1" dropdownMatchSelectWidth="false" dropdownClassName="options_1" value={selectedFilters.statusLines ? selectedFilters.statusLines.description : ''} onChange={this.selectHandleChange}>{this.getSelectOptions('statusLines')}</Select>
        <Checkbox onChange={this.checkboxHandleChange} value={this.state.selectedFilters.scrollEnd}>Scroll to end</Checkbox>
      </div >
    )
  }
}

export default TabFilter
