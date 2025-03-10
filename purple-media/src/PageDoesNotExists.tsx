import React from 'react';
import { Link } from 'react-router-dom';

const PageDoesNotExist: React.FC = () => {
    return (
        <div className='space-y-6' style={{ textAlign: 'center', marginTop: '50px' }}>
            <h1 className='text-9xl'>404</h1>
            <p className='text-4xl'>This page does not exist.</p>
            <Link className='p-1 border-3 rounded-lg' to="/">Go back to the main page</Link>
        </div>
    );
};

export default PageDoesNotExist;