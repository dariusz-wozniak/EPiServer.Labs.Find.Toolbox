# EPiServer.Labs.Find.ImprovedSynonyms

ImprovedSynonyms offers a way around current limitations of the Find synonym implementation.
Currently the synonym expansion is done in backend (Elastic Search) and relies on a synonym index.

ImprovedSynonyms solves limitations in the following scenarios:
* Missing or unexplainable hits when using .WithAndAsDefaultOperator()
* Multiple term synonyms
* Multiple term synonyms within quotes
* Multiple term synonyms requires all terms to match
* Does not rely on an synonym index to be up to date

This is solved by downloading and caching the synonym list, and when the query comes in
we expand the matching synonyms on the query, on the client side.

Searching for 'episerver find' where find is a synonym for 'search & navigation"
will result in 'episerver (find OR (search & navigation))'

ImprovedSynonyms also comes with support for Elastic Search's MinimumShouldMatch. 
With MinimumShouldMatch it's possible to set or or more conditions for how many terms (in percentage and absolutes) should match.
If you specify 2<60% all terms up to 2 terms will be required to match. More than 2 terms 60% of the terms are required to match.
Queries with only synonym expansions are always given a minimumShouldMatch of 1>40%.
[![MinimumShouldMatch documentation](https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-minimum-should-match.html)]

[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

---

## Table of Contents

- [System requirements](#system-requirements)
- [Installation](#installation)

---

## System requirements

* Find 12 or higher

See also the general [Episerver system requirements](https://world.episerver.com/documentation/system-requirements/) on Episerver World.

---

## Installation

1. Copy all files into your project

2. Remove any use of .UsingSynonyms()

2. Add .WithAndAsDefaultOperator if you want but we recommend .MinimumShouldMatch(). Not specifying either will allow for OR as the default operator.
   Using MinimumShouldMatch() will preced any use of .WithAndAsDefaultOperator() or the default OR.

3. Add .UsingSynonymsImproved()

4. It could look like this

    UnifiedSearchResults results = SearchClient.Instance.UnifiedSearch(Language.English)
                                    .For(query)             
                                    .MinimumShouldMatch("2<60%")
                                    .UsingSynonymsImproved()                                         
                                    .GetResult();

    or like this

    UnifiedSearchResults results = SearchClient.Instance.UnifiedSearch(Language.English)
                                    .For(query)             
                                    .MinimumShouldMatch("2")
                                    .UsingSynonymsImproved()                                         
                                    .GetResult();
4. Evaluate!

