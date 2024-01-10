using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class GenreValidator : IValidator<GenreDTO>
    {
        protected IUnitOfWork _unitOfWork;

        public GenreValidator(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork; 
        }
        public async Task<ValidationResult> ValidateForAddAsync(GenreDTO model, bool identityCheck = true)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                result.IsValid = false;
                result.Messages.Add("Genre name cannot be empty");
            }
            else if(identityCheck)
            {
                var genre = (await _unitOfWork.GenreRepository.GetAllAsync()).FirstOrDefault(x => x.Name.Equals(model.Name));
                if (genre != null)
                {
                    result.IsValid = false;
                    result.Messages.Add($"Genre with name {model.Name} already exists");
                }
            }
            if (model.Name.Length > 50)
            {
                result.IsValid = false;
                result.Messages.Add("Genre name cannot be longer than 50 characters");
            }

            if(model.BookIds != null)
            {
                foreach (var bookId in model.BookIds)
                {
                    var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                    if (book == null)
                    {
                        result.IsValid = false;
                        result.Messages.Add($"Book with id {bookId} does not exist");
                    }
                }
            }

            return result;

        }

        public async Task<ValidationResult> ValidateForUpdateAsync(GenreDTO model)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if (model.Id == default)
            {
                result.IsValid = false;
                result.Messages.Add("Genre id cannot be empty");
            }
            else 
            {
                var validationResult = await ValidateIdAsync(model.Id);
                if (!validationResult.IsValid)
                {
                    result.IsValid = false;
                    result.Messages.AddRange(validationResult.Messages);
                }
                else 
                {
                    var currentGenre = await _unitOfWork.GenreRepository.GetByIdAsync(model.Id);
                    var genre = (await _unitOfWork.GenreRepository.GetAllAsync()).FirstOrDefault(x => !currentGenre.Name.Equals(x.Name) && x.Name.Equals(model.Name));
                    if (genre != null)
                    {
                        result.IsValid = false;
                        result.Messages.Add($"Genre with name {model.Name} already exists");
                    }
                }
            }
            var validationResultForAdd = await ValidateForAddAsync(model, false);
            if (!validationResultForAdd.IsValid)
            {
                result.IsValid = false;
                result.Messages.AddRange(validationResultForAdd.Messages);
            }            

            return result;

        }

        public async Task<ValidationResult> ValidateIdAsync(int id)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id);
            if (genre == null) 
            { 
                result.IsValid = false;
                result.Messages.Add($"Genre with id {id} does not exist");
            }
            return result;
        }
    }
}
