

## Challenge Deliverables

The first few deliverable bulet-points basically establish a development environment:

- Create a website using Angular on the front end and an ASP.NET API on the back end
- Use VS2017 and follow the instructions (at http://www.talkingdotnet.com/how-to-create-an-angular-5-app-with-visual-studio-2017/) to create the project template.
- Add a page that uses the ASP.NET API and gets its data from http://jsonplaceholder.typicode.com/posts

All good.  This is a pretty nice tutorial and a good platform.  It was a good starting point I have been enjoying this project to refresh on Visual Studio and C# and to work with Angular 5.

### Challenge Page

The more specific points on the challenge deliverables are:

- Display 10 latest posts by ID
- Have a way to filter by User ID and show only their posts

The posts records include only the userId associated with the post.  I added the foreign key lookup to show the user name on each record.

The sample data is served from the jsonplaceholder.typicode.com API in id sequence.
I am assuming that this is by design, so am not sorting the received sample data.
This seems like a fair assumption because most databases should/could/would easily return data queries sorted by the record ids.
If this turns out to be an incorrect assumption, the posts received will need to be explicitly sorted, at least in GetLatest and GetLatestByFieldValue.
Because the posts are served in id sequence, the posts with the highest post ids will be the last 10 posts of the array of posts returned from the sample API.

It is fair, in the absence of post timestamps to presume that the post id indicates chronological sequencing as the challenge suggests.
You would expect that post ids would be generated in chronological sequence - get each new post's id by incrementing the id of the chronological previous post.

However the sample data is not realistic in that regard.
Post ID's 1 - 10 are all associated with user id 1.
Post ID's 11-20 are all associated with user id 2.
So, the sample data fails for demonstration purposes because it shows the latest 10 posts (post ids 91-100) to be all from the same user (user id 10).

I assume this is a flaw in the sample data, so the '/discussion/posts/latest' API delivers the challenge functionality by using the post id to indicate chronological order (the higher the post id, the more recent the post).

The first cut at the challenge, the 'Challenge' page, demonstrates this.  The initial query shows the 10 latest posts.  Unfortunately they are all from the same user.

The 'Challenge' page also demonstrates the next problem with the base requirements.
Query by any user, but you see results only when you query for user id 10 because this page, as the challenge requrements might be interpreted only filters for user among the most recent posts.

I can imagine a use case for the filter by user id to only be among the latest posts, but I expect that the most common use case would be "to show the latest 10 posts from the selected user" rather than "to show the posts for the selected user from among the latest 10 posts".

### Latest Posts Page

I took this to the next level to produce a page with these improvements:

- Display the 10 latest posts over all or for the selected user id.
- Dynamically queries the server for each user id - rather than merely filtering the list on the client side, it queries for the latest post for the user id from the serever.

It might be interesting to fetch, on the original list, not just the latest 10 posts overall, but the latest 10 posts for each user or all posts.
This would allow a client-side filtering by user id as I have implemented.  But this would not scale well as users are added.  I didn't go there.

So, I just implemented drop-down selection list of users and re-queried the API for each change of user.

The received post record is normalized - It includes the user id, but not the user name, which is needed for usability.
So, the client side Posts controller looks up the user name associated with each user id.
For better usability, the user selection box shows the id and name for each user.
Also, for readability, the list of posts shows the user name along with the user id for each post when viewing the latest posts among all users.

It is useful that the list of latest posts for all users shows the user id and name.
But, when we show the latest posts for a particular user, the user id and name for each post is redundant.
So, we hide those columns in the client side view with an angular directive when a user is selected.

C:\Users\Jim\source\repos\SellerActiveChallenge\ClientApp\src\app\posts\

### Posts Page

The Posts is similar to the above except that it shows all posts (or all possts for the selected user).

I am starting, for my own enjoyment and as a learning experience, to dig deeper with a more robust "drill in" architecture.
This next version will present a users page with a button on each record to drill in to the posts for that user.
Then, the posts page will show the user information at the top and the list of posts for the usere will include a button to drill in to the comments for that post.
And the comments page will show the user and post information at the top and the list of comments for that post.

I commented out the prototype users and comments pages for now.

Notice that the server api is already built to support this next version.

## The Server

### The Controller exposes an api with these routes (documented in the static ROUTES class in the discussion controller:

- /discussion/users - list all users
- /discussion/users/{id} - fetch user by id (PK)
- /discussion/users/email/{email} - fetch user by email (AK)

- /discussion/posts - list all posts
- /discussion/posts/{id} - fetch post by id (PK)
- /discussion/posts/user/{userId} - list posts by user id (FK-users)
- /discussion/posts/latest - list latest (by post id) 10 posts
- /discussion/posts/latest/{userId} - list latest (by post id) 10 posts for a user id

- /discussion/comments - list all comments
- /discussion/comments/{id} - fetch comment by id (PK)
- /discussion/comments/post/{postId} - list comments for post id (FK-posts)

### The Controller serves from a data model layer for the static sample challenge database with these public methods:

The data model layer interogates the sample data (served from http://jsonplaceholder.typicode.com/) to expose:

- GetTableForEntity(entity): returns records for entity (Users, Posts, Comments)
- GetLatest(entity, pageSize): returns latest (pageSize) records for entity
- GetRecordById(entity, id): returns entity record with indicated id
- GetRecordByFieldValue(entity, field, value): returns entity record by a field value
- GetTableByFieldValue(entity, field, value): returns entity records by a field value
- GetLatestByFieldValue(entity, field, value, pageSize): returns latest (pageSize) records by a field value

The next task, if we were to move the sample to a useful data store might be to implement a database to reflect the sample.

The relational model for the sample data is:

	USER
	id (PK)
	email (AK - note that comments use the email as a foreign key)
	username (AK - usernames probably will need to be distinct to support login by username)
	name
	address
	phone
	website
	company

	POST
	id (PK)
	userId (FK-user)
	title
	body

	COMMENT
	PK  id (PK)
	postId (FK-post)
	name
	email (FK-user with AK)
	body

Note that in the sample database, each user record is a nested structure.
The address and company fields are objects with properties.
And, the address object has a property (geo) which itself is an object with properties.

The relational database model here might handle this in any of these ways:

1. Flatten the structured records to something like this:

	USER
	id (PK)
	email (AK)
	username (AK)
	name
	address_street
	address_suite
	address_city
	address_zipcode
	address_geo_lat
	address_gel_lng
	phone
	website
	company_name
	company_catchPhrase
	company_bs

2. Manage address and/or company fields as json.

3. Normalize the data structure with distinct address, company and geo entities and appropriate foreign key references.
This might make sense especially if the use case involves multiple users that share a company.

4. Or, use a non relational model.
The user table would work niceley, for example, keyed by 'user|' + userId and serving the user object or json.

## Deficiencies and Some Next Steps

- Unit Tests
- Users/Posts/Comments drill-in application as discussed above.
- Establish a real database layer.
- REST Authentication tokens

## Original Challenge Requirement

Create a Website that consumes a RESTFUL API

Goals
- Show that you can create a website
- Show that you consume an API
- Show that you know OO principles
- Show that you understand C# and Angular
- Show that you can use an IDE (Visual Studio) to create code

Technologies
- Angular
- CSS
- ASP.NET
- C#
- Visual Studio
- JSON

Deliverables
- Create a website using Angular on the front end and an ASP.NET API on the back end
  - Use VS2017 and follow the instructions here to create the project template
- Add a page that uses the ASP.NET API and gets its data from: http://jsonplaceholder.typicode.com/posts
  - Display 10 latest posts by ID
  - Have a way to filter by User ID and show only their posts.

Nice to Haves
- Format the display using CSS
- Make the app flexible enough that it can consume any of these APIs: http://jsonplaceholder.typicode.com/
