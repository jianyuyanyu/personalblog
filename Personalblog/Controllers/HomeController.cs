using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.ViewModels;
using Personalblog.Models;
using PersonalblogServices.FCategory;
using PersonalblogServices.FPhoto;
using PersonalblogServices.FPost;
using PersonalblogServices.FtopPost;
using PersonalblogServices.Links;
using System.Diagnostics;
using Personalblog.Services;
using PersonalblogServices.Notice;

namespace Personalblog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFPhotoService _fPhotoService;
        private readonly IFCategoryService _fCategoryService;
        private readonly ITopPostService _TopPostService;
        private readonly IFPostService _PostService;
        private readonly ILinkService _linkService;
        private readonly INoticeService _noticeService;
        public HomeController(ILogger<HomeController> logger,IFPhotoService fPhotoService,
            IFCategoryService fCategoryService,ITopPostService topPostService,
            IFPostService fPostService,ILinkService linkService,
            INoticeService noticeService)
        {
            _logger = logger;
            _fPhotoService = fPhotoService;
            _fCategoryService = fCategoryService;
            _TopPostService = topPostService;
            _PostService = fPostService;
            _linkService = linkService;
            _noticeService = noticeService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel homeView = new HomeViewModel()
            {
                FeaturedPhotos = _fPhotoService.GetFeaturePhotos(),
                FeaturedCategories = _fCategoryService.GetFeaturedCategories(),
                TopPost = _TopPostService.GetTopOnePost(),
                FeaturedPosts = _PostService.GetFeaturedPosts(),
                Links = _linkService.GetAll(),
                Notices = await _noticeService.GetAllAsync()
            };
            return View(homeView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}