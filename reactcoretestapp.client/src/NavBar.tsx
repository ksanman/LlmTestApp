import './NavBar.css';
import { useState, useTransition } from "react";
import { Home } from "./Home";
import { Documents } from "./Documents";
import { Search } from './Search';
import { Chat } from './IntelliChat';

export function NavBar(){
    const [currentTab, setCurrentTab] = useState('home');
    return (
        <>
            <nav className="navigation">
                <a onClick={() => setCurrentTab('home')} className="brand-name">
                    GPT Test App
                </a>
                <div
                    className="navigation-menu">
                    <ul>
                        <li>
                            <NavButton isActive={currentTab === 'home'} 
                                onClick={() => setCurrentTab('home')}>Home</NavButton>
                        </li>
                        <li>
                            <NavButton isActive={currentTab === 'search'} 
                                    onClick={() => setCurrentTab('search')}>Search</NavButton>
                        </li>
                        <li>
                            <NavButton isActive={currentTab === 'chat'} 
                                    onClick={() => setCurrentTab('chat')}>Chat</NavButton>
                        </li>
                        <li>
                            <NavButton isActive={currentTab === 'documents'} 
                                    onClick={() => setCurrentTab('documents')}>Manage Documents</NavButton>
                        </li>
                    </ul>
                </div>
            </nav>
            <div className="main">
                {currentTab === 'home' && <Home onItemClick={(tab: string) => setCurrentTab(tab)}/>}
                {currentTab === 'documents' && <Documents/>}
                {currentTab === 'search' && <Search/>}
                {currentTab === 'chat' && <Chat/>}
            </div>
        </>
    );
}

interface NavButtonProps {
    children: React.ReactNode,
    isActive: boolean,
    onClick: () => void;
}

export function NavButton({children, isActive, onClick}: NavButtonProps) {
    const [isPending, startTransition] = useTransition();
    if (isActive) {
        return <b>{children}</b>
    }
    if (isPending) {
    return <b className="pending">{children}</b>;
    }
    return (
    <a className="nav-button" 
        onClick={() => {
            startTransition(() => {
                onClick();
        });
    }}>
        {children}
    </a>
    );
}
