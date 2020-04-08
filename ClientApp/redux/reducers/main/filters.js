import { SET_FILTERS } from '../../actions/main';

const initialState = {
  filters: {},
};

const filterApp = (state = initialState, action) => {
  switch (action.type) {
      case SET_FILTERS:
        return Object.assign({}, state);
  }
  return state;
};
