﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using LinCms.Aop.Attributes;
using LinCms.Cms.Groups;
using LinCms.Cms.Users;
using LinCms.Data;
using LinCms.Entities;
using LinCms.IRepositories;
using LinCms.Security;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Controllers.Cms
{
    [ApiExplorerSettings(GroupName = "cms")]
    [ApiController]
    [Route("cms/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IFreeSql _freeSql;
        private readonly IMapper _mapper;
        private readonly IUserService _userSevice;
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IGroupService _groupService;
        private readonly IFileRepository _fileRepository;

        public UserController(IFreeSql freeSql, IMapper mapper, IUserService userSevice, ICurrentUser currentUser, IUserRepository userRepository, IGroupService groupService, IFileRepository fileRepository)
        {
            _freeSql = freeSql;
            _mapper = mapper;
            _userSevice = userSevice;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _groupService = groupService;
            _fileRepository = fileRepository;
        }

        [HttpGet("get")]
        public JsonResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }


        /// <summary>
        /// 新增用户-不是注册，注册不可能让用户选择gourp_id
        /// </summary>
        /// <param name="userInput"></param>
        [Logger("管理员新建了一个用户")]
        [HttpPost("register")]
        [Authorize(Roles = LinGroup.Admin)]
        public async Task<UnifyResponseDto> CreateAsync([FromBody] CreateUserDto userInput)
        {
            await _userSevice.CreateAsync(_mapper.Map<LinUser>(userInput), userInput.GroupIds, userInput.Password);
            return UnifyResponseDto.Success("用户创建成功");
        }

        /// <summary>
        /// 得到当前登录人信息
        /// </summary>
        [HttpGet("information")]
        public Task<UserInformation> GetInformationAsync()
        {
            return _userSevice.GetInformationAsync(_currentUser.Id ?? 0);
        }

        /// <summary>
        /// 查询自己拥有的权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("permissions")]
        public async Task<UserInformation> Permissions()
        {
            UserInformation userInformation = await _userSevice.GetInformationAsync(_currentUser.Id ?? 0);
            var permissions = await _userSevice.GetStructualUserPermissions(_currentUser.Id ?? 0);
            userInformation.Permissions = permissions;
            userInformation.Admin = _groupService.CheckIsRootByUserId(_currentUser.Id ?? 0);
            return userInformation;
        }

        [Logger("修改了自己的密码")]
        [HttpPut("change_password")]
        public async Task<UnifyResponseDto> ChangePasswordAsync([FromBody] ChangePasswordDto passwordDto)
        {
            await _userSevice.ChangePasswordAsync(passwordDto);
            return UnifyResponseDto.Success("密码修改成功");
        }

        [HttpPut("avatar")]
        public async Task<UnifyResponseDto> SetAvatar(UpdateAvatarDto avatarDto)
        {
            await _freeSql.Update<LinUser>(_currentUser.Id).Set(a => new LinUser()
            {
                Avatar = avatarDto.Avatar
            }).ExecuteAffrowsAsync();

            return UnifyResponseDto.Success("更新头像成功");
        }

        [HttpPut("nickname")]
        public UnifyResponseDto SetNickname(UpdateNicknameDto updateNicknameDto)
        {
            _freeSql.Update<LinUser>(_currentUser.Id).Set(a => new LinUser()
            {
                Nickname = updateNicknameDto.Nickname
            }).ExecuteAffrows();
            return UnifyResponseDto.Success("更新昵称成功");
        }

        [HttpPut]
        public UnifyResponseDto SetProfileInfo(UpdateProfileDto profileDto)
        {
            _freeSql.Update<LinUser>(_currentUser.Id).Set(a => new LinUser()
            {
                Nickname = profileDto.Nickname,
                BlogAddress = profileDto.BlogAddress,
                Introduction = profileDto.Introduction
            }).ExecuteAffrows();
            return UnifyResponseDto.Success("更新基本信息成功");
        }

        [AllowAnonymous]
        [HttpGet("avatar/{userId}")]
        public async Task<string> GetAvatarAsync(long userId)
        {
            string avatar = await _userRepository.Where(r => r.Id == userId).FirstAsync(r => r.Avatar);
            return _fileRepository.GetFileUrl(avatar);

        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<OpenUserDto?> GetUserByUserId(long userId)
        {
            LinUser linUser = await _userRepository.Where(r => r.Id == userId).FirstAsync();
            OpenUserDto openUser = _mapper.Map<LinUser, OpenUserDto>(linUser);
            if (openUser == null) return null;
            openUser.Avatar = _fileRepository.GetFileUrl(openUser.Avatar);
            return openUser;
        }

        /// <summary>
        /// 获取最新的12条新手用户数据
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Cacheable]
        [HttpGet("novices")]
        public virtual async Task<List<UserNoviceDto>> GetNovicesAsync()
        {
            List<UserNoviceDto> userNoviceDtos = (await _userRepository.Select
                .OrderByDescending(r => r.CreateTime)
                .Take(12)
                .ToListAsync(r => new UserNoviceDto()
                {
                    Id = r.Id,
                    Introduction = r.Introduction,
                    Nickname = r.Nickname,
                    Avatar = r.Avatar,
                    Username = r.Username,
                    LastLoginTime = r.LastLoginTime,
                    CreateTime = r.CreateTime,
                })).Select(r =>
                {
                    r.Avatar = _fileRepository.GetFileUrl(r.Avatar);
                    return r;
                }).ToList();

            return userNoviceDtos;
        }
    }
}
