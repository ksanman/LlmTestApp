import React from 'react';
import './Modal.css';

interface AddDocProps { 
    isOpen: boolean;
    onClose: () => void;
    children: React.ReactNode;
};

export function Modal({ isOpen, onClose, children }: AddDocProps) {
  if (!isOpen) {
    return null;
  }

  return (
    <div className="modal-overlay">
      <div className="modal">
        <button className="close-button" onClick={onClose} type="button">
          X
        </button>
        {children}
      </div>
    </div>
  );
};