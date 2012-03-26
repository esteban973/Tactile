using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace CRM_HELIANTIS.Utils
{

    public interface IPagedList
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }

    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPageIndex = pageIndex;
            StartRecordIndex = (CurrentPageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Add(items[i]);
            }
        }

        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
        }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; private set; }
        public int StartRecordIndex { get; private set; }
        public int EndRecordIndex { get; private set; }

        public JqGridResult toJqGridResult()
        {

            return new JqGridResult
            {
                page = this.CurrentPageIndex,
                totalpage = this.TotalPageCount,
                totalrecords = this.TotalItemCount,
                dataitems = this.ToList()
            };
            
        }

        
    }




    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            
            var totalItemCount = allItems.Count();
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
    }

    public class JqGridResult
    {
        public int page { get; set; }
        public int totalpage { get; set; }
        public int totalrecords { get; set; }
        public object dataitems { get; set; }
        
    }
}