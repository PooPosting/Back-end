﻿namespace PicturesAPI.Models;

public class PagedResult<T>
{
    public PagedResult(List<T> items, int pageSize, int pageNumber)
    {
        Items = items;
        Page = pageNumber;
        PageSize = pageSize;
    }
    
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

}