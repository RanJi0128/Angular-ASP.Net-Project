
export const URI = {
  latest: 'discussion/posts/latest',
  posts: 'discussion/posts',
  users: 'discussion/users',
  comments: 'discussion/comments'
}

interface Geo {
  lat: string;
  lng: string;
}

interface Address {
  street: string;
  suite: string;
  city: string;
  zipcode: string;
  geo: Geo;
}

interface Company {
  name: string;
  catchPhrase: string;
  bs: string;
}

export interface User {
  id: number;
  name: string;
  username: string;
  email: string;
  address: Address;
  phone: string,
  website: string;
  company: Company;
  _posts: number;
  _comments: number;
}

export interface Post {
  userId: number;
  id: number;
  title: string;
  body: string;
  _userName: string;
  _comments: number;
}

export interface Comments {
  id: number;
  postId: number;
  name: string;
  email: string;
  body: string;
  _userId: number;
  _userName: string;
  _postTitle: string;
}
