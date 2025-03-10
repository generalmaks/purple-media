export type CommentProps = {
    commentId: number,
    content: string;
    createdAt: Date,
    username: string,
    pfp: string,
    commentsCount: number,
    likes: number
  };

const Comment: React.FC<CommentProps> = ({ content, createdAt, username, pfp, commentsCount, likes }) => {
    return (
        <div className="w-full max-w-2xl">
            <div className="flex flex-col w-full bg-purple-700 p-10 rounded-4xl border-4 border-black space-y-2">
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
            </div>
        </div>
    );
}

export default Comment;