export const SET_FILTERS = 'SET_FILTERS';
export const SET_FILTER_SELECTED = 'SET_FILTER_SELECTED';
export const SET_ALL_FILTER_SELECTED = 'SET_ALL_FILTER_SELECTED';
export const setFiltersData = (filtersData) => ({
  type: SET_FILTERS,
  filtersData,
});

export const setFilterSelected = (selectedObj) => ({
  type: SET_FILTER_SELECTED,
  selectedObj,
});

export const setAllFilterSelected = (selectedObj) => ({
  type: SET_FILTER_SELECTED,
  selectedObj,
});
