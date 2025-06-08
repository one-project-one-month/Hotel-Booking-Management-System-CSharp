using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Implementation
{
    public class BlogService : HttpContextService
    {
        public BlogService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public async Task<string> GetUserId()
        {
            return await Task.FromResult(UserId);
        }
    }
}
