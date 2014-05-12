﻿using System.Collections.Generic;
using System.Linq;
using CavemanTools.Model;

namespace CavemanTools
{
    public static class ModelExtensions
    {
        public static Pagination ToPagination(this IPagedInput input, int pageSize = 15)
        {
            return new Pagination(input.Page, pageSize);
        }

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> list, Pagination pager)
        {
            return list.Skip((int) pager.Skip).Take(pager.PageSize);            
        }

    }
}