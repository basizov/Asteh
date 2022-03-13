import { Button, FormControl, Grid, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { Form, Formik } from "formik";
import { useMemo } from "react";
import { useDispatch } from "react-redux";
import { object, string } from "yup";
import { SchemaOptions } from "yup/lib/schema";
import { CreateUserModel } from "../../api/models/request/CreateUserModel";
import { useTypedSelector } from "../../hooks/useTypedSelector";
import { createUserAsync } from "../../store/userStore/asyncActions";

type PropsType = {
  closeModal: () => void;
};

export const UserCreate : React.FC<PropsType> = ({closeModal}) => {
  const dispatch = useDispatch();
  const intialCreatedUserState = useMemo(() => ({
    login: '',
    password: '',
    name: '',
    typeName: ''
  } as CreateUserModel), []);
  const validationSchema: SchemaOptions<CreateUserModel> = useMemo(() => object({
    login: string().required(),
    name: string().required(),
    typeName: string().required(),
    password: string().required()
  }), []);
  const {userTypes} = useTypedSelector(s => s.userTypes);
  
  return <Formik
    initialValues={intialCreatedUserState}
    validationSchema={validationSchema}
    onSubmit={async values => {
      await dispatch(createUserAsync(values));
      closeModal();
    }}
  >
    {({
      handleSubmit,
      handleBlur,
      handleChange,
      values,
      errors,
      setFieldValue
    }) => (
      <Form onSubmit={handleSubmit}>
        <Grid
          container
          sx={{padding: '1rem', minWidth: '20rem'}}
          direction='column'
          spacing={1}
        >
          <Grid item>
            <TextField
              id='login'
              type='text'
              variant='outlined'
              fullWidth
              onBlur={handleBlur}
              onChange={handleChange}
              onFocus={(e) => e.target.select()}
              value={values.login}
              error={!!errors.login}
              label="Логин"
            />
          </Grid>
          <Grid item>
            <TextField
              id='password'
              type='password'
              variant='outlined'
              fullWidth
              onBlur={handleBlur}
              onChange={handleChange}
              onFocus={(e) => e.target.select()}
              value={values.password}
              error={!!errors.password}
              label="Пароль"
            />
          </Grid>
          <Grid item>
            <TextField
              id='name'
              type='name'
              variant='outlined'
              fullWidth
              onBlur={handleBlur}
              onChange={handleChange}
              onFocus={(e) => e.target.select()}
              value={values.name}
              error={!!errors.name}
              label="ФИО"
            />
          </Grid>
          <Grid item>
              <FormControl fullWidth>
                <InputLabel id="typeName-label">Тип пользователя</InputLabel>
                <Select
                  id="typeName"
                  labelId="typeName-label"
                  value={values.typeName}
                  label="Тип пользователя"
                  error={!!errors.typeName}
                  onChange={(e) => setFieldValue('typeName', e.target.value)}
                >
                  {userTypes.map(userType => <MenuItem
                    key={userType.id}
                    value={userType.name}
                  >{userType.name}</MenuItem>)}
                </Select>
              </FormControl>
          </Grid>
          <Grid item>
            <Button
              variant="outlined"
              sx={{marginLeft: 'auto', display: 'block'}}
              type='submit'
            >Создать</Button>
          </Grid>
        </Grid>
      </Form>
    )}
  </Formik>
};