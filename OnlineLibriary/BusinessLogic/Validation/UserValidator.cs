using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class UserValidator : IValidator<UserDTO>
    {
        IUnitOfWork _unitOfWork;
        public UserValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ValidationResult> ValidateForAddAsync(UserDTO model, bool identityCheck = true)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                result.IsValid = false;
                result.Messages.Add("User username cannot be empty");
            }
            else if (model.Username.Length > 50)
            {
                result.IsValid = false;
                result.Messages.Add("User username cannot be longer than 50 characters");
            }
            else if (model.Username.Length < 5)
            {
                result.IsValid = false;
                result.Messages.Add("User username cannot be shorter than 5 characters");
            }
            else if(identityCheck)
            {
                var user =(await _unitOfWork.UserRepository.GetAllAsync()).FirstOrDefault(x=>x.Username.Equals(model.Username));
                if(user != null)
                {
                    result.IsValid = false;
                    result.Messages.Add($"User with username {model.Username} already exists");
                }
            }


            if (string.IsNullOrWhiteSpace(model.Password))
            {
                result.IsValid = false;
                result.Messages.Add("User password cannot be empty");
            }
            else if (model.Password.Length > 50)
            {
                result.IsValid = false;
                result.Messages.Add("User password cannot be longer than 50 characters");
            }
            else if (model.Password.Length < 5)
            {
                result.IsValid = false;
                result.Messages.Add("User password cannot be shorter than 5 characters");
            }

            if (string.IsNullOrWhiteSpace(model.FullName)) 
            {
                result.IsValid = false;
                result.Messages.Add("User full name cannot be empty");
            }
            else if(model.FullName.Split().Length != 2) 
            {
                result.IsValid = false;
                result.Messages.Add("User full name must contain first and last name");
            }
            else if (model.FullName.Length > 100)
            {
                result.IsValid = false;
                result.Messages.Add("User full name cannot be longer than 100 characters");
            }
            else if (model.FullName.Length < 5)
            {
                result.IsValid = false;
                result.Messages.Add("User full name cannot be shorter than 5 characters");
            }

            if(string.IsNullOrWhiteSpace(model.Email))
            {
                result.IsValid = false;
                result.Messages.Add("User email cannot be empty");
            }
            else if(model.Email.Length > 50)
            {
                result.IsValid = false;
                result.Messages.Add("User email cannot be longer than 50 characters");
            }
            else if(model.Email.Length < 5)
            {
                result.IsValid = false;
                result.Messages.Add("User email cannot be shorter than 5 characters");
            }
            else 
            {
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if(!regex.IsMatch(model.Email))
                {
                    result.IsValid = false;
                    result.Messages.Add("User email is not in valid format");
                }
                else 
                {
                    var user = (await _unitOfWork.UserRepository.GetAllAsync()).FirstOrDefault(x => x.Email.Equals(model.Email));
                    if (user != null)
                    {
                        result.IsValid = false;
                        result.Messages.Add($"User with email {model.Email} already exists");
                    }
                }
            }

            if(model.BookIds != null && model.BookIds.Any())
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

            if(model.ProfilePicture == null)
            {
                result.IsValid = false;
                result.Messages.Add("User profile picture cannot be empty");
            }
            else if(model.ProfilePicture.Length > 10000000)
            {
                result.IsValid = false;
                result.Messages.Add("User profile picture cannot be larger than 10MB");
            }

            return result;
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(UserDTO model)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            if (model.Id == default)
            {
                result.IsValid = false;
                result.Messages.Add("User id cannot be empty");
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
                    var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(model.Id);
                    var user = (await _unitOfWork.UserRepository.GetAllAsync()).FirstOrDefault(x => !currentUser.Username.Equals(x.Username) && x.Username.Equals(model.Username));
                    if (user != null)
                    {
                        result.IsValid = false;
                        result.Messages.Add($"User with username {model.Username} already exists");
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
            var author = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (author == null)
            {
                result.IsValid = false;
                result.Messages.Add($"User with id {id} does not exist");
            }
            return result;
        }
    }
}
