using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PaginatedList<T> : IEnumerable<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public List<T> Items { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        Items = items.Skip((pageIndex - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();
    }

    public bool HasPreviousPage
    {
        get { return PageIndex > 1; }
    }

    public bool HasNextPage
    {
        get { return PageIndex < TotalPages; }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
