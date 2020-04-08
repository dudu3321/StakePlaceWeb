import React, { useState } from 'react'
import PropTypes from 'prop-types';
import { Dropdown, Input } from 'antd';
//import { Test } from './Filter.styles';

const TabFilter = (props) => {

  const [filtersData, setFiltersData] = useState(await fetch('Filter'));

  return (
    <div>
    </div>
  );
};

TabFilter.propTypes = {
  // bla: PropTypes.string,
};

TabFilter.defaultProps = {
  // bla: 'test',
};

export default TabFilter;
