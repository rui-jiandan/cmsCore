﻿using Autofac;
using LinCms.Cms.Files;
using LinCms.Data.Options;
using LinCms.Exceptions;
using LinCms.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LinCms.Controllers.Cms
{
    [ApiExplorerSettings(GroupName = "cms")]
    [Route("cms/file")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly FileStorageOption _fileStorageOption;
        private readonly IFileService _fileService;
        public FileController(IOptionsSnapshot<FileStorageOption> optionsSnapshot, IComponentContext componentContext)
        {
            _fileStorageOption = optionsSnapshot.Value;
            _fileService = componentContext.ResolveNamed<IFileService>(_fileStorageOption.ServiceName);
        }

        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<FileDto>> UploadFiles()
        {
            IFormFileCollection files = Request.Form.Files;

            if (files.Count > _fileStorageOption.NumLimit)
            {
                throw new LinCmsException($"最大文件数量{_fileStorageOption.NumLimit}");
            }
            long len = 0;
            foreach (var file in files)
            {
                len += file.Length;
                this.ValidFile(file);
            }

            if (len > _fileStorageOption.MaxFileSize)
            {
                throw new LinCmsException($"文件总大小{len}，超过上传文件总大小{_fileStorageOption.MaxFileSize}");
            }

            int i = 0;
            List<FileDto> fileDtos = new List<FileDto>();
            foreach (var file in files)
            {
                FileDto fileDto = await _fileService.UploadAsync(file, i++);
                fileDtos.Add(fileDto);
            }
            return fileDtos;
        }

        /// <summary>
        /// 单文件上传，键为file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public Task<FileDto> UploadAsync(IFormFile file, int key = 0)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                throw new LinCmsException($"The request couldn't be processed (Error 1).");
            }

            this.ValidFile(file);
            return _fileService.UploadAsync(file, key);
        }

        private void ValidFile(IFormFile file)
        {
            string? ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext))
            {
                throw new LinCmsException($"不支持的文件类型");
            }

            if (_fileStorageOption.Include.IsNotNullOrEmpty())
            {
                if (!_fileStorageOption.Include.Contains(ext))
                {
                    throw new LinCmsException($"不支持文件类型{ext}");
                }
                return;
            }

            if (_fileStorageOption.Exclude.IsNotNullOrEmpty() && _fileStorageOption.Exclude.Contains(ext))
            {
                throw new LinCmsException($"文件类型{ext}被禁止上传");
            }
        }

    }
}
