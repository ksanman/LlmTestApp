import { FormEvent, useState } from "react"
import './IntelliChat.css'
import { ChatRequest, ChatResponse } from "./models";


export function Chat() {
    const [isChatting, setIsChatting] = useState(false);
    const [chatResponse, setChatResponse]= useState<ChatResponse | undefined>(undefined);
    const [chatTime, setChatTime] = useState('');

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
            const start = new Date().getTime();
            const response = await fetch(url, requestOptions);
            if(response.ok) {
                const results = await response.json() as ChatResponse;
                const end = new Date().getTime() - start;
                const minutes = Math.floor(end / (60 * 1000));
                const seconds = Math.floor((end % (60 * 1000)) / 1000);
                const remainingMilliseconds = end % 1000;


                // Pad single-digit seconds and milliseconds with leading zeros
                const formattedSeconds = seconds < 10 ? `0${seconds}` : `${seconds}`;
                const formattedMilliseconds = remainingMilliseconds < 100
                    ? remainingMilliseconds < 10
                    ? `00${remainingMilliseconds}`
                    : `0${remainingMilliseconds}`
                    : `${remainingMilliseconds}`;
                setChatTime(`${minutes}:${formattedSeconds}.${formattedMilliseconds}`);
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
                <span>Search Documents with LLM</span>
            </div>
            <div className="chat-bar">
                <form onSubmit={chat}>
                    <input name="chatBox" type="search" placeholder="Enter chat here..." id="chatBox" disabled={isChatting}/>
                    <input type="submit" value="Go!" id="chatButton" disabled={isChatting}/>
                </form>
            </div>
            {isChatting 
            ? <div className="processing"> <span>Generating Response...</span> </div> 
            : <div className="results">
                {chatResponse !== undefined &&
                    <div className="result">
                        <div>Response: {chatResponse.responseText}</div>
                        <div>Response Time: {chatTime} </div>
                        <div>
                            {chatResponse.sources?.map((s, i) => <div key={i}>Source: {s.title}</div>)}
                        </div>
                    </div>
                }
              </div>
            }
        </div>
    )
}