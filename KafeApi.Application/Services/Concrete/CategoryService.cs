using AutoMapper;
using KafeApi.Application.Constants;
using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.Category;
using KafeApi.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _genericRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly AddCategoryValidator _validation;
        private readonly UpdateCategoryValidator _validationn;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly ILogService<Category> _logger;
        private readonly IDistributedCache _distributedCache;

        public CategoryService(IGenericRepository<Category> genericRepository, IMapper mapper, AddCategoryValidator validation, UpdateCategoryValidator validationn, ICategoryRepository categoryRepository, ICacheService cacheService, IConfiguration configuration, ILogService<Category> logService, IDistributedCache distributedCache)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _validation = validation;
            _validationn = validationn;
            _categoryRepository = categoryRepository;
            _cacheService = cacheService;
            _configuration = configuration;
            _logger = logService;
            _distributedCache = distributedCache;
        }

        public async Task<ResponseDto<object>> AddCategory(CreateCategoryDto createCategoryDto)
        {

            var validation = await _validation.ValidateAsync(createCategoryDto);
            if (!validation.IsValid)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Success = false,
                    Message = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage))
                };
            }
            var result = _mapper.Map<Category>(createCategoryDto);

            await _genericRepository.CreateAsync(result);
            _cacheService.Remove(CacheKeys.AllCategories);
            return new ResponseDto<object>
            {
                Data = result,
                Success = true,
                Message = "Kategori eklendi."
            };
        }


        public async Task<ResponseDto<object>> DeleteCategory(int id)
        {

            var result = await _genericRepository.GetByIdAsync(id);
            if (result == null)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Data = null,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Kategori bulunamadi."
                };
            }

            await _genericRepository.DeleteAsync(result);
            _cacheService.Remove(CacheKeys.AllCategories);
            return new ResponseDto<object>
            {
                Data = null,
                Success = true,
                Message = "Kategori silindi."
            };

        }

        public async Task<ResponseDto<List<ResultCategoryDto>>> GetAllCategories()
        {
            List<Category> categories;
            var cache = await _distributedCache.GetStringAsync(CacheKeys.AllCategories);
            if (cache == null)
            {
                _logger.LogInfo("Redis boş, veritabanına gidiliyor...");
                categories = await _genericRepository.GetAllAsync();
            }
            else
            {
                categories = JsonSerializer.Deserialize<List<Category>>(cache);
                _logger.LogInfo("Veriler Redis'ten geldi.");
            }
            if (categories != null && categories.Any())
            {
                string jsonveri = JsonSerializer.Serialize(categories);
                var options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                };
                await _distributedCache.SetStringAsync(CacheKeys.AllCategories, jsonveri, options);
            }

            //if (!_cacheService.TryGetValue(CacheKeys.AllCategories, out List<Category> categories))
            //{
            //    _logger.LogInfo("Kategoriler cache'de bulunamadı, veritabanından getiriliyor...");
            //    categories = await _genericRepository.GetAllAsync();
            //    if (categories != null && categories.Any())
            //    {
            //        var expirationMinutes = _configuration.GetValue<int>("CacheSettings:DefaultExpirationMinutes");
            //        _cacheService.Set(CacheKeys.AllCategories, categories, options: new()
            //        {
            //            AbsoluteExpiration = DateTime.Now.AddMinutes(expirationMinutes),
            //            SlidingExpiration = TimeSpan.FromMinutes(expirationMinutes),

            //        });
            //    }
            //}

            if (categories == null || !categories.Any())
            {
                return new ResponseDto<List<ResultCategoryDto>>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Kategoriler bulunamadi."
                };
            }

            var result = _mapper.Map<List<ResultCategoryDto>>(categories);

            return new ResponseDto<List<ResultCategoryDto>>
            {
                Success = true,
                Data = result
            };
        }

        public async Task<ResponseDto<DetailCategoryDto>> GetCategoryWithMenuItems(int id)
        {

            var categoryWithMenuItems = await _categoryRepository.GetCategoryWithMenuItemsAsync(id);
            if (categoryWithMenuItems == null)
            {
                return new ResponseDto<DetailCategoryDto>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Kategoriler bulunamadi."
                };
            }
            var result = _mapper.Map<DetailCategoryDto>(categoryWithMenuItems);
            return new ResponseDto<DetailCategoryDto>
            {
                Success = true,
                Data = result
            };
        }

        public async Task<ResponseDto<DetailCategoryDto>> GetCategoryById(int id)
        {
            Category category = null;


            if (_cacheService.TryGetValue(CacheKeys.AllCategories, out List<Category> cachedCategories))
            {
                category = cachedCategories.FirstOrDefault(x => x.Id == id);
            }

            if (category == null)
            {
                _logger.LogInfo($"GetCategoryById Cachede kayitli degil veritabanindan getiriliyor id : {id}");
                category = await _genericRepository.GetByIdAsync(id);
            }

            if (category == null)
            {
                return new ResponseDto<DetailCategoryDto>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Kategori bulunamadi."
                };
            }
            var result = _mapper.Map<DetailCategoryDto>(category);
            return new ResponseDto<DetailCategoryDto>
            {
                Success = true,
                Data = result,

            };

        }
        public async Task<ResponseDto<object>> UpdateCategory(UpdateCategoryDto update)
        {

            var validation = await _validationn.ValidateAsync(update);
            if (!validation.IsValid)
            {

                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Message = string.Join(Environment.NewLine, validation.Errors.Select(X => X.ErrorMessage))
                };

            }

            var itemdb = await _genericRepository.GetByIdAsync(update.Id);
            if (itemdb == null)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Kategori bulunamadi."

                };
            }
            var result = _mapper.Map(update, itemdb);
            await _genericRepository.UpdateAsync(result);
            _cacheService.Remove(CacheKeys.AllCategories);
            return new ResponseDto<object>
            {
                Data = null,
                Success = true,
                Message = "Kategori Guncellendi."
            };
        }
        public async Task<ResponseDto<List<ResultCategoryWithMenuItems>>> GetAllCategoreisWithMenuItems()
        {
            var allCategoriesWithMenuItems = await _categoryRepository.GetAllCategoriesWithMenuItemsAsync();
            if (allCategoriesWithMenuItems.Count == 0)
            {
                return new ResponseDto<List<ResultCategoryWithMenuItems>>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Kategoriler bulunamadi."
                };
            }
            var result = _mapper.Map<List<ResultCategoryWithMenuItems>>(allCategoriesWithMenuItems);

            return new ResponseDto<List<ResultCategoryWithMenuItems>>
            {
                Success = true,
                Data = result
            };

        }
    }
}
