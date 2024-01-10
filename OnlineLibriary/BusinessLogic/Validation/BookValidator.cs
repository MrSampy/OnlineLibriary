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
    public class BookValidator : IValidator<BookDTO>
    {
        protected IUnitOfWork _unitOfWork;
        public BookValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ValidationResult> ValidateForAddAsync(BookDTO model, bool identityCheck = true)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if(string.IsNullOrWhiteSpace(model.Title))
            {
                result.IsValid = false;
                result.Messages.Add("Book title cannot be empty");
            }
            else if(model.Title.Length > 100)
            {
                result.IsValid = false;
                result.Messages.Add("Book title cannot be longer than 100 characters");
            }
            else if(model.Title.Length < 2)
            {
                result.IsValid = false;
                result.Messages.Add("Book title cannot be shorter than 1 character");
            }

            if(model.AuthorId != default)
            {
                var author = await _unitOfWork.AuthorRepository.GetByIdAsync(model.AuthorId);
                if(author == null)
                {
                    result.IsValid = false;
                    result.Messages.Add($"Author with id {model.AuthorId} does not exist");
                }
            }

            if(model.GenreId == default)
            {
                result.IsValid = false;
                result.Messages.Add("Genre id cannot be empty");
            }
            else
            {
                var genre = await _unitOfWork.GenreRepository.GetByIdAsync(model.GenreId);
                if(genre == null)
                {
                    result.IsValid = false;
                    result.Messages.Add($"Genre with id {model.GenreId} does not exist");
                }
            }

            if(model.Year == default)
            {
                result.IsValid = false;
                result.Messages.Add("Publication year cannot be empty");
            }
            else if(model.Year < 0)
            {
                result.IsValid = false;
                result.Messages.Add("Publication year cannot be negative");
            }
            else if(model.Year > DateTime.Now.Year)
            {
                result.IsValid = false;
                result.Messages.Add("Publication year cannot be greater than current year");
            }

            if(string.IsNullOrWhiteSpace(model.Description))
            {
                result.IsValid = false;
                result.Messages.Add("Book description cannot be empty");
            }
            else if(model.Description.Length > 1000)
            {
                result.IsValid = false;
                result.Messages.Add("Book description cannot be longer than 1000 characters");
            }
            else if(model.Description.Length < 10)
            {
                result.IsValid = false;
                result.Messages.Add("Book description cannot be shorter than 10 characters");
            }

            if (model.BookContent == null)
            {
                result.IsValid = false;
                result.Messages.Add("Book content cannot be empty");
            }
            else if (model.BookContent.Length > 1000000)
            {
                result.IsValid = false;
                result.Messages.Add("Book content cannot be longer than 1MB");
            }

            if(model.UserIds != null && model.UserIds.Any())
            {
                foreach (var userId in model.UserIds)
                {
                    var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                    if(user == null)
                    {
                        result.IsValid = false;
                        result.Messages.Add($"User with id {userId} does not exist");
                    }
                }
            }

            return result;
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(BookDTO model)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if(model.Id == default)
            {
                result.IsValid = false;
                result.Messages.Add("Book id cannot be empty");
            }
            else
            {
                var validationResult = await ValidateIdAsync(model.Id);
                if(!validationResult.IsValid)
                {
                    result.IsValid = false;
                    result.Messages.AddRange(validationResult.Messages);
                }
            }

            var validationResultForAdd = await ValidateForAddAsync(model);
            if(!validationResultForAdd.IsValid)
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
            var author = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (author == null)
            {
                result.IsValid = false;
                result.Messages.Add($"Book with id {id} does not exist");
            }
            return result;
        }
    }
}
