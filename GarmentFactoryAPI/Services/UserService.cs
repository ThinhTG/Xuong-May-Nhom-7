using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Models;
using GermentFactory.Services;
using GermentFactoryAPI.Services;
using System;
using System.Threading.Tasks;

namespace GarmentFactoryAPI.Services
{
    public interface IUserService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(string code);
        Task<IBusinessResult> Save(User user);
        Task<IBusinessResult> Update(User user);
        Task<IBusinessResult> DeleteById(string code);
        Task<IBusinessResult> GetById1(int code);
        Task<IBusinessResult> DeleteById1(int code);
    }

    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(DataContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                #region Business rule
                #endregion

                var services = await _unitOfWork.UserRepository.GetAllAsync();

                if (services == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, services);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(string code)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByUsername(code);
                if (user == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, user);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(User user)
        {
            try
            {
                int result = await _unitOfWork.UserRepository.CreateAsync(user);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> Update(User user)
        {
            try
            {
                //int result = await _currencyRepository.UpdateAsync(currency);
                int result = await _unitOfWork.UserRepository.UpdateAsync(user);

                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }

        public async Task<IBusinessResult> DeleteById(string code)
        {
            try
            {
                //var currency = await _currencyRepository.GetByIdAsync(code);
                var currency = await _unitOfWork.UserRepository.GetByIdAsync(code);
                if (currency != null)
                {
                    //var result = await _currencyRepository.RemoveAsync(currency);
                    var result = await _unitOfWork.UserRepository.RemoveAsync(currency);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetById1(int code)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserById(code);
                if (user == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, user);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteById1(int code)
        {
            try
            {
                //var currency = await _currencyRepository.GetByIdAsync(code);
                var currency = await _unitOfWork.UserRepository.GetByIdAsync(code);

                currency.IsDeleted = true;
                if (currency != null)
                {
                    //var result = await _currencyRepository.RemoveAsync(currency);
                    var result = await _unitOfWork.UserRepository.UpdateAsync(currency);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            // Tìm kiếm người dùng trong cơ sở dữ liệu dựa trên tên đăng nhập và mật khẩu
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAndPasswordAsync(username, password);
            return user;
        }

        public async Task<User?> RegisterAsync(RegisterDTO userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                RoleId = userDto.roleId,
                IsDeleted = false
                // Map other properties as needed
            };

            bool usernameExists = await _unitOfWork.UserRepository.UsernameExistsAsync(userDto.Username);
            if (usernameExists)
            {
                return null; // Or throw an exception if preferred
            }

            int result = await _unitOfWork.UserRepository.CreateAsync(user);
            return result > 0 ? user : null;
        }
    }
}
