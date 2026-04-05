using AutoMapper;
using ClassWork_WEBAPI.BLL.Dtos.Pagination;
using ClassWork_WEBAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public abstract class PaginationService
    {
        private readonly IMapper _mapper;
        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PaginationInfoDto<TDto>> GetPaginationAsync<TEntity, TDto>(IQueryable<TEntity> entities, PaginationDto pagination)
            where TEntity : class, IBaseEntiy
        {
            var totalCount = entities.Count();
            int pageCount = (int)Math.Ceiling((double)totalCount / pagination.PageSize);
            int pageSize = pagination.PageSize < 0 ? 10 : pagination.PageSize;
            int page = pagination.Page < 0 || pagination.Page > pageCount ? 1 : pagination.Page;

            var trueEntities = await entities.OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync<TEntity>();

            return new PaginationInfoDto<TDto>
            {
                Page = page,
                PageSize = pageSize,
                PageCount = pageCount,
                TotalCount = totalCount,
                Data = _mapper.Map<List<TDto>>(trueEntities),
            };
        }
    }
}
