using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

using PagedList;

namespace CarService.Web.Infrastructure.ExtensionMethods
{
    public static class Helper
    {
        public static IPagedList<TDestination> ToMappedPagedList<TSource, TDestination>(this IPagedList<TSource> list)
        {
            IEnumerable<TDestination> sourceList = Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);
            IPagedList<TDestination> pagedResult = new StaticPagedList<TDestination>(sourceList, list.GetMetaData());
            return pagedResult;
        }

        public static string CyrillicToLatinToUpper(this string input)
        {
            Dictionary<char, char> replace = new Dictionary<char, char>()
            {
                {'А', 'A'},
                {'В', 'B'},
                {'С', 'C'},
                {'Е', 'E'},
                {'Н', 'H'},
                {'К', 'K'},
                {'М', 'M'},
                {'О', 'O'},
                {'Р', 'P'},
                {'Т', 'T'},
                {'Х', 'X'}
            };
            var inputArr = input.ToUpperInvariant().ToCharArray();
            for (int i = 0; i < inputArr.Length; i++)
            {
                char current = inputArr[i];
                char other;
                if (replace.TryGetValue(current, out other))
                {
                    inputArr[i] = other;
                }
            }
            return new string(inputArr);
        }
    }
}