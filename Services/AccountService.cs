using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using Microsoft.IdentityModel.Tokens;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Models.Request;
using thu6_pvo_dictionary.Repositories;
using AutoMapper;
using System.Web;

namespace thu6_pvo_dictionary.Services
{
    public class AccountService
    {
        private readonly UserRepository _userRepository;
        private readonly ApiOption _apiOption;
        private readonly IMapper _mapper;
        private readonly DictionaryRepository _dictionaryRepository;
        private readonly AppDbContext _databaseContext;

        public AccountService(ApiOption apiOption, AppDbContext databaseContext, IMapper mapper)
        {
            _userRepository = new UserRepository(apiOption, databaseContext, mapper);
            _apiOption = apiOption;
            _mapper = mapper;
            _dictionaryRepository = new DictionaryRepository(apiOption, databaseContext, mapper);
            _databaseContext = databaseContext;
        }

        public object UserLogin(UserLoginRequest userLoginRequest)
        {
            try
            {
                var user = _userRepository.UserLogin(userLoginRequest);
                if (user == null)
                    throw new ValidateError(1000, "Incorrect email or password");

                if (user.status == 0)
                    throw new ValidateError(1004, "Unactivated account");

                var token = GenerateJwtToken(user);

                var dictionaries = _databaseContext.dictionaries
                    .Where(d => d.user_id == user.user_id)
                    .Select(d => new { DictionaryId = d.dictionary_id, DictionaryName = d.dictionary_name })
                    .ToList();

                return new
                {
                    token,
                    UserId = user.user_id,
                    UserName = user.user_name,
                    DisplayName = user.display_name,
                    Avatar = user.avatar,
                    Dictionaries = dictionaries
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object UserRegister(UserRegisterRequest userRegisterRequest)
        {
            try
            {
                ValidateUserRegistration(userRegisterRequest);

                var newUser = new User
                {
                    user_name = userRegisterRequest.username,
                    password = Untill.CreateMD5(userRegisterRequest.password),
                    email = userRegisterRequest.username,
                    status = 0,
                };

                _userRepository.CreateEntity(newUser);
                _userRepository.SaveChange();
                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendActivateEmail(SendActivateEmailRequest sendActivateEmailRequest)
        {
            try
            {
                var user = _userRepository.GetEntitiesByCondition(row =>
                    row.user_name == sendActivateEmailRequest.username &&
                    row.password == Untill.CreateMD5(sendActivateEmailRequest.password))
                    .FirstOrDefault();

                if (user == null)
                    throw new ValidateError(1002, "Invalid account");

                // Generate the activation token
                var token = GenerateActivationToken(user);

                var activationLink = $"{_apiOption.BaseUrl}/api/account/activate_account?username={sendActivateEmailRequest.username}&token={token}";
                var mailBody = $"Bạn đã đăng ký tài khoản thành công. Bấm vào đây để active tài khoản: <a href=\"{activationLink}\">Activate Now</a>";

                SendEmail(user.user_name, "PVO Dictionary send email active account", mailBody);
                return activationLink;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateActivationToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            if (_apiOption == null)
            {
                throw new ArgumentNullException(nameof(_apiOption), "ApiOption object cannot be null.");
            }

            if (string.IsNullOrEmpty(_apiOption.Secret))
            {
                throw new ArgumentNullException(nameof(_apiOption.Secret), "API Secret is not set.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiOption.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimList = new List<Claim>
    {
        new Claim("ActivationData", "activate"),
        new Claim(ClaimTypes.UserData, user.user_name ?? string.Empty),
        new Claim(ClaimTypes.Sid, user.user_id.ToString()),
    };

            var token = new JwtSecurityToken(
                issuer: _apiOption.ValidIssuer ?? string.Empty,
                audience: _apiOption.ValidAudience ?? string.Empty,
                expires: DateTime.Now.AddDays(1),
                claims: claimList,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public bool ActivateAccount(string username)
        {
            try
            {
                var user = _userRepository.GetAllEntities().FirstOrDefault(row => row.user_name == username);
                if (user == null)
                    return false;

                user.status = 1;
                _userRepository.UpdateEntity(user);
                _userRepository.SaveChange();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ForgotPassword(string email)
        {
            try
            {
                var user = _userRepository.GetEntitiesByCondition(row => row.user_name == email).FirstOrDefault();
                if (user == null)
                    throw new ValidateError(1002, "Invalid account");

                // Generate the reset token
                var token = GenerateResetPasswordToken(user);

                // Properly URL encode the token
                var encodedToken = HttpUtility.UrlEncode(token);

                // Create the reset link with the encoded token
                var resetLink = $"{_apiOption.BaseUrl}/api/account/reset_password?email={email}&token={encodedToken}";
                var mailBody = $"Click on the link below to reset your password: <a href=\"{resetLink}\">Reset Password</a>";

                SendEmail(user.user_name, "PVO Dictionary Password Reset", mailBody);
                return resetLink;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ResetPassword(string email, string token, string newPassword)
        {
            try
            {
                var user = _userRepository.GetEntitiesByCondition(row => row.user_name == email).FirstOrDefault();
                if (user == null)
                    throw new ValidateError(1002, "Invalid account");

       

                // Update the user's password
                user.password = Untill.CreateMD5(newPassword);
                _userRepository.UpdateEntity(user);
                _userRepository.SaveChange();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiOption.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.UserData, user.user_name),
                new Claim(ClaimTypes.Sid, user.user_id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _apiOption.ValidIssuer,
                audience: _apiOption.ValidAudience,
                expires: DateTime.Now.AddDays(1),
                claims: claimList,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void ValidateUserRegistration(UserRegisterRequest userRegisterRequest)
        {
            var existingUser = _userRepository.GetEntitiesByCondition(row => row.user_name == userRegisterRequest.username).FirstOrDefault();
            if (existingUser != null)
                throw new ValidateError(1001, "Email has been used");

            if (string.IsNullOrEmpty(userRegisterRequest.username))
                throw new ValidateError(9000, "Email is required");

            if (string.IsNullOrEmpty(userRegisterRequest.password))
                throw new ValidateError(9000, "Password is required");

            if (!IsValidEmail(userRegisterRequest.username))
                throw new ValidateError(9000, "Incorrect email format");

            if (!IsValidPassword(userRegisterRequest.password))
                throw new ValidateError(9000, "Incorrect password format");
        }

        private void SendEmail(string recipient, string subject, string body)
        {
            try
            {
                using (var mail = new MailMessage("phattrienphanmemhmh123@gmail.com", recipient, subject, body))
                {
                    mail.IsBodyHtml = true;
                    using (var client = new SmtpClient("smtp.gmail.com"))
                    {
                        client.Host = "smtp.gmail.com";
                        client.UseDefaultCredentials = false;
                        client.Port = 587;
                        client.Credentials = new System.Net.NetworkCredential("phattrienphanmemhmh123@gmail.com", "nsjndsjndjand");
                        client.EnableSsl = true;
                        client.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateOTP()
        {
            var listNumber = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            var otp = "";
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                var randomNumber = rand.Next(0, 9);
                otp += listNumber[randomNumber];
            }
            return otp;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,16}$");
            return regex.IsMatch(password);
        }
        private string GenerateResetPasswordToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            if (_apiOption == null)
            {
                throw new ArgumentNullException(nameof(_apiOption), "ApiOption object cannot be null.");
            }

            if (string.IsNullOrEmpty(_apiOption.Secret))
            {
                throw new ArgumentNullException(nameof(_apiOption.Secret), "API Secret is not set.");
            }

            // Set the expiration time for the reset token (e.g., 1 hour)
            var tokenExpiration = DateTime.UtcNow.AddHours(1);

            // Create the token and encode the user ID and expiration time in the claims
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiOption.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimList = new List<Claim>
            {
                new Claim("ResetPasswordData", "reset"),
                new Claim(ClaimTypes.UserData, user.user_name ?? string.Empty),
                new Claim(ClaimTypes.Sid, user.user_id.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _apiOption.ValidIssuer ?? string.Empty,
                Audience = _apiOption.ValidAudience ?? string.Empty,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Subject = new ClaimsIdentity(claimList)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool IsValidResetPasswordToken(User user, string token)
        {
            if (user == null || string.IsNullOrEmpty(token))
            {
                return false;
            }

            if (_apiOption == null)
            {
                throw new ArgumentNullException(nameof(_apiOption), "ApiOption object cannot be null.");
            }

            if (string.IsNullOrEmpty(_apiOption.Secret))
            {
                throw new ArgumentNullException(nameof(_apiOption.Secret), "API Secret is not set.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiOption.Secret));

            try
            {
                // Validate the token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = !string.IsNullOrEmpty(_apiOption.ValidIssuer),
                    ValidateAudience = !string.IsNullOrEmpty(_apiOption.ValidAudience),
                    ValidateLifetime = true,
                    ValidIssuer = _apiOption.ValidIssuer, // Set your valid issuer here
                    ValidAudience = "https://example.com/api", // Set your valid audience (base URL) here
                    IssuerSigningKey = securityKey,
                    ClockSkew = TimeSpan.Zero // No clock skew
                };

                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var userDataClaim = claimsPrincipal.FindFirst(ClaimTypes.UserData);

                if (userDataClaim != null && int.TryParse(userDataClaim.Value, out int userId))
                {
                    // Verify if the token corresponds to the correct user ID
                    if (user.user_id == userId)
                    {
                        // Verify if the token has not expired
                        return validatedToken.ValidTo >= DateTime.UtcNow;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token validation failed: " + ex.Message);
            }

            return false;
        }

    }
}
