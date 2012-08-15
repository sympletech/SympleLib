using System.Web;
using SympleLib.Helpers;

namespace SympleLib.MVC
{
    public class KendoGridPost
    {
        public KendoGridPost()
        {
            if (HttpContext.Current != null)
            {
                HttpRequest curRequest = HttpContext.Current.Request;
                this.Page = curRequest["page"].Parse<int>(1);
                this.PageSize = curRequest["pageSize"].Parse<int>(5);
                this.Skip = curRequest["skip"].Parse<int>(0);
                this.Take = curRequest["take"].Parse<int>(5);

                this.SortOrd = curRequest["sort[0][dir]"];
                this.SortOn = curRequest["sort[0][field]"];

                this.Export = curRequest["export"];
            }
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortOrd { get; set; }
        public string SortOn { get; set; }

        public string Export { get; set; }
    }
}