using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using PersonalblogServices.Articels;
using PersonalblogServices.Categorys;
using Personalblog.Contrib.SiteMessage;
using PersonalblogServices.CommentService;
using System.Net.Mail;

namespace Personalblog.Controllers
{
    public class BlogController : Controller
    {
        public ICategoryService CategoryService { get; set; }
        public IArticelsService ArticelsService { get; set; }
        private Personalblog.Services.CategoryService _categoryService1 { get; set; }
        private readonly Messages _messages;
        private readonly Icommentservice _commentservice;
        public BlogController(ICategoryService categoryService,IArticelsService articelsService, 
            Messages messages, Personalblog.Services.CategoryService categoryService1,
            Icommentservice commentservice)
        {
            this.CategoryService = categoryService;
            this.ArticelsService = articelsService;
            _categoryService1 = categoryService1;
            _messages = messages;
            _commentservice = commentservice;

        }
        /// <summary>
        /// 所有文章列表
        /// </summary>
        /// <param name="categoryId">文章类别id 默认是.net文章</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">页面最大展示数据的数量</param>
        /// <returns></returns>
        public IActionResult List(int categoryId = 1, int page = 1, int pageSize = 5)
        {
            var clist = CategoryService.categories();
            var currentCategory = categoryId == 0 ? clist[0] : CategoryService.GetById(categoryId);

            if (currentCategory == null)
            {
                _messages.Error($"分类 {categoryId} 不存在！");
                return RedirectToAction(nameof(List));
            }
            if (!currentCategory.Visible)
            {
                _messages.Warning($"分类 {currentCategory.Name} 暂不开放！");
                return RedirectToAction(nameof(List));
            }

            BlogListViewModel blogList = new BlogListViewModel()
            {
                CurrentCategory = categoryId == 0 ? clist[0] : clist.First(c => c.Id == categoryId),
                CurrentCategoryId = categoryId,
                Categories = clist,
                CategoryNodes = _categoryService1.GetNodes(),
                Posts = ArticelsService.GetPagedList(new QueryParameters
                {
                    Page = page,
                    PageSize = pageSize,
                    CategoryId = categoryId,
                })
            };
            return View(blogList);
        }
        /// <summary>
        /// 根据文章id去查看文章
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [Route("blog/post/{id}.html")]
        public IActionResult Post(string id)
        {
            try
            {
                var post = ArticelsService.GetArticels(id);
                return View(ArticelsService.GetPostViewModel(post));
            }
            catch (Exception)
            {
                return Content(id);
                throw;
            }
        }
        /// <summary>
        /// 随机一篇文章
        /// </summary>
        /// <returns></returns>
        public IActionResult RandomPost()
        {
            var posts = ArticelsService.GetPhotos();
            var randPost = posts[Random.Shared.Next(posts.Count)];
            if (posts.Count == 0)
            {
                _messages.Error("当前没有文章，请先添加文章！");
                return RedirectToAction("Index", "Home");
            }
            _messages.Info($"随机推荐了文章 <b>{randPost.Title}</b> 给你~" +
                      $"<span class='ps-3'><a href=\"{Url.Action(nameof(RandomPost))}\">再来一次</a></span>");
            return RedirectToAction(nameof(Post), new { id = randPost.Id });
        }

        [HttpPost]
        public async Task<IActionResult> SubComment(Comments comments)
        {
            comments.ParentCommentId ??= 0;

            if (comments.Content == null)
            {
                _messages.Warning("请输入评论内容~");
                return RedirectToAction(nameof(Post), new { id = comments.ArticleId});
            }

            if (comments.Name == null)
            {
                _messages.Warning("请输入昵称~");
                return RedirectToAction(nameof(Post), new { id = comments.ArticleId});
            }

            if (comments.Email == null)
            {
                _messages.Warning("请输入邮箱~");
                return RedirectToAction(nameof(Post), new { id = comments.ArticleId});
            }

            comments.CreateTime = DateTime.Now;
            var data = await _commentservice.SubmitCommentsAsync(comments);
            var content = data.Data as Comments;
            try
            {
                if (comments.ParentCommentId != 0)
                {
                    var email = await _commentservice.GetEmail( comments.ParentCommentId ?? default);
                    if (email == "邮箱错误")
                    {
                        _messages.Error("邮箱错误！~");
                        return RedirectToAction(nameof(Post), new { id = comments.ArticleId});
                    }

                    try
                    {
                        SendEmail(email, content.Content, $"{comments.ArticleId}");
                    }
                    catch (Exception e)
                    {
                        _messages.Error("邮箱错误！，无法发送邮箱信息~");
                        return RedirectToAction(nameof(Post), new { id = comments.ArticleId});
                        throw;
                    }
                }
                _messages.Success("评论成功~");
            }
            catch (Exception e)
            {
                _messages.Error("邮箱错误！，无法发送邮箱信息~");
                throw;
            }
            return RedirectToAction(nameof(Post), new { id = comments.ArticleId});
        }

        public static void SendEmail(string email,string content,string link)
        {
            try
            {
                // 设置发件人、收件人、邮件主题和正文
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("1767992919@qq.com", "ZY");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "ZY知识库评论通知";
                mail.Body = $"您收到来自ZY知识库评论通知，内容如下{content}<br>点击跳转:<a href='https://pljzy.top/blog/post/{link}.html'>文章地址</a>";
                mail.IsBodyHtml = true;
                
                // 设置 SMTP 客户端
                SmtpClient client = new SmtpClient("smtp.qq.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("1767992919@qq.com", "nebttozhnztwdeeb");
                // 添加 EHLO 和 AUTH 支持
                client.TargetName = "STARTTLS/smtp.qq.com";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                // 发送邮件
                try
                {
                    client.Send(mail);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                Console.WriteLine("邮件已成功发送！");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
