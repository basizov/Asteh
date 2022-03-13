import { Typography } from "@mui/material";
import { red } from "@mui/material/colors";
import { Form, Formik } from "formik";
import { useMemo } from "react";
import { useDispatch } from "react-redux";
import { object, string } from "yup";
import { SchemaOptions } from "yup/lib/schema";
import { UpdateUserModel } from "../../api/models/request/UpdateUserModel";
import { useTypedSelector } from "../../hooks/useTypedSelector";
import { updateUserAsync } from "../../store/userStore/asyncActions";

export const UserEdit : React.FC = () => {
  const dispatch = useDispatch();
  const validationSchema: SchemaOptions<UpdateUserModel> = useMemo(() => object({
    name: string().required(),
    typeName: string().required(),
    password: string().required(),
    lastVisitDate: string().required()
  }), []);

  const {selectedUser} = useTypedSelector(s => s.users);
  const intialCreatedUserState = useMemo(() => ({
    password: '',
    name: selectedUser?.name || '',
    typeName: selectedUser?.typeName || '',
    lastVisitDate: selectedUser?.lastVisitDate || ''
  } as UpdateUserModel), []);
  const {from} = useTypedSelector(s => s.authorization);
  if (selectedUser === null) {
    return <Typography
      variant="caption"
      sx={{color: red[500]}}
    >Такого пользователя не существует</Typography>
  }

  return <Formik
    initialValues={intialCreatedUserState}
    validationSchema={validationSchema}
    onSubmit={async values => {
      await dispatch(updateUserAsync(selectedUser.id, values, from));
    }}
  >
    {({
      handleSubmit,
      handleBlur,
      handleChange,
      values,
      errors,
      setFieldError
    }) => (
      <Form onSubmit={handleSubmit}>
        
      </Form>
    )}
  </Formik>
};