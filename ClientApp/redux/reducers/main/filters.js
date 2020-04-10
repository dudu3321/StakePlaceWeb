import { SET_FILTERS, SET_FILTER_SELECTED } from '../../actions/main';
import { combineReducers } from 'redux'


const initialState = {
  filters: {},
  selectedOption: {}
};

const filters = (state = initialState, action) => {
  switch (action.type) {
    case SET_FILTERS:
      return Object.assign({}, state, {
        filters: action.filters
      });
    default:
      return state;
  }
};


const selectedOption = (state = initialState, action) => {
  switch (action.type) {
    case SET_FILTER_SELECTED:
      return Object.assign({}, state, {
        selectedOption: action.opt
      });
    default:
      return state;
  }
};

const filterApp = combineReducers({ 
  filters, 
  selectedOption 
});

export default filterApp;