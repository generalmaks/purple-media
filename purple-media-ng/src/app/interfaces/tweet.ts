export interface Tweet {
  postId: number;
  content: string;
  createdAt: string;
  author: string;
  authorsProfilePictureId: number;
  parentPost: number;
  responses: number[];
  likedBy: string[];
}
