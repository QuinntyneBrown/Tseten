export interface Comment {
  commentId: string;
  parentCommentId?: string;
  body: string;
  author: string;
  resolved: boolean;
  comments: Comment[];
}
