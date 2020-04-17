export const SET_FILTERS_DATA = 'SET_FILTERS_DATA';
export const SET_QUERY_PARAM = 'SET_QUERY_PARAM';

export const setFiltersData = (filtersData) => ({
  type: SET_FILTERS_DATA,
  filtersData
});

export const setQueryParam = (index, queryParam) => ({
  type: SET_QUERY_PARAM,
  index,
  queryParam
});