import { ThunkAction } from "redux-thunk";
import { AuthorizeAction, authorizeActions } from ".";
import { RootState } from "..";
import { API } from "../../api";
import { AuthorizeModel } from "../../api/models/request/AuthorizeModel";

type AsyncThunkType = ThunkAction<
  Promise<void>,
  RootState,
  unknown,
  AuthorizeAction
>;

export const getFullInfoAsync = (): AsyncThunkType => {
  return async dispatch => {
    dispatch(authorizeActions.setLoadingInitial(true));
    try {
      const response = await API.AUTHORIZATION.getFullInfo();
      if (response) {
        dispatch(authorizeActions.setFullInfo(response));
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(authorizeActions.setLoadingInitial(false));
    }
  };
};

export const authorizeUserAsync = (
  payload: AuthorizeModel
): AsyncThunkType => {
  return async dispatch => {
    dispatch(authorizeActions.setLoading(true));
    try {
      const response = await API.AUTHORIZATION.login(payload);
      if (response) {
        dispatch(authorizeActions.setFullInfo(response));
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(authorizeActions.setLoading(false));
    }
  };
};
