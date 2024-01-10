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

namespace BusinessLogic.Validation
{
    public class AuthorValidator : IValidator<AuthorDTO>
    {
        protected readonly IUnitOfWork _unitOfWork;
        public AuthorValidator(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ValidationResult> ValidateForAddAsync(AuthorDTO model, bool identityCheck = true)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                result.IsValid = false;
                result.Messages.Add("Author full name cannot be empty");
            }
            else if(model.FullName.Split().Length != 2)
            {
                result.IsValid = false;
                result.Messages.Add("Author full name must contain first and last name");
            }
            else if(model.FullName.Length > 100)
            {
                result.IsValid = false;
                result.Messages.Add("Author full name cannot be longer than 100 characters");
            }
            else if(model.FullName.Length < 5)
            {
                result.IsValid = false;
                result.Messages.Add("Author full name cannot be shorter than 5 characters");
            }

            if(model.DateOfBirth == default)
            {
                result.IsValid = false;
                result.Messages.Add("Author date of birth cannot be empty");
            }
            else if(model.DateOfBirth > DateTime.Now)
            {
                result.IsValid = false;
                result.Messages.Add("Author date of birth cannot be in the future");
            }

            if (string.IsNullOrWhiteSpace(model.Country))
            {
                result.IsValid = false;
                result.Messages.Add("Author country cannot be empty");
            }

            if(model.BookIds != null && model.BookIds.Count != 0)
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

        public async Task<ValidationResult> ValidateForUpdateAsync(AuthorDTO model)
        {
           var result = new ValidationResult()
           {
                IsValid = true,
                Messages = new List<string>()
            };

            if (model.Id == default)
            {
                result.IsValid = false;
                result.Messages.Add("Author id cannot be empty");
            }
            else
            {
                var validationResult = await ValidateIdAsync(model.Id);
                if (!validationResult.IsValid)
                {
                    result.IsValid = false;
                    result.Messages.AddRange(validationResult.Messages);
                }
            }
            var validationResultForAdd = await ValidateForAddAsync(model);
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
            var author = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            if(author == null)
            {
                result.IsValid = false;
                result.Messages.Add($"Author with id {id} does not exist");
            }
            return result;
        }
    }
}
