import { Modal, Paper, styled } from "@mui/material";

const StyledPaper = styled(Paper)({
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  outline: 'none'
});

type PropsType = {
  openFlag: boolean;
  closeModal: () => void;
};

export const ModalHOC: React.FC<PropsType> = ({
  openFlag,
  closeModal,
  children
}) => {
  return <Modal
    open={openFlag}
    onClose={closeModal}
  >
    <StyledPaper>
      {children}
    </StyledPaper>
  </Modal>;
};