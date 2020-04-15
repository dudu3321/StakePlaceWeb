import { combineReducers } from 'redux';
import filters from './filters';
import selectedFilters from './selectedFilters';

const filterApp = combineReducers({
  filters,
  selectedFilters
});

export default filterApp;