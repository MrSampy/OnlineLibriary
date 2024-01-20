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
    public class UserService : ICrud<UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IValidator<UserDTO> _validator;
        private readonly ISecurePasswordHasher _passwordHasher;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IValidator<UserDTO> validator, ISecurePasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _validator = validator;
            _passwordHasher = passwordHasher;
        }

        public override async Task<IEnumerable<UserDTO>> GetAllAsync(PaginationModel paginationModel)
        {
            var cacheKey = $"User:{paginationModel}";
            var cachedResult = _cacheService.Get<IEnumerable<UserDTO>>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.UserRepository.GetAllAsync());
            if (paginationModel.PageNumber.HasValue && paginationModel.PageNumber > 0 && paginationModel.PageSize.HasValue && paginationModel.PageSize > 0)
            {
                result = ApplyPagination(result, paginationModel.PageNumber.Value, paginationModel.PageSize.Value);
            }
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        public override async Task<UserDTO> GetByIdAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            var cacheKey = $"User:{id}";
            var cachedResult = _cacheService.Get<UserDTO>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = _mapper.Map<UserDTO>(await _unitOfWork.UserRepository.GetByIdAsync(id));
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }
        private async Task<User> AddDependenciesAsync(UserDTO model)
        {
            var entity = _mapper.Map<User>(model);
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

        public override async Task<UserDTO> AddAsync(UserDTO model)
        {
            var validationResult = await _validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            model.Password = _passwordHasher.Hash(model.Password);
            await _unitOfWork.UserRepository.AddAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return model;
        }

        public override async Task<UserDTO> UpdateAsync(UserDTO model)
        {
            var validationResult = await _validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            model.Password = _passwordHasher.Hash(model.Password);
            var result = await _unitOfWork.UserRepository.UpdateAsync(await AddDependenciesAsync(model));
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserDTO>(result);
        }

        public override async Task<UserDTO> DeleteAsync(int id)
        {
            var validationResult = await _validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new Exception("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            _cacheService.Reset();
            var result = await _unitOfWork.UserRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserDTO>(result);
        }
    }
}
