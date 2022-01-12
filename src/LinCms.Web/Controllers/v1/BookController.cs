﻿using System.Threading.Tasks;
using LinCms.Aop.Filter;
using LinCms.Data;
using LinCms.v1.Books;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("v1/book")]
    [ApiController]
    // [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpDelete("{id}")]
        [LinCmsAuthorize("删除图书", "图书")]
        public async Task<UnifyResponseDto> DeleteAsync(int id)
        {
            await _bookService.DeleteAsync(id);
            return UnifyResponseDto.Success();

        }

        [HttpGet]
        public async Task<PagedResultDto<BookDto>> GetListAsync([FromQuery] PageDto pageDto)
        {
            return await _bookService.GetListAsync(pageDto);
        }

        [HttpGet("{id}")]
        public async Task<BookDto> GetAsync(int id)
        {
            return await _bookService.GetAsync(id);
        }

        [HttpPost]
        public async Task<UnifyResponseDto> CreateAsync([FromBody] CreateUpdateBookDto createBook)
        {
            await _bookService.CreateAsync(createBook);
            return UnifyResponseDto.Success("新建图书成功");
        }

        [HttpPut("{id}")]
        public async Task<UnifyResponseDto> UpdateAsync(int id, [FromBody] CreateUpdateBookDto updateBook)
        {
            await _bookService.UpdateAsync(id, updateBook);
            return UnifyResponseDto.Success("更新图书成功");
        }
    }
}