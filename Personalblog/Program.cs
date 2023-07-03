using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Personalblog.Contrib.SiteMessage;
using Personalblog.Filter;
using Personalblog.Middlewares;
using Personalblog.Model;
using Personalblog.Models.Config;
using Personalblog.Services;
using PersonalblogServices;
using PersonalblogServices.Articels;
using PersonalblogServices.Categorys;
using PersonalblogServices.Config;
using PersonalblogServices.FCategory;
using PersonalblogServices.FPhoto;
using PersonalblogServices.FPost;
using PersonalblogServices.FtopPost;
using PersonalblogServices.Links;
using SixLabors.ImageSharp.Web.DependencyInjection;
using StackExchange.Profiling.Storage;
using System.Text;
using PersonalblogServices.CommentService;
using PersonalblogServices.Notice;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllersWithViews(
    options => { options.Filters.Add<ResponseWrapperFilter>(); }
);
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.WebHost.UseUrls("http://*:7031");
// Add services to the container.
builder.Services.AddControllersWithViews();
//���ݿ�����
builder.Services.AddDbContext<MyDbContext>(opt =>
{
    //opt.UseMySql(connStr, new MySqlServerVersion(new Version(5, 7, 40)));   
    string connStr = "Data Source=app.db";
    opt.UseSqlite(connStr);
});
//����
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        opt => opt.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("http://localhost:8080/"));
});
//ע��AotoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
//�����ע��
builder.Services.AddTransient<IArticelsService, ArticelsService>();
builder.Services.AddTransient<IPhotoService, PersonalblogServices.PhotoService>();
builder.Services.AddTransient<IFPhotoService, FPhotoService>();
builder.Services.AddTransient<IFCategoryService, FCategoryService>();
builder.Services.AddTransient<ITopPostService, TopPostService>();
builder.Services.AddTransient<IFPostService, FPostService>();
builder.Services.AddTransient<ILinkService, LinkService>();
builder.Services.AddTransient<Icommentservice, commentservice>();
builder.Services.AddTransient<INoticeService, NoticeService>();

builder.Services.AddHttpContextAccessor();
// ע�� IHttpClientFactory���ο���https://docs.microsoft.com/zh-cn/dotnet/core/extensions/http-client
builder.Services.AddHttpClient();
//ע���Զ������
builder.Services.AddScoped<ICategoryService, PersonalblogServices.Categorys.CategoryService>();
builder.Services.AddScoped<Personalblog.Services.PhotoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ConfigService>();
builder.Services.AddScoped<Personalblog.Services.CategoryService>();
builder.Services.AddScoped<VisitRecordService>();
builder.Services.AddSingleton<Messages>();
builder.Services.AddSingleton<CommonService>();
builder.Services.AddSingleton<PiCLibService>();
builder.Services.AddSingleton<CrawlService>();

//ע��jwt����
builder.Services.Configure<SecuritySetting>(builder.Configuration.GetSection(nameof(SecuritySetting)));
//ע��Miniprofiler
builder.Services.AddMiniProfiler(options =>
{
    //���ʵ�ַ·�ɸ�Ŀ¼��Ĭ��Ϊ��/mini-profiler-resources
    options.RouteBasePath = "/profiler";
    //���ݻ���ʱ��
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
    //sql��ʽ������
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    //�������Ӵ򿪹ر�
    options.TrackConnectionOpenClose = true;
    //����������ɫ����;Ĭ��ǳɫ
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
    //.net core 3.0���ϣ���MVC���������з���
    options.EnableMvcFilterProfiling = true;
    //����ͼ���з���
    options.EnableMvcViewProfiling = true;
}).AddEntityFramework();
//���jwt
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        // �����õ�����֮ǰ����õ�������
        var secSettings = builder.Configuration.GetSection(nameof(SecuritySetting)).Get<SecuritySetting>();
        // ����jwt token�ĸ�����Ϣ������֤
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = secSettings.Token.Issuer,
            ValidAudience = secSettings.Token.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secSettings.Token.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });
//ͼƬ����ͼ
// ע�����
builder.Services.AddImageSharp();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//�Ƿ�������
//app.UseMiniProfiler();

app.UseHttpsRedirection();

app.UseStaticFiles();

//���������ȡ����
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

//�м�� ���ڱ���ӿڷ�����Ϣ
//app.UseMiddleware<VisitRecordMiddleware>();
app.UseVisitRecordMiddleware();

//��������
app.UseCors("CorsPolicy");

// ����м��
app.UseImageSharp();

app.UseRouting();


//����jwt����
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
