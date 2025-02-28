﻿using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Queries;
using System.Linq;
using System.Text.RegularExpressions;

namespace EPiServer.Labs.Find.Toolbox
{
    public static class QueryHelpers
    {
        public static readonly Regex SpaceTabsRegex = new Regex(@"/\\s+/", RegexOptions.Compiled);
        public static readonly Regex TermsAndQuotedTermsRegex = new Regex(@"([\d\.\,]+)|([\w\-&´'`\.\/:]+)|([""][\s\w\-&´'`\.\/\:]+[""])", RegexOptions.Compiled);

        public static string[] GetQueryPhrases(string query)
        {
            string[] AndOrNotOperators = new string[] { "AND", "OR", "NOT" };

            // Replace double space, tabs with single whitespace and trim space on side
            string cleanedQuery = SpaceTabsRegex.Replace(query, " ").Trim();

            // Match single terms and quoted terms, allow -&´'` in terms, allow ., in numerics, allow space between quotes and word.
            return TermsAndQuotedTermsRegex.Matches(cleanedQuery).Cast<Match>().Select(c => c.Value.Trim()).Where(x => !AndOrNotOperators.Contains(x)).Take(50).ToArray();
        }

        public static bool GetFirstQueryStringQuery(ISearchContext context, out IQuery query, out BoolQuery boolQuery)
        {

            // Check for existing bool query with possible queries within and get the first which would be the one generated by For()
            if (TryGetBoolQuery(context.RequestBody.Query, out boolQuery))
            {
                query = boolQuery.Should.FirstOrDefault();
            }
            else
            {
                query = context.RequestBody.Query;
            }

            return query is QueryStringQuery;
        }

        public static bool TryGetBoolQuery(IQuery query, out BoolQuery currentBoolQuery)
        {
            currentBoolQuery = query as BoolQuery;
            if (currentBoolQuery == null)
            {
                currentBoolQuery = new BoolQuery();
                return false;
            }

            return true;

        }

        public static string GetRawQueryString(IQuery query)
        {
            return ((query as QueryStringQuery).RawQuery ?? string.Empty).ToString();
        }

        public static bool IsStringQuoted(string text)
        {
            return text.StartsWith("\"") && text.EndsWith("\"");
        }

        public static bool IsStringParenthesized(string text)
        {
            return text.StartsWith("(") && text.EndsWith(")");
        }

    }

}