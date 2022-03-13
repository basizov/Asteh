import { Backdrop, CircularProgress } from '@mui/material';
import React from 'react';

type PropsType = {
  loading: boolean;
};

export const Loading: React.FC<PropsType> = ({ loading }) => {
  return (
    <Backdrop
      sx={{color: '#fff'}}
      open={loading}
    ><CircularProgress color="inherit"/></Backdrop>
  );
};
