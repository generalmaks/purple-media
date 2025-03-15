import React from 'react';
import { useState } from 'react';
import { Link } from 'react-router-dom';

type FormData = {
    username: string,
    email: string,
    password: string
}

const Login: React.FC = () => {
    const [formData, setFormData] = useState<FormData>({
        username: '',
        email: '',
        password: ''
    })

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        })
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch('http://localhost:5101/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    username: formData.username,
                    email: formData.email,
                    password: formData.password
                })
            })
            if (!response.ok) {
                const errorData = await response.json();
                alert('Error logging in: ' + errorData.message);
                throw new Error(errorData.message || 'Something went wrong!');
            }
            const data = await response.json();
            const token = data.token;
            localStorage.setItem('token', token);
            alert('Logged in successfully!');
        } catch (error) {
            alert('Error logging in: ' + error);
        }
    }

    function logOut(): void {
        localStorage.removeItem('token')
    }

    return (
        <div className='space-y-6' style={{ textAlign: 'center', marginTop: '50px' }}>
        <h1 className='text-5xl m-15'>Login to your account</h1>
        <form className='space-y-4 flex flex-col items-center' onSubmit={handleSubmit}>
            <input
                name="username"
                value={formData.username}
                onChange={handleChange}
                className='p-2 border-3 rounded-lg w-64'
                type='text'
                placeholder='Username'
            />
            <input
                name="email"
                value={formData.email}
                onChange={handleChange}
                className='p-2 border-3 rounded-lg w-64'
                type='email'
                placeholder='Email'
            />
            <input
                name="password"
                value={formData.password}
                onChange={handleChange}
                className='p-2 border-3 rounded-lg w-64'
                type='password'
                placeholder='Password'
            />
            <button
                className='p-2 bg-blue-500 text-white border-3 rounded-lg w-64 hover:bg-blue-600'
                type='submit'
            >
                Login
            </button>
        </form>
        <p>Don't have an account? <Link to="/register" className='p-1 border-2 rounded-lg'>Register</Link></p>
        <Link className='p-1 border-3 rounded-lg' to="/">Go back to the main page</Link>
        <button className='p-1 border-3 rounded-lg m-2' onClick={logOut}>Lougout</button>
    </div>
    );
};

export default Login;
