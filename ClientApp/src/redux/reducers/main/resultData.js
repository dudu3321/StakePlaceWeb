import { SET_RESULT_DATA } from '../../actions/main';


const initialState = { resultData: [[]], resultDetailData: [[]] };

const resultData = (state = initialState, action) => {
    let newState = Object.assign({}, state);
    switch (action.type) {
        case SET_RESULT_DATA:
            const { index, resultData } = action;
            newState.resultData.splice(index, 1, resultData.resultData);
            newState.resultDetailData.splice(index, 1, resultData.resultDetailData);
            return newState;
        default:
            return state;
    }
};

export default resultData;
