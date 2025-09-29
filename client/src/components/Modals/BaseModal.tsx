import { MouseEvent } from "react";
import { createPortal } from "react-dom";
import { closeModal } from "@redux/slices/modalSlice";
import styles from "./BaseModal.module.scss";
import AddEventModal from "./AddEvent";
import { useAppDispatch, useAppSelector } from "@redux/store";
import EditEventModal from "./EditModal/EditEvent";
import CancelEventModal from "./CancelEvent";

export const BaseModal = () => {
  const dispatch = useAppDispatch();
  const { type, props } = useAppSelector((state) => state.modal);

  const handleOutsideClick = (e: MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      dispatch(closeModal());
    }
  };

  if (!type) return null;

  const portalRoot = document.getElementById("portal-root");
  if (!portalRoot) return null;

  let content;
  switch (type) {
    case "addEvent":
      content = <AddEventModal {...props} />;
      break;
    case "editEvent":
      content = <EditEventModal {...props} />;
      break;
    case "cancelEvent":
      content = <CancelEventModal {...props} />;
      break;
    // case "inviteToEvent":
    //   content = <InviteToEventModal {...props} />;
    //   break;
    default:
      return null;
  }

  return createPortal(
    <div onClick={handleOutsideClick} className={styles.ModalContainer}>
      {content}
    </div>,
    portalRoot
  );
};

export default BaseModal;
