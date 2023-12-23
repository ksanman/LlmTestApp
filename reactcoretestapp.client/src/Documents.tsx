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
        e.preventDefault();
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
                const newDoc = await response.json() as Doc;
                documents.push(newDoc);
                setDocuments(documents);
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

    const onDelete = async (id: string) => {
        const requestOptions: RequestInit = {
            method: 'DELETE',
        };

        try
        {
            const response = await fetch(`api/document/${id}`, requestOptions);
            if(!response.ok) {
                alert('Error deleting document');
                const msg = await response.json();
                console.error('Error deleting document', msg);
            } else {
                const newDocs = documents.filter(d => d.id !== id);
                setDocuments(newDocs);

            }
        } catch(error) {
            alert('Error deleting document')
            console.error('Error deleting document', error);
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
                        <Doc doc={d} key={i} onDelete={onDelete}/>
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
    onDelete: (id: string) => void;
}

export function Doc({doc, onDelete}: DocProps) {
    const handleDelete = () => {
        onDelete(doc.id);
    };  

    return (
        <div className="doc">
            <div className="doc-header">
                <div className="left">
                    <span className="title">{doc.title}</span>
                    <span className="author">{doc.author}</span>
                    <span className="desc">{doc.description}</span>
                </div>
                <div className="right">
                    <button type="button" className="delete" onClick={handleDelete}>Delete</button>
                </div>
            </div>
           
            <div className="content">{doc.content}</div>
        </div>
    );
}