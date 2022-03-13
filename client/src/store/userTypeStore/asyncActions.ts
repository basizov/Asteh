import { ThunkAction } from "redux-thunk";
import { UserTypeAction, userTypeActions } from ".";
import { RootState } from "..";
import { API } from "../../api";

type AsyncThunkType = ThunkAction<
  Promise<void>,
  RootState,
  unknown,
  UserTypeAction
>;

export const getUserTypesAsync = (fromDatabase = true): AsyncThunkType => {
  return async dispatch => {
    dispatch(userTypeActions.setLoading(true));
    try {
      const response = await API(fromDatabase).USER_TYPES.getUserTypes();
      if (response) {
        dispatch(userTypeActions.setUserTypes(response));
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userTypeActions.setLoading(false));
    }
  };
};
