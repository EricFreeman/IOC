using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcTestApp.Models
{
    public class Post
    {
        public int id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        public Post(int i, string title, string text, DateTime created)
        {
            id = i;
            Title = title;
            Text = text;
            DatePosted = created;
        }
    }
}