using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using BusinessLogic.Models.DTOs;
using Data.Entities;
using BusinessLogic.Models;
using BusinessLogic.Validation;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Services
{
    public class GenreService : ICrud<GenreDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IValidator<GenreDTO> _validator;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IValidator<GenreDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _validator = validator;
        }
        public async override Task<IEnumerable<GenreDTO>> GetAllAsync(PaginationModel paginationModel)
        {
            var cacheKey = $"Genre:{paginationModel}";
            var cachedResult = _cacheService.Get<IEnumerable<GenreDTO>>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            
            var result = _mapper.Map<IEnumerable<GenreDTO>>(await _unitOfWork.GenreRepository.GetAllAsync()); 
            if (paginationModel.PageNumber.HasValue && paginationModel.PageNumber > 0 && paginationModel.PageSize.HasValue && paginationModel.PageSize > 0)
            {
                result = ApplyPagination(result, paginationModel.PageNumber.Value, paginationModel.PageSize.Value);
            }
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        private async Task<Genre> AddDependenciesAsync(GenreDTO model)
        {
            var entity = _mapper.Map<Genre>(model);
            entity.Books = new List<Book>();
            if (model.BookIds != null) 
            {
                foreach (var bookId in model.BookIds)
                {
                    var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                    entity.Books.Add(book);
                }
            }
            
            return entity;
        }

        public async override Task<GenreDTO> GetByIdAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if(!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            var cacheKey = $"Genre:{id}";
            var cachedResult = _cacheService.Get<GenreDTO>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<GenreDTO>(await _unitOfWork.GenreRepository.GetByIdAsync(id));
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        public async override Task<GenreDTO> AddAsync(GenreDTO model)
        {            
            var validationResult = await _validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();          
            await _unitOfWork.GenreRepository.AddAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return model;
        }

        public async override Task<GenreDTO> UpdateAsync(GenreDTO model)
        {            
            var validationResult = await _validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();            
            var result = await _unitOfWork.GenreRepository.UpdateAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GenreDTO>(result);
        }
        public async override Task<GenreDTO> DeleteAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            var result = await _unitOfWork.GenreRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GenreDTO>(result);
        }
    }
}
