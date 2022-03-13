import { DatePicker, DateRangePicker, LocalizationProvider } from "@mui/lab";
import DateFnsAdapter from '@mui/lab/AdapterDateFns';
import { Button, ButtonGroup, FormControl, Grid, InputLabel, MenuItem, Select, TextField, Typography } from "@mui/material";
import { red } from "@mui/material/colors";
import { format, parse } from "date-fns";
import { ru } from "date-fns/locale";
import { Form, Formik } from "formik";
import { useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { object, string } from "yup";
import { SchemaOptions } from "yup/lib/schema";
import { UpdateUserModel } from "../../api/models/request/UpdateUserModel";
import { useTypedSelector } from "../../hooks/useTypedSelector";
import { deleteUserAsync, updateUserAsync } from "../../store/userStore/asyncActions";

type PropsType = {
  closeModal: () => void;
};

export const UserEdit : React.FC<PropsType> = ({closeModal}) => {
  const dispatch = useDispatch();
  const validationSchema: SchemaOptions<UpdateUserModel> = useMemo(() => object({
    name: string().required(),
    typeName: string().required(),
    password: string().required(),
    lastVisitDate: string().required()
  }), []);
  const {userTypes} = useTypedSelector(s => s.userTypes);

  const {selectedUser} = useTypedSelector(s => s.users);
  const intialCreatedUserState = useMemo(() => ({
    password: '',
    name: selectedUser?.name || '',
    typeName: selectedUser?.typeName || '',
    lastVisitDate: selectedUser?.lastVisitDate || ''
  } as UpdateUserModel), []);
  const [lastDate, setLastDate] =
    useState<Date | null>(selectedUser
      ? parse(selectedUser.lastVisitDate, 'dd.MM.yyyy', new Date())
      : null);
      
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
          spacing={1}
          direction='column'
          sx={{padding: '1rem', minWidth: '20rem'}}
        >
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
            <LocalizationProvider
              dateAdapter={DateFnsAdapter} 
              locale={ru}
            >
              <DatePicker
                label="Дата посещения"
                value={lastDate}
                onChange={(newDate) => {
                  if (newDate) {
                    setLastDate(newDate);
                    setFieldValue('lastVisitDate', format(newDate, 'dd.MM.yyyy'));
                  }
                }}
                renderInput={(params) => <TextField fullWidth {...params} />}
              />
            </LocalizationProvider>
          </Grid>
          <ButtonGroup
            variant="outlined"
            sx={{marginLeft: 'auto', marginTop: '.3rem'}}
          >
            <Button
              color='warning'
              type='submit'
            >Изменить</Button>
            <Button
              color='error'
              onClick={async () => {
                await dispatch(deleteUserAsync(selectedUser.id, from));
                closeModal();
              }}
            >Удалить</Button>
          </ButtonGroup>
        </Grid>
      </Form>
    )}
  </Formik>
};