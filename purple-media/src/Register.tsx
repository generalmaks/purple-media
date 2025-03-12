import React from 'react';
import { useState } from 'react';
import { Link } from 'react-router-dom';

type FormData = {
    username: string,
    email: string,
    password: string,
    confirmPassword: string
}

const Register: React.FC = () => {
    const [formData, setFormData] = useState<FormData>({
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
    })

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        })
    }

    const  handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (formData.password !== formData.confirmPassword) {
            alert('Passwords do not match!');
            return;
        }
        try {
            const response = await fetch('http://localhost:5101/api/User', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    username: formData.username,
                    password: formData.password,
                    email: formData.email
                })
            }
            )
            if (!response.ok) {
                const errorData = await response.json();
                alert('Error registering user: ' + errorData.message);
                throw new Error(errorData.message || 'Something went wrong!');
            }
        } catch (error) {
            alert('Error registering user: ' + error);
        }
    }

    return (
        <div className='space-y-6' style={{ textAlign: 'center', marginTop: '50px' }}>
            <h1 className='text-5xl m-15'>Create new account</h1>
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
                <input
                    name="confirmPassword"
                    value={formData.confirmPassword}
                    onChange={handleChange}
                    className='p-2 border-3 rounded-lg w-64'
                    type='password'
                    placeholder='Confirm password'
                />
                <button
                    className='p-2 bg-blue-500 text-white border-3 rounded-lg w-64 hover:bg-blue-600'
                    type='submit'
                >
                    Register
                </button>
            </form>
            <p>Already have an account? <Link to="/login" className='p-1 border-2 rounded-lg'>Login</Link></p>
            <Link className='p-1 border-3 rounded-lg' to="/">Go back to the main page</Link>
        </div>
    );
};

export default Register;