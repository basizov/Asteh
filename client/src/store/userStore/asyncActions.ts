import { ThunkAction } from "redux-thunk";
import { UserAction, userActions } from ".";
import { RootState } from "..";
import { API } from "../../api";
import{ CreateUserModel } from "../../api/models/request/CreateUserModel";
import { FilterModel } from "../../api/models/request/FilterModel";
import { UpdateUserModel } from "../../api/models/request/UpdateUserModel";

type AsyncThunkType = ThunkAction<
  Promise<void>,
  RootState,
  unknown,
  UserAction
>;

export const getUsersAsync = (fromDatabase = true): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API(fromDatabase).USERS.getUsers();
      if (response) {
        dispatch(userActions.setUsers(response));
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};

export const getUserByIdAsync = (
  id: number,
  fromDatabase = true
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API(fromDatabase).USERS.getUserById(id);
      if (response) {
        dispatch(userActions.setSelectedUser(response));
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};

export const filterUsersAsync = (
  payload: FilterModel,
  fromDatabase = true
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const params = new URLSearchParams();
      params.append('name', payload.name || '');
      params.append('typeName', payload.typeName || '');
      params.append('beginDate', payload.beginDate || '');
      params.append('endDate', payload.endDate || '');

      const response = await API(fromDatabase).USERS.filterUsers(params);
      if (response) {
        dispatch(userActions.setUsers(response));
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};

export const createUserAsync = (
  payload: CreateUserModel,
  fromDatabase = true
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API(fromDatabase).USERS.createUser(payload);
      if (response) {
        await dispatch(getUsersAsync());
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};

export const updateUserAsync = (
  id: number,
  payload: UpdateUserModel,
  fromDatabase = true
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API(fromDatabase).USERS.updateUser(id, payload);
      if (response) {
        await dispatch(getUsersAsync());
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};

export const deleteUserAsync = (
  id: number,
  fromDatabase = true
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API(fromDatabase).USERS.deleteUser(id);
      if (response) {
        await dispatch(getUsersAsync());
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};
