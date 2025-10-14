export interface Tweet {
  postId: number;
  content: string;
  createdAt: string;
  author: string;
  authorProfilePicturePath: string;
  parentPost: number;
  responses: number[];
  likedBy: string[];
}
