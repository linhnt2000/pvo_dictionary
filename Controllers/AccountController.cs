using AutoMapper;
using AutoMapper.Configuration;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Controllers;
using thu6_pvo_dictionary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Request;
using thu6_pvo_dictionary.Repositories;
using System.Web;

namespace thu6_pvo_dictionary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseApiController<AccountController>
    {
        private readonly AccountService _accountService;
        private readonly UserRepository _userRepository;
        public AccountController(AppDbContext databaseContext, IMapper mapper, ApiOption apiConfig)
        {
            _accountService = new AccountService(apiConfig, databaseContext, mapper);
            _userRepository = new UserRepository(apiConfig, databaseContext, mapper);
        }

        /// <summary>
        /// Get achievement list of user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public MessageData Login(UserLoginRequest userLoginRequest)
        {
            try
            {
                var res = _accountService.UserLogin(userLoginRequest);
                return new MessageData { Data = res, Status = 1, Message = "Login thanh cong" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        /// <summary>
        /// account register
        /// </summary>
        /// <param name="userRegisterRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public MessageData Register(UserRegisterRequest userRegisterRequest)
        {
            try
            {
                var res = _accountService.UserRegister(userRegisterRequest);
                return new MessageData { Data = res, Status = 1, Message = "Dang ky thanh cong" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        /// <summary>
        /// Send Activate Email
        /// </summary>
        /// <param name="sendActivateEmailRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("send_activate_email")]
        public MessageData SendActivateEmail(SendActivateEmailRequest sendActivateEmailRequest)
        {
            try
            {
                var res = _accountService.SendActivateEmail(sendActivateEmailRequest);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData() { Code = "Fail", Message = ex.Message, Status = 2, ErrorCode =1002 };
            }
        }

        /// <summary>
        /// Activate Account
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("activate_account")]
        public MessageData ActivateAccount(string username)
        {
            try
            {
                var res = _accountService.ActivateAccount(username);
                if (res)
                {
                    return new MessageData { Message = " active tài khoản thành công " , Status = 1 };
                }
                return new MessageData { Message = "Active tài khoản thất bại", Status = 2 };
            }
            catch (Exception ex)
            {
                return new MessageData { Code = "Fail", Message = ex.Message, Status = 2 };
            }
        }

        /// <summary>
        /// forgot password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("forgot_password")]
        public MessageData ForgotPassword(string email)
        {
            try
            {
                var res = _accountService.ForgotPassword(email);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        /// <summary>
        /// reset password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("reset_password")]
        public MessageData ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                // Decode the token received in the request
                var decodedToken = HttpUtility.UrlDecode(resetPasswordRequest.Token);

                var res = _accountService.ResetPassword(resetPasswordRequest.Email, decodedToken, resetPasswordRequest.NewPassword);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }


    }
}
