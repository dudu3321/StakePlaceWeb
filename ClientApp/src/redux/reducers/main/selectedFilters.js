import { SET_FILTER_SELECTED, SET_ALL_FILTER_SELECTED } from '../../actions/main';

const initialState = {};

const selectedFilters = (state = initialState, action) => {
  switch (action.type) {
    case SET_FILTER_SELECTED:
      return Object.assign({}, state, 
        Object.assign({}, state, action.selectedObj
        )
      );
    case SET_ALL_FILTER_SELECTED:
      return Object.assign({}, state, {
        selectedFilters: action.selectedObj
      });
    default:
      return state;
  }
};

export default selectedFilters;