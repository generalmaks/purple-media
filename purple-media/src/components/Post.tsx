import { useEffect, useState } from "react";
import Comment, { CommentProps } from "./Comment";

type PostProps = {
    postId: number,
    content: string;
    createdAt: Date,
    username: string,
    pfp: string,
    commentsCount: number,
    likes: number
  };

const Post: React.FC<PostProps> = ({ postId, content, createdAt, username, pfp, commentsCount, likes }) => {
    const [comments, setComments] = useState<CommentProps[]>([]);
    useEffect(() => {
        fetch(`http://localhost:5101/api/Comment/ByPost/${postId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            })
            .then((data: any[]) => {
                console.log('Fetched data:', data);
                setComments(data.map(comment => ({
                    commentId: comment.commentId,
                    content: comment.content,
                    createdAt: new Date(comment.createdAt),
                    username: comment.username,
                    pfp: '/src/assets/pfps/' + comment.profilePicturePath, // Update image path if needed
                    likes: comment.likes,
                    commentsCount: comment.commentsCount,
                })));
            })
            .catch(error => {
                console.error('Error fetching comments:', error);
            });
    }, []);
    return (
        <div className="w-full max-w-2xl">
            <div className="flex flex-col w-full bg-purple-800 p-10 rounded-4xl border-5 border-black space-y-2">
                <div className="flex flex-row space-x-3">
                    <img src={pfp} alt="Profile Picture" className="w-10 h-10 rounded-full" />
                    <h2 className="text-2xl font-bold">{username}</h2>
                </div>
                <p className="text-lg">{content}</p>
                <p className="text-sm">{createdAt.toDateString()}</p> {/* Already formatted as string */}
                <div className="flex flex-row space-x-3">
                    <p className="text-lg">Likes: {likes}</p>
                    <p className="text-lg">Comments: {commentsCount}</p>
                </div>
                {comments.map((comment) => (
                    <Comment
                        commentId={comment.commentId}
                        key={comment.commentId}
                        content={comment.content}
                        createdAt={comment.createdAt}
                        username={comment.username}
                        pfp={comment.pfp}
                        likes={comment.likes}
                        commentsCount={comment.commentsCount}
                    />
                ))}
            </div>
        </div>
    );
}

export default Post;
