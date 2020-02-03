using System.Collections;
using System.Collections.Generic;
using StopGambleProject.Models.Post;

namespace StopGambleProject.Models.Search
{
    public class SearchResultModel
    {
        
        public IEnumerable<PostListingModel> Posts { get; set; }
        public string SearchQuery { get; set; }
        public bool EmptySearchResults { get; set; }
    }
}