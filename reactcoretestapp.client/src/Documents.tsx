import { useState, useEffect } from "react";
import './Documents.css';
import { Modal } from './Modal';

enum DocType {
    Text = 0,
    Pdf,
    Html,
    Url
}

interface Doc {
    id: string;
    title: string;
    author: string;
    description: string;
    content: string;
    text: string;
    type: DocType
}

export function Documents() {
    const [documents, setDocuments] = useState<Doc[]>([]);
    const [isModalOpen, setIsModalOpen] = useState(false);

    useEffect(() => {
        fetch('api/document')
            .then(res => res.json())
            .then(ds => setDocuments(ds as Doc[]));
      }, []);

    const openModal = () => {
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        // Handle the form submission logic here
        // You can access form data using event.target elements
        // For example: event.target.title.value, event.target.author.value, etc.
        const form = e.target as any;

        const doc: Doc = {
            id: '',
            title: form.title.value,
            author: form.author.value,
            description: form.description.value,
            content: form.content.value,
            type: DocType.Text,
            text: ''
        }

        const requestOptions: RequestInit = {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json', // Adjust the content type based on your server requirements
            },
            body: JSON.stringify(doc),
          };
      
        try {
            const response = await fetch('api/document', requestOptions);
        
            if (response.ok) {
                closeModal(); // Close the modal after successful submission
            } else {
                console.error('Failed to create document', response);
                alert('Failed to create document');
                closeModal(); // Close the modal after form submission
            }
        } catch (error) {
                console.error('Failed to create document', error);
                
                alert('Failed to create document');
                closeModal(); // Close the modal after form submission
        }
    };

    return (
        <div className="documents">
            <div className="doc-tools">
                <h3> Documents </h3>
                <div className="doc-actions">
                    <button type="button" onClick={openModal}>Add</button>
                </div>
            </div>
            <div className="docsContainer">
                {documents.map((d, i) => {
                    return (
                        <Doc doc={d} key={i}/>
                    )
                })}
            </div>
            <Modal isOpen={isModalOpen} onClose={closeModal}>
                <h2>Add Document</h2>
                <form className="add-form" onSubmit={handleSubmit}>
                    <label>
                        Title
                        <input type="text" id="title" name="title" required/>
                    </label>
                    <label>
                        Author
                        <input type="text" id="author" name="author"/>
                    </label>
                    <label>
                        Description
                        <input type="text" id="description" name="description"/>
                    </label>
                        
                    <label>
                        Content
                        <textarea rows={3} cols={3} id="content" name="content" required></textarea>
                    </label>
                    
                    <div className="actions">
                        <button type="button" className="cancel" onClick={closeModal}>Cancel</button>
                        <input type="submit" value="Add" className="submit" />
                    </div>
                </form>
            </Modal>
        </div> 
    )
}
interface DocProps {
    doc: Doc;
}

export function Doc({doc}: DocProps) {
    return (
        <div className="doc">
            <span>{doc.title}</span>
            <div>{doc.content}</div>
        </div>
    );
}