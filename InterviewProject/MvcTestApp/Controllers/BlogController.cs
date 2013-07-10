using MvcTestApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTestApp.Controllers
{
    public class BlogController : Controller
    {
        #region Private Properties

        private IDal _dal;

        #endregion

        #region Constructor

        public BlogController(IDal dal)
        {
            _dal = dal;
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult GetPost(int id)
        {
            Post post = _dal.GetPost(id);

            return View(post);
        }

        [HttpGet]
        public ActionResult GetAllPosts()
        {
            List<Post> posts = _dal.GetAllPosts();

            return View(posts);
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        #endregion
    }

    #region Data Access Layer

    public class Dal : IDal
    {
        private List<Post> Posts;

        public Dal()
        {
            Posts = new List<Post>();
            Posts.Add(new Post(1, "First Post", CreatePostText(), DateTime.Now));
            Posts.Add(new Post(2, "Second Post", CreatePostText(), DateTime.Now.AddDays(-1)));
            Posts.Add(new Post(3, "Third Post", CreatePostText(), DateTime.Now.AddDays(-12)));
            Posts.Add(new Post(4, "Last Post", CreatePostText(), DateTime.Now.AddDays(-15)));
        }

        private string CreatePostText()
        {
            return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dignissim interdum erat vel dictum. Pellentesque sed tincidunt libero, vitae tincidunt nisl. Vestibulum mattis eu felis vitae feugiat. Aenean sed eros eget elit facilisis interdum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse potenti. Sed vitae auctor ipsum. Pellentesque ac malesuada nisi. Etiam tincidunt, nunc sit amet ornare lobortis, risus lectus rutrum nisl, sed pretium risus tortor non urna. Vestibulum dapibus ac nibh non aliquet. Nullam non condimentum leo, sed viverra mauris. Phasellus pulvinar erat tortor, posuere semper nulla lobortis a. Integer sollicitudin fringilla risus, in malesuada justo gravida non. Cras vel magna vestibulum nulla placerat ornare. Pellentesque pellentesque felis vitae erat scelerisque, vel semper dolor vulputate. Praesent ac mauris congue, tincidunt leo at, ultrices quam.";
        }

        public Post GetPost(int id)
        {
            return Posts.Where(x => x.id == id).FirstOrDefault();
        }

        public List<Post> GetAllPosts()
        {
            return Posts;
        }
    }

    public interface IDal
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
    }

    #endregion
}
