import { SET_QUERY_PARAM } from '../../actions/main';

const initialState = {
  queryParam:[{}]
};

const ticketsQueryParamter = (state = initialState, action) => {
  let newState = Object.assign({}, state);
  switch (action.type) {
    case SET_QUERY_PARAM:
      const { index, queryParam } = action;
      newState.queryParam.splice(index, 1, queryParam);
      return newState;
    default:
      return state;
  }
};

export default ticketsQueryParamter;