using System;
using Microsoft.AspNetCore.Mvc;

using Models      = SellerActiveChallenge.Models;
using ActiveModel = SellerActiveChallenge.Models.Typicode;
using User        = SellerActiveChallenge.Models.User;
using Post        = SellerActiveChallenge.Models.Post;
using Comment     = SellerActiveChallenge.Models.Comment;

namespace SellerActiveChallenge.Controllers
{
    [Route("discussion")]
    public class DiscussionController : Controller
    {
        [HttpGet("users")] // List all Users
        public IActionResult ListAllUsers() =>
            ListAll<User>(ActiveModel.USERS);

        [HttpGet("users/{id}")] // Fetch User by id
        public IActionResult FetchUserById(int id) =>
            FetchById<User>(ActiveModel.USERS, id);

        [HttpGet("users/email/{email}")] // Fetch User by email
        public IActionResult FetchUserByEmail(string email) =>
            FetchByAlternateKey<User>(ActiveModel.USERS, ActiveModel.EMAIL, email);

        [HttpGet("posts")] // List all Posts
        public IActionResult ListAllPosts() =>
            ListAll<Post>(ActiveModel.POSTS);

        [HttpGet("posts/{id}")] // Fetch Post by id
        public IActionResult FetchPostById(int id) =>
            FetchById<Post>(ActiveModel.POSTS, id);

        [HttpGet("posts/user/{userId}")] // fetch Posts by User id
        public IActionResult ListPostsForUserId(int userId) =>
            ListForField<Post>(ActiveModel.POSTS, ActiveModel.USERID, userId);

        [HttpGet("posts/latest")] // fetch latest Posts
        public IActionResult ListLatestPosts() =>
            ListLatest<Post>(ActiveModel.POSTS);

        [HttpGet("posts/latest/{userId}")] // fetch latest Posts for User id
        public IActionResult ListLatestPostsForUserId(int userId) =>
            ListLatestByFieldValue<Post>(ActiveModel.POSTS, ActiveModel.USERID, userId.ToString());

        [HttpGet("comments")] // fetch all comments
        public IActionResult ListAllComments() =>
            ListAll<Comment[]>(ActiveModel.COMMENTS);

        [HttpGet("comments/{id}")] // fetch comment by id
        public IActionResult FetchCommentById(int id) =>
            FetchById<Comment>(ActiveModel.COMMENTS, id);

        [HttpGet("comments/post/{postId}")] // fetch comments for a Post id
        public IActionResult ListCommentsForPostId(int postId) =>
            ListForField<Comment>(ActiveModel.COMMENTS, ActiveModel.POSTID, postId);

        // Page size for "show latest"
        private const int PAGESIZE = 10;  // maximum number of items on latest page

        private IActionResult ListAll<T>(String entity)
        {
            try
            {
                return Json(ActiveModel.GetTableForEntity<T>(entity));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult FetchById<T>(String entity, int id)
        {
            try
            {
                return Json(ActiveModel.GetRecordById<T>(entity, id));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult ListForField<T>(String entity, String field, int value)
        {
            try
            {
                return Json(ActiveModel.GetTableByFieldValue<T>(entity, field, value.ToString()));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult FetchByAlternateKey<T>(String entity, String field, string email)
        {
            try
            {
                return Json(ActiveModel.GetRecordByFieldValue<T>(entity, field, email));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult ListLatest<T>(String entity)
        {
            try
            {
                return Json(ActiveModel.GetLatest<T>(entity, PAGESIZE));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult ListLatestByFieldValue<T>(String Entity, String Field, String Value)
        {
            try
            {
                return Json(ActiveModel.GetLatestByFieldValue<T>(Entity, Field, Value, PAGESIZE));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private NotFoundResult NotFoundHandler(Exception e)
        {
            if (e.Message.Contains("404") || e.Message.Contains("Not Found"))
                return NotFound();
            else
                throw e;
        }

    }
}
