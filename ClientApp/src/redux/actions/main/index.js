export const SET_FILTERS_DATA = 'SET_FILTERS_DATA';
export const SET_QUERY_PARAM = 'SET_QUERY_PARAM';
export const SET_RESULT_DATA = 'SET_RESULT_DATA';

export const setFiltersData = (filtersData) => ({
  type: SET_FILTERS_DATA,
  filtersData
});

export const setQueryParam = (index, queryParam) => ({
  type: SET_QUERY_PARAM,
  index,
  queryParam
});

export const setResultData = (index, resultData) => ({
  type: SET_RESULT_DATA,
  index,
  resultData
});


