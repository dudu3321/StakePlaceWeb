import { SET_QUERY_PARAM } from '../../actions/main';

const initialState = {
  queryParam:[]
};

const ticketsQueryParamter = (state = initialState, action) => {
  switch (action.type) {
    case SET_QUERY_PARAM:
      let newState = Object.assign({}, state);
      if (newState.queryParam.Any(x => x == action.index)) {
        newState.queryParam[action.index] = action.queryParam;
      }
      else {
        newState.queryParam.push(action.queryParam);
      }
      return newState;
    default:
      return state;
  }
};

export default ticketsQueryParamter;