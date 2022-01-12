﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Test.Controller.v1
{
    public class CommentControllerTest : BaseControllerTests
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IMapper _mapper;
        private readonly IFreeSql _freeSql;

        public CommentControllerTest() : base()
        {
            _hostingEnv = ServiceProvider.GetService<IWebHostEnvironment>();

            _mapper = ServiceProvider.GetService<IMapper>();
            _freeSql = ServiceProvider.GetService<IFreeSql>();
        }


    }
}
