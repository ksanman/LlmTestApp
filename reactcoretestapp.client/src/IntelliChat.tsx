import { FormEvent, useState } from "react"
import './IntelliChat.css'
import { ChatRequest, ChatResponse } from "./models";


export function Chat() {
    const [isChatting, setIsChatting] = useState(false);
    const [chatResponse, setChatResponse]= useState<ChatResponse | undefined>(undefined);

    const chat = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setIsChatting(true);
        const query = (e.target as any).chatBox.value;
        const chat: ChatRequest = {
            userText: query,
            history: []
        };
        const url = '/api/chat';
        const requestOptions: RequestInit = {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json', // Adjust the content type based on your server requirements
            },
            body: JSON.stringify(chat)
        };

        try
        {
            const response = await fetch(url, requestOptions);
            if(response.ok) {
                const results = await response.json() as ChatResponse;
                setChatResponse(results);
            } else {
                console.error("Error querying document");
            }

            setIsChatting(false);
        }
        catch(error) {
            alert("Error querying documents");
            console.error("Error querying document", error);
            setIsChatting(false);
        }

        
        (e.target as any).chatBox.value = '';
    }

    return (
        <div className="chat-container">
            <div className="chat-header">
                <span>Intelli Search Documents</span>
            </div>
            <div className="chat-bar">
                <form onSubmit={chat}>
                    <input name="chatBox" type="search" placeholder="Enter chat here..." id="chatBox"/>
                    <input type="submit" value="Go!" id="chatButton"/>
                </form>
            </div>
            {isChatting 
            ? <div className="processing"> <span>Processing...</span> </div> 
            : <div className="results">
                {chatResponse !== undefined &&
                    <div className="result">
                        <div>Response: {chatResponse.responseText}</div>
                        <div>
                            {chatResponse.sources?.map((s, i) => <div>Source: {s.title}</div>)}
                        </div>
                    </div>
                }
              </div>
            }
        </div>
    )
}