import { combineReducers } from 'redux';
import filtersData from './filtersData';
import ticketsQueryParamter from './ticketsQueryParamter';
import resultData from './resultData'
const filterApp = combineReducers({
  filtersData,
  ticketsQueryParamter,
  resultData
});

export default filterApp;