import { FormEvent, useState } from "react"
import './Search.css'
import { SearchResult } from "./models";


export function Search() {
    const [isSearching, setIsSearching] = useState(false);
    const [results, setResults]= useState<SearchResult[]>([]);
    const [isFirstSearch, setIsFirstSearch] = useState(true);
    const [searchTime, setSearchTime] = useState('');

    const search = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setIsFirstSearch(false);
        setIsSearching(true);
        const query = (e.target as any).searchBox.value;
        const url = '/api/document/query';
        const requestOptions: RequestInit = {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json', // Adjust the content type based on your server requirements
            },
            body: JSON.stringify({'query': query})
        };

        try
        {
            const start = new Date().getTime();
            const response = await fetch(url, requestOptions);
            if(response.ok) {
                const results = await response.json() as SearchResult[];
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
                setSearchTime(`${minutes}:${formattedSeconds}.${formattedMilliseconds}`);
                setResults(results);
            } else {
                console.error("Error querying document");
            }

            setIsSearching(false);
        }
        catch(error) {
            alert("Error querying documents");
            console.error("Error querying document", error);
            setIsSearching(false);
        }

        
        (e.target as any).searchBox.value = '';
    }

    return (
        <div className="search-container">
            <div className="search-header">
                <span>Search Documents</span>
            </div>
            <div className="search-bar">
                <form onSubmit={search}>
                    <input name="searchBox" type="search" placeholder="Enter search here..." id="searchBox" disabled={isSearching}/>
                    <input type="submit" value="Go!" id="searchButton" disabled={isSearching}/>
                </form>
            </div>
            {isSearching 
            ? <div className="searching"> <span>Searching...</span> </div> 
            : <div className="results">
                {results.length > 0 && <div>Results in {searchTime} </div>}
                {results.length === 0 && !isFirstSearch ? <div>No Results!</div> : results.map((r, i) => {
                    return (
                        <Result result={r} key={i}/>
                    )
                })}
              </div>
            }
        </div>
    )
}
interface ResultProps {
    result: SearchResult;
}
export function Result({result}: ResultProps) {
    return (
        <div className="result">
            <span className="title">{result.document.title}</span>
            <span className="text"><b>Source:</b> {result.text}</span>
        </div>
    )
}