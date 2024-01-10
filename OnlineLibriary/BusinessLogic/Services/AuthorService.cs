using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AuthorService : ICrud<AuthorDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IValidator<AuthorDTO> _validator;
        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IValidator<AuthorDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _validator = validator;
        }
        public async override Task<IEnumerable<AuthorDTO>> GetAllAsync(PaginationModel paginationModel)
        {
            var cacheKey = $"Author:{paginationModel}";
            var cachedResult = _cacheService.Get<IEnumerable<AuthorDTO>>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<IEnumerable<AuthorDTO>>(await _unitOfWork.AuthorRepository.GetAllAsync());
            if (paginationModel.PageNumber.HasValue && paginationModel.PageNumber > 0 && paginationModel.PageSize.HasValue && paginationModel.PageSize > 0)
            {
                result = ApplyPagination(result, paginationModel.PageNumber.Value, paginationModel.PageSize.Value);
            }
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }
        public async override Task<AuthorDTO> GetByIdAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            var cacheKey = $"Author:{id}";
            var cachedResult = _cacheService.Get<AuthorDTO>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<AuthorDTO>(await _unitOfWork.AuthorRepository.GetByIdAsync(id));
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        private async Task<Author> AddDependenciesAsync(AuthorDTO model)
        {
            var entity = _mapper.Map<Author>(model);
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
        public async override Task<AuthorDTO> AddAsync(AuthorDTO model)
        {
            var validationResult = await _validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            await _unitOfWork.AuthorRepository.AddAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return model;
        }
        public async override Task<AuthorDTO> UpdateAsync(AuthorDTO model)
        {
            var validationResult = await _validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            var result = await _unitOfWork.AuthorRepository.UpdateAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return _mapper.Map<AuthorDTO>(result);
        }
        public async override Task<AuthorDTO> DeleteAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            var result = await _unitOfWork.AuthorRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<AuthorDTO>(result);
        }
    }
}
