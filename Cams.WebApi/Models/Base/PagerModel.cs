using System;
using System.Collections.Generic;
using System.Text;

namespace XichLip.WebApi.Models.Base
{
    public class PagerModel
    {
        public PagerModel()
        {

        }
        public PagerModel(int pageIndex, int pageSize, long? totalRecord)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecord ?? 0;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((double)TotalRecords / PageSize);
            }
        }
    }
}
