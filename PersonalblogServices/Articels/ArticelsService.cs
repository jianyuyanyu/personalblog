using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using X.PagedList;

namespace PersonalblogServices.Articels
{
    public class ArticelsService : IArticelsService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _generator;
        private readonly IHttpContextAccessor _accessor;
        public ArticelsService(MyDbContext myDbContext, IMapper mapper,
            LinkGenerator linkGenerator, IHttpContextAccessor accessor)
        {
            _myDbContext = myDbContext;
            _mapper = mapper;
            _generator = linkGenerator;
            _accessor = accessor;
        }

        public Post AddPost(Post post)
        {
            try
            {
                _myDbContext.posts.Add(post);
                _myDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return post;
        }

        public Post GetArticels(string pid)
        {
            Post post = _myDbContext.posts.Include("Categories").First(p => p.Id == pid);
            //Post post = _myDbContext.posts.FirstOrDefault(p => p.Id == pid);
            //var c = post.Categories;
            //post.Categories = _myDbContext.categories.First(c => c.Id == post.CategoryId);
            return post;
        }

        public IPagedList<Post> GetPagedList(QueryParameters param)
        {
            if (param.CategoryId != 0)
            {
                return _myDbContext.posts.Where(p => p.CategoryId == param.CategoryId).ToList().ToPagedList(param.Page, param.PageSize);
            }
            else
            {
                return _myDbContext.posts.ToList().ToPagedList(param.Page, param.PageSize);
            }
        }

        public List<Post> GetPhotos()
        {
            return _myDbContext.posts.ToList();
        }

        public PostViewModel GetPostViewModel(Post post)
        {
            var model = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Summary = post.Summary ?? "（没有介绍）",
                Content = post.Content ?? "（没有内容）",
                Path = post.Path ?? string.Empty,
                Url = _generator.GetUriByAction(
        _accessor.HttpContext!,
        "Post", "Blog",
        new { post.Id }
    ),
                CreationTime = post.CreationTime,
                LastUpdateTime = post.LastUpdateTime,
                Category = post.Categories,
                Categories = new List<Category>(),
                CommentsList =  _myDbContext.comments.Where(c =>c.ArticleId==post.Id).ToList(),
                ConfigItem = _myDbContext.configItems.FirstOrDefault()
            };
            return model;
        }
    }
}
