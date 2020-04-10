export const SET_FILTERS = 'SET_FILTERS';
export const SET_FILTER_SELECTED = 'SET_FILTER_SELECTED';

export const setFilters = (filters) => ({
  type: SET_FILTERS,
  filters,
});

export const setFilterSelected = (opt) => ({
  type: SET_FILTER_SELECTED,
  opt,
});
