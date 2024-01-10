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
    public class BookService : ICrud<BookDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IValidator<BookDTO> _validator;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IValidator<BookDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _validator = validator;
        }

        public override async Task<IEnumerable<BookDTO>> GetAllAsync(PaginationModel paginationModel)
        {
            var cacheKey = $"Book:{paginationModel}";
            var cachedResult = _cacheService.Get<IEnumerable<BookDTO>>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<IEnumerable<BookDTO>>(await _unitOfWork.BookRepository.GetAllAsync());
            if (paginationModel.PageNumber.HasValue && paginationModel.PageNumber > 0 && paginationModel.PageSize.HasValue && paginationModel.PageSize > 0)
            {
                result = ApplyPagination(result, paginationModel.PageNumber.Value, paginationModel.PageSize.Value);
            }
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        public override async Task<BookDTO> GetByIdAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            var cacheKey = $"Book:{id}";
            var cachedResult = _cacheService.Get<BookDTO>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<BookDTO>(await _unitOfWork.BookRepository.GetByIdAsync(id));
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        private async Task<Book> AddDependenciesAsync(BookDTO model)
        {
            var entity = _mapper.Map<Book>(model);
            
            entity.Users = new List<User>();
            if (model.UserIds != null)
            {
                foreach (var userId in model.UserIds)
                {
                    var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                    entity.Users.Add(user);
                }
            }

            if(model.AuthorId != default)
            {
                entity.Author = await _unitOfWork.AuthorRepository.GetByIdAsync(model.AuthorId);
            }

            if(model.GenreId != default)
            {
                entity.Genre = await _unitOfWork.GenreRepository.GetByIdAsync(model.GenreId);
            }

            return entity;
        }


        public override async Task<BookDTO> AddAsync(BookDTO model)
        {
            var validationResult = await _validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            
            await _unitOfWork.BookRepository.AddAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return model;
        }

        public override async Task<BookDTO> UpdateAsync(BookDTO model)
        {
            var validationResult = await _validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();           
            var result = await _unitOfWork.BookRepository.UpdateAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return _mapper.Map<BookDTO>(result);
        }

        public override async Task<BookDTO> DeleteAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            var result = await _unitOfWork.BookRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<BookDTO>(result);
        }
    }
}
