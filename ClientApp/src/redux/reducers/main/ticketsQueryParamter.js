import { SET_QUERY_PARAM } from '../../actions/main';

const initialState = {
  queryParam:[{}]
};

const ticketsQueryParamter = (state = initialState, action) => {
  switch (action.type) {
    case SET_QUERY_PARAM:
      let newState = Object.assign({}, state);
      const { index, queryParam } = action;
      newState.queryParam.splice(index, 1, queryParam);
      return newState;
    default:
      return state;
  }
};

export default ticketsQueryParamter;