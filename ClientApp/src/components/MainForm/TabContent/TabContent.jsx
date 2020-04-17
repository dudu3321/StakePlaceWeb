import React from 'react'
import ResultData from '../ResultData/index';
import Filter from '../Filter/index';
import Info from '../Info/index';
import { connect } from 'react-redux';
import './TabContent.styles.scss'

const TabContent = () => {
  return (
    <div>
      <div className="tab_content_control">
        <Filter></Filter>
      </div>
      <div className="tab_content_control tab_content_Info">
        <Info></Info>
      </div>
      <div className="tab_content_control">
        <ResultData></ResultData>
      </div>
    </div>
  )
}

const mapStateToProps = (state) => {

};

const mapDispatchToProps = (dispatch) => {

};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TabContent);
