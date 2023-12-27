import './Home.css';

export interface HomeProps {
    onItemClick: (linkName: string) => void;
}

export function Home({onItemClick}: HomeProps) {
    return (
        <div className="home-container">
            <div className="header">
                <h3>Welcome!</h3>
            </div>
            <div className="body">
                <div className="card">
                    <div className="card-header">
                        <div className="title">Search Documents</div>
                        <div className="subtitle">Search documents using vector search</div>
                    </div>
                    <div className="card-body">
                        <button className="card-button" type="button" onClick={() => onItemClick('search')}>Search with Chroma</button>
                    </div>
                </div>
                <div className="card">
                    <div className="card-header">
                        <div className="title">Search Documents with LLM</div>
                        <div className="subtitle">Search documents using vector search and an LLM.</div>
                    </div>
                    <div className="card-body">
                        <button className="card-button" type="button" onClick={() => onItemClick('chat')}>Search with LLAMA</button>
                    </div>
                </div>
                <div className="card">
                    <div className="card-header">
                        <div className="title">Manage Documents</div>
                        <div className="subtitle">Manage Documents</div>
                    </div>
                    <div className="card-body">
                        <button className="card-button" type="button" onClick={() => onItemClick('documents')}>Manage</button>
                    </div>
                </div>
            </div>
        </div>     
    )
}