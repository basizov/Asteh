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

export const getUsersAsync = (): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API.USERS.getUsers();
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

export const getUserByIdAsync = (id: number): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API.USERS.getUserById(id);
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
  payload: FilterModel
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const params = new URLSearchParams();
      params.append('name', payload.name || '');
      params.append('typeName', payload.typeName || '');
      params.append('beginDate', payload.beginDate || '');
      params.append('endDate', payload.endDate || '');

      const response = await API.USERS.filterUsers(params);
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

export const createUserAsync = (
  payload: CreateUserModel
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API.USERS.createUser(payload);
      if (response) {
        dispatch(userActions.addUser(response));
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
  payload: UpdateUserModel
): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API.USERS.updateUser(id, payload);
      if (response) {
        getUsersAsync();
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};

export const deleteUserAsync = (id: number): AsyncThunkType => {
  return async dispatch => {
    dispatch(userActions.setLoading(true));
    try {
      const response = await API.USERS.deleteUser(id);
      if (response) {
        getUsersAsync();
      }
    } catch (e) {
      console.log(e);
    } finally {
      dispatch(userActions.setLoading(false));
    }
  };
};
