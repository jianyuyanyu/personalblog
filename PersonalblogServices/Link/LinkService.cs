using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.Links
{
    public class LinkService : ILinkService
    {
        private readonly MyDbContext _myDbContext;
        public LinkService(MyDbContext myDbContext) 
        {
            _myDbContext = myDbContext;
        }
        public List<Link> GetAll(bool onlyVisible = true)
        {
            return _myDbContext.links.Where(a=>a.Visible).ToList();
        }
    }
}
