import { SET_FILTERS } from '../../actions/main';


const initialState = {
  filtersData: {}
};

const filters = (state = initialState, action) => {
  switch (action.type) {
    case SET_FILTERS:
      return Object.assign({}, state, {filtersData: action.filtersData});
    default:
      return state;
  }
};


export default filters;
