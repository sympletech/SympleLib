using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SympleLib.Helpers.MVC
{
    public class PageableSearchResults<T> where T : class
    {
        /// <summary>
        /// Total Number of Records In Dataset
        /// </summary>
        public int Records { get; private set; }

        /// <summary>
        /// Number of Records to display Per Page (Defaults to 15)
        /// </summary>
        public int? RecordsPerPage { get; set; }

        /// <summary>
        /// Current Page Number To Display
        /// </summary>
        public int? CurPage { get; set; }

        /// <summary>
        /// Total Number of Pages in Pager
        /// </summary>
        public int? TotalPages { get; private set; }

        /// <summary>
        /// The Collection of Items To Be Paged
        /// </summary>
        public IEnumerable<T> Items
        {
            get
            {
                try
                {
                    return this.VisibleItems ?? (this.VisibleItems = new List<T>());
                }
                catch
                {
                    return new List<T>();
                }
            }
            set
            {
                this.CompleteDataSet = value.AsQueryable();
                SetVisibleItems();
            }
        }
        private IQueryable<T> CompleteDataSet { get; set; }
        private IEnumerable<T> VisibleItems { get; set; }

        private void SetVisibleItems()
        {
            if (this.CurPage == null)
            {
                this.CurPage = 1;
            }
            if (this.RecordsPerPage == null)
            {
                this.RecordsPerPage = 15;
            }

            this.Records = this.CompleteDataSet.Count();
            this.TotalPages = Convert.ToInt32(Math.Ceiling((double)this.Records / this.RecordsPerPage.Value));

            if (CurPage > TotalPages)
            {
                CurPage = TotalPages;
            }

            this.VisibleItems = this.CompleteDataSet.Skip((this.CurPage.Value - 1) * this.RecordsPerPage.Value).Take(this.RecordsPerPage.Value);
        }

        /// <summary>
        /// Set the visible items before Passing To Class
        /// </summary>
        /// <param name="prePagedItems"></param>
        /// <param name="recordCount"></param>
        public void SetPrePagedItems(IEnumerable<T> prePagedItems, int recordCount)
        {
            if (this.CurPage == null)
            {
                this.CurPage = 1;
            }
            if (this.RecordsPerPage == null)
            {
                this.RecordsPerPage = 15;
            }

            this.Records = recordCount;

            this.TotalPages = Convert.ToInt32(Math.Ceiling((double)this.Records / this.RecordsPerPage.Value));

            this.VisibleItems = prePagedItems;
        }

        /// <summary>
        /// Generates The Pager for the grid
        /// </summary>
        /// <param name="jsPagerFunction">The name of the JS Function to call to page -- passes the new page number to it</param>
        public HtmlString GeneratePager(string jsPagerFunction)
        {
            return this.GeneratePager(jsPagerFunction, "");
        }

        /// <summary>
        /// Generates The Pager for the grid
        /// </summary>
        /// <param name="jsPagerFunction">The name of the JS Function to call to page -- passes the new page number to it</param>
        /// <param name="additionalParamsCsv">Additional Parameters to send to javascript function</param>
        public HtmlString GeneratePager(string jsPagerFunction, string additionalParamsCsv)
        {
            if (additionalParamsCsv != "")
            {
                additionalParamsCsv = "," + additionalParamsCsv;
            }

            var sbPager = new StringBuilder();
            sbPager.Append("<div class='pager'>");
            sbPager.Append("<input type='hidden' id='cur_page' value='" + CurPage + "' />");

            if (this.CurPage > 1)
            {
                sbPager.AppendFormat("<div class='previous' onclick='{0}({1}{2})'></div>", jsPagerFunction, this.CurPage - 1, additionalParamsCsv);
            }

            sbPager.AppendFormat("Page {0} of {1}", this.CurPage, this.TotalPages);

            if (this.CurPage < this.TotalPages)
            {
                sbPager.AppendFormat("<div class='next' onclick='{0}({1}{2})'></div>", jsPagerFunction, this.CurPage + 1, additionalParamsCsv);
            }

            sbPager.Append("</div>");
            return new HtmlString(sbPager.ToString());
        }
    }
}