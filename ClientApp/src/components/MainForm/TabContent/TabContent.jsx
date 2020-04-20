import React from 'react'
import ResultData from '../ResultData/index';
import Filter from '../Filter/index';
import Info from '../Info/index';
import { connect } from 'react-redux';
import './TabContent.styles.scss'
import { PureComponent } from 'react';

class TabContent extends PureComponent {
  constructor(props) {
    super(props)
    const { tabIndex } = this.props;
    this.state = {
      tabIndex: tabIndex
    }
  }

  render() {
    return (
      <div>
        <div className="tab_content_control">
          <Filter tabIndex={this.state.tabIndex}></Filter>
        </div>
        <div className="tab_content_control tab_content_Info">
          <Info tabIndex={this.state.tabIndex}></Info>
        </div>
        <div className="tab_content_control">
          <ResultData tabIndex={this.state.tabIndex}></ResultData>
        </div>
      </div>
    )
  }
}

const mapStateToProps = (state) => {
  return {};
}

const mapDispatchToProps = (dispatch) => {
  return {};
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TabContent);
