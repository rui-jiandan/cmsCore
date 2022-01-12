﻿using LinCms.Aop.Filter;
using LinCms.Cms.Permissions;
using LinCms.Data;
using LinCms.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinCms.Controllers.Cms
{
    [ApiExplorerSettings(GroupName = "cms")]
    [Route("cms/admin/permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 查询所有可分配的权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LinCmsAuthorize("查询所有可分配的权限", "管理员")]
        public IDictionary<string, List<PermissionDto>> GetAllPermissions()
        {
            return _permissionService.GetAllStructualPermissions();
        }

        /// <summary>
        /// 删除某个组别的权限
        /// </summary>
        /// <param name="permissionDto"></param>
        /// <returns></returns>
        [HttpPost("remove")]
        [LinCmsAuthorize("删除多个权限", "管理员")]
        public async Task<UnifyResponseDto> RemovePermissions(RemovePermissionDto permissionDto)
        {
            await _permissionService.DeletePermissionsAsync(permissionDto);
            return UnifyResponseDto.Success("删除权限成功");
        }

        /// <summary>
        /// 分配多个权限
        /// </summary>
        /// <param name="permissionDto"></param>
        /// <returns></returns>
        [HttpPost("dispatch/batch")]
        [LinCmsAuthorize("分配多个权限", "管理员")]
        public async Task<UnifyResponseDto> DispatchPermissions(DispatchPermissionsDto permissionDto)
        {
            List<PermissionDefinition> permissionDefinitions = ReflexHelper.GetAssemblyLinCmsAttributes();
            await _permissionService.DispatchPermissions(permissionDto, permissionDefinitions);
            return UnifyResponseDto.Success("添加权限成功");
        }

        [HttpGet("tree-list")]
        public Task<List<TreePermissionDto>> GetTreePermissionListAsync()
        {
            return _permissionService.GetTreePermissionListAsync();
        }
    }
}
