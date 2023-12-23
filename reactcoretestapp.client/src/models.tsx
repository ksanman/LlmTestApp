export enum DocType {
    Text = 0,
    Pdf,
    Html,
    Url
}

export interface Doc {
    id: string;
    title: string;
    author: string;
    description: string;
    content: string;
    text: string;
    type: DocType
}

export interface SearchResult {
    text: string;
    document: Doc;
}

export interface ChatRequest {
    userText: string;
    history: string[];
}

export interface ChatResponse {
    responseText: string;
    sources: Doc[];
}
