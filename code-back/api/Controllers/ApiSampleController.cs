using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using dockerapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;


namespace dockerapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiSampleController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiSampleController> _logger;
        private readonly CacheService _cache;

        public ApiSampleController(IConfiguration configuration, ILogger<ApiSampleController> logger, CacheService cache)
        {
            _configuration = configuration;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<string> GetAsync()
        {
            _logger.LogWarning("ApiSampleController Get");


            return await _cache.TryGet<string>(
                "MyTimeKey",
                new TimeSpan(0, 0, 10),
                () =>
                {

                    using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        return db.QueryFirstOrDefault<string>("SELECT getdate()");
                    };
                }
                );


        }
    }
}
