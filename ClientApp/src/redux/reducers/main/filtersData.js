import { SET_FILTERS_DATA } from '../../actions/main';


const initialState = {
  filtersData: {
    data1: 'test'
  }
};

const filtersData = (state = initialState, action) => {
  switch (action.type) {
    case SET_FILTERS_DATA:
      return Object.assign({}, state, {filtersData: action.filtersData});
    default:
      return state;
  }
};


export default filtersData;
