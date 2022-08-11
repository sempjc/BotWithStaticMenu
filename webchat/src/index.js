import React from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App';

(async function () {
    const res = await fetch('tokengenerator');
    const { token, userId } = await res.json();
    const root = createRoot(document.getElementById('root'))
    root.render(
        <App token={token} userid={userId} />,
    );
})().catch(err => console.log(err));