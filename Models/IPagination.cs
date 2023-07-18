using System;
using System.Collections.Generic;
using mvc.Data;

namespace mvc.Models
{
    public class Pagination
    {
        public Pagination()
        {
            Page = 1;
            TotalPage = 1;
            ItemCount = 1;
            Limit = 10;
        }
        public string Search {get;set;}
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int ItemCount { get; set; }
        public int Limit { get; set; }
        public string Path {get; set;}

        public bool HasNextPage() { return Page < TotalPage; }
        public bool HasPreviousPage() { return Page > 1; }
        public bool IsFirstPage() { return Page == 1; }
        public bool IsLastPage() { return Page == TotalPage; }
        public bool IsPageInMiddle() { return (TotalPage > Page) && (Page > 1); }
        public int GetCenterPage() { return (int)Math.Floor((Double)Page / 2); }
    }

    public interface IPagination
    {
        abstract Pagination Pagination { get; set; }
    }
}
