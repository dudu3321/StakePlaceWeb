import { combineReducers } from 'redux';
import filtersData from './filtersData';
import ticketsQueryParamter from './ticketsQueryParamter';

const filterApp = combineReducers({
  filtersData,
  ticketsQueryParamter
});

export default filterApp;