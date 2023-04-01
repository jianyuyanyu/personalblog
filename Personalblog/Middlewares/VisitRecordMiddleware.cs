using Microsoft.EntityFrameworkCore;
using Personalblog.Extensions;
using Personalblog.Model;
using SixLabors.ImageSharp;

namespace Personalblog.Middlewares
{
    public class VisitRecordMiddleware
    {
        private readonly RequestDelegate _next;
        public VisitRecordMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(Microsoft.AspNetCore.Http.HttpContext context,MyDbContext myDbContext)
        {
            var request = context.Request;
            var response = context.Response;
            DateTime serverTime = DateTime.Now.ToUniversalTime(); // 获取服务器时间并转换为协调世界时
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"); // 获取中国标准时间的时区信息
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, cstZone);
            myDbContext.visitRecords.Add(new Model.Entitys.VisitRecord
            {
                Ip = context.GetRemoteIPAddress()?.ToString().Split(":")?.Last(),
                RequestPath = request.Path,
                RequestQueryString = request.QueryString.Value,
                RequestMethod = request.Method,
                UserAgent = request.Headers.UserAgent,
                Time = cstTime
            });
            myDbContext.SaveChanges();
            return _next(context);
        }
    }
}
