using Interface.Common;
using Interface.Service;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Caching;

namespace Interface.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CheckController : Controller
    {
        private ObjectCache cache;
        private string dfaCacheKey = "DFAObject";
        DFA dfa = new DFA();

        public CheckController()
        {
            cache = MemoryCache.Default;
        }
        protected ApiResult Response(object data = null, string message = null, string errorCode = null)
        {
            return new ApiResult
            {
                Data = data,
                Message = message,
                ErrorCode = errorCode
            };
        }
        [HttpPost]
        public async Task<ApiResult> MatchInputString(string inputString)
        {
            if (cache.Contains(dfaCacheKey))
            {
                dfa = (DFA)cache.Get(dfaCacheKey);
            }
            else
            {
                string lexiconFilePath = @"C:\\Users\\Ice\\Desktop\\屏蔽词.txt";
                await dfa.InitializeAsync(lexiconFilePath);
                cache.Add(dfaCacheKey, dfa, DateTimeOffset.MaxValue);
            }

            bool isMatched = dfa.Match(inputString);

            return Response(isMatched);
        }

    }



}
