﻿using System;
using LinCms.Entities.Blog;

namespace LinCms.Blog.UserLikes
{
    public class CreateUpdateUserLikeDto
    {
        public Guid SubjectId { get; set; }
        /// <summary>
        /// 1.文章 2 评论
        /// </summary>
        public UserLikeSubjectType SubjectType { get; set; }
    }
}
