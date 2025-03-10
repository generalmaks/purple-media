import './components/NavigationButton'
import './App.css'
import NavigationBar from './components/NavigationBar'
import { useState, useEffect } from 'react'
import Post from './components/Post';
import React from 'react';

function App() {
    const navigationButtons = [
        { label: "🏛️ Home", url: "/home" },
        { label: "🤝 Friends", url: "/friends" },
        { label: "⚙️ Settings", url: "/settings" },
    ];

    // Define Post type
    type PostType = {
        postId: number,
        content: string;
        createdAt: Date,
        username: string,
        pfp: string,
        commentsCount: number,
        likes: number
      };

    const [posts, setPosts] = useState<PostType[]>([]);

    useEffect(() => {
        fetch('http://localhost:5101/api/Post')
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            })
            .then((data: any[]) => {
                console.log('Fetched data:', data);

                // Process and format posts
                const fetchedPosts = data.map(post => ({
                    postId: post.postId,
                    content: post.content,
                    createdAt: new Date(post.createdAt), // Convert to Date object
                    username: post.username, 
                    pfp: '/src/assets/react.svg', // Update image path if needed
                    likes: post.likes,
                    commentsCount: post.commentsCount,
                }));

                console.log('Processed posts:', fetchedPosts);
                setPosts(fetchedPosts);
            })
            .catch(error => {
                console.error('Error fetching posts:', error);
            });

    }, []);

    return (
        <div className='flex flex-col h-screen pr-50 pl-50'>
            <h1 className="text-6xl font-bold text-center pt-10 pb-10">My App</h1>
            <div className="flex flex-row h-full">
                {/* Left Navigation bar */}
                <NavigationBar navigationButtons={navigationButtons} />

                {/* Content Column */}
                <div className="flex-grow flex flex-3/3 flex-col space-y-5 items-center pt-20 bg-white overflow-y-auto">
                    {posts.map((post) => (
                        <Post 
                            postId={post.postId}
                            key={post.postId} 
                            username={post.username} 
                            pfp={post.pfp} 
                            likes={post.likes}
                            commentsCount={post.commentsCount}
                            content={post.content} 
                            createdAt={post.createdAt} 
                        />
                    ))}
                </div>

                {/* Right Navigation bar */}
                <NavigationBar navigationButtons={navigationButtons} />
            </div>
        </div>
    );
}

export default App;
