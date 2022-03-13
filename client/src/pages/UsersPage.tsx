import styled from "@emotion/styled";
import { Button, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import React, { useCallback, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { ModalHOC } from "../components/HOC/ModalHOC";
import { StyledPaper } from "../components/Styled/StyledPaper";
import { UserCreate } from "../components/Users/UserCreate";
import { UserDetails } from "../components/Users/UserDetails";
import { UserEdit } from "../components/Users/UserEdit";
import { useTypedSelector } from "../hooks/useTypedSelector";
import { getUserByIdAsync } from "../store/userStore/asyncActions";

const StyledTableConteiner = styled(TableContainer)({
  maxHeight: '30rem',
  width: 'auto'
});

export const UsersPage: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const {isAccessEnabled} = useTypedSelector(s => s.authorization);
  const {users} = useTypedSelector(s => s.users);
  const {userTypes} = useTypedSelector(s => s.userTypes);
  const [showInfoModal, setShowInfoModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);

  const handleOpenModal = useCallback(async (id: number) => {
    await dispatch(getUserByIdAsync(id));
    if (isAccessEnabled) {
      setShowEditModal(true);
    } else {
      setShowInfoModal(true);
    }
  }, [isAccessEnabled]);
  const handleCloseCreateModal = useCallback(() => {
    setShowCreateModal(false);
  }, []);

  return <StyledPaper>
    <Grid container justifyContent='space-between' sx={{marginBottom: '.3rem'}}>
      <Grid item>
        <Button
          variant="outlined"
          color="warning"
          onClick={() => navigate('/auth')}
        >Сменить права доступа</Button>
      </Grid>
      {isAccessEnabled && <Grid item>
        <Button
          variant="outlined"
          color="success"
          onClick={() => setShowCreateModal(true)}
        >Создать пользователя</Button>
      </Grid>}
    </Grid>
    <StyledTableConteiner>
      <Table stickyHeader>
        <TableHead>
          <TableRow>
            <TableCell align="center">Логин</TableCell>
            <TableCell align="center">Имя</TableCell>
            <TableCell align="center">Тип</TableCell>
            <TableCell align="center">Дата последнего посещения</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {users.map(user => (<TableRow
            key={user.id}
            hover
            role="button"
            sx={{cursor: 'pointer'}}
            onClick={async () => await handleOpenModal(user.id)}
          >
            <TableCell align="center">{user.login}</TableCell>
            <TableCell align="center">{user.name}</TableCell>
            <TableCell align="center">{user.typeName}</TableCell>
            <TableCell align="center">{user.lastVisitDate}</TableCell>
          </TableRow>))}
        </TableBody>
      </Table>
    </StyledTableConteiner>

    <ModalHOC
      openFlag={showInfoModal}
      closeModal={() => setShowInfoModal(false)}
    ><UserDetails/></ModalHOC>
    <ModalHOC
      openFlag={showEditModal}
      closeModal={() => setShowEditModal(false)}
    ><UserEdit/></ModalHOC>
    <ModalHOC
      openFlag={showCreateModal}
      closeModal={handleCloseCreateModal}
    ><UserCreate closeModal={handleCloseCreateModal}/></ModalHOC>
  </StyledPaper>;
};