import { Avatar, Card, CardContent, CardHeader, Typography } from "@mui/material";
import { green, red } from "@mui/material/colors";
import { fontWeight } from "@mui/system";
import { useTypedSelector } from "../../hooks/useTypedSelector";

export const UserDetails : React.FC = () => {
  const {selectedUser} = useTypedSelector(s => s.users);

  if (selectedUser === null) {
    return <Typography
      variant="caption"
      sx={{color: red[500]}}
    >Такого пользователя не существует</Typography>
  }
  return <Card sx={{position: 'relative'}}>
    <CardHeader
      avatar={<Avatar sx={{bgcolor: green[300]}}>{selectedUser.name[0]}</Avatar>}
      title={selectedUser.name}
      subheader={selectedUser.login}
    />
    <CardContent sx={{paddingTop: 0}}>
      <Typography
        variant="body2"
        color="text.secondary"
      ><span style={{fontWeight: 'bold'}}>Тип пользователя:</span> {selectedUser.typeName}</Typography>
      <Typography
        variant="body2"
        color="text.secondary"
      ><span style={{fontWeight: 'bold'}}>Последняя дата входа:</span> {selectedUser.lastVisitDate}</Typography>
    </CardContent>
  </Card>
};