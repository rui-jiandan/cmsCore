﻿using System.Threading.Tasks;
using LinCms.Data;

namespace LinCms.Base.Localizations
{
    public interface IResourceService
    {
        Task<PagedResultDto<ResourceDto>> GetListAsync(ResourceSearchDto searchDto);

        Task DeleteAsync(long id);

        Task<ResourceDto> GetAsync(long id);

        Task CreateAsync(ResourceDto createChannel);

        Task UpdateAsync(ResourceDto updateChannel);
    }
}
