﻿using EPiServer.Find.Api.Querying.Queries;
using EPiServer.ServiceLocation;
using System;

namespace EPiServer.Find.Cms
{
    public static class MultiFieldQueryStringQueryExtensions
    {
        private static Lazy<UsingSynonymService> _lazyUsingSynonymService = new Lazy<UsingSynonymService>(() => ServiceLocator.Current.GetInstance<UsingSynonymService>());

        public static IQueriedSearch<TSource, QueryStringQuery> UsingSynonymsImproved<TSource>(this IQueriedSearch<TSource> search)
        {
            return UsingSynonymsImproved(search, _lazyUsingSynonymService.Value);
        }

        public static IQueriedSearch<TSource, QueryStringQuery> UsingSynonymsImproved<TSource>(this IQueriedSearch<TSource> search, UsingSynonymService usingSynonymService)
        {
            return usingSynonymService.UsingSynonyms(search);
        }
    }
}