using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.Links
{
    public interface ILinkService
    {
        List<Link> GetAll(bool onlyVisible = true);
    }
}
