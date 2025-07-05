﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurvayBasket.Abstractions;
using SurvayBasket.Contracts.AccountProfile.cs;
using SurvayBasket.Helper.cs;

namespace SurvayBasket.services
{
    public class AccountService(IHttpContextAccessor httpContextAccessor ,UserManager<AppUser> userManager ,
                        ILogger<AccountService> logger,IEmailSender emailSender ) : IAccountService
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly UserManager<AppUser> userManager = userManager;
        private readonly ILogger<AccountService> logger = logger;
        private readonly IEmailSender emailSender = emailSender;

        public async Task<Result<UserProfileResponse>> GetUserProfileAsync()
        {
           var userId = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var userInfo = await userManager.Users
                                .Where(x => x.Id == userId)
                                .ProjectToType<UserProfileResponse>().SingleAsync();
                              //  .Select(x=> new UserProfileResponse(x.FirstName, x.LastName, x.Email!, x.UserName!)).SingleAsync();
                 
            return Result.Success(userInfo);

        }

        public async Task<Result> UpdateUserProfileAsync(UserUpdatedProfileRequest Request)
        {
            var userId = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            //var user = await userManager.FindByIdAsync(userId);

            //user = Request.Adapt(user);

            //  await userManager.UpdateAsync(user!); -------->> bad performance

            await userManager.Users.Where(x => x.Id == userId)
                                   .ExecuteUpdateAsync(x => x.SetProperty(s => s.FirstName, Request.FirstName)
                                                            .SetProperty(e => e.LastName, Request.LastName));

            return Result.Success();
        }


        public async Task<Result> ChangeUserPassword(ChangePassWordRequest Request)
        {
            var userId = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var user = await userManager.FindByIdAsync(userId);

           var result =  await userManager.ChangePasswordAsync(user!, Request.CurrentPassword, Request.NewPassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Falire(new Error(error.Code, error.Description));
        }

        public async Task<Result> ForgetUserPassword(ForgetPasswordRequest Request)
        {
            var userId= httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await userManager.FindByEmailAsync(Request.Email);

            if(user is null)
                return Result.Success();

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


            logger.LogInformation("Rest Password {Token} ", code);

            var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var forgetPassEmailBody =  EmailBodyBuilder.GenerateEmailBody("ForgetPssword",


                new Dictionary<string, string>
                {

                    {"{{name}}",$"{user.FirstName}" },
                    {"{{action_url}}",$"{origin}/change-Password?Email={user.Email},code={code}" }
                  

                } );



            await emailSender.SendEmailAsync(user.Email!,"Survay Basket Team " ,forgetPassEmailBody);


            return Result.Success();


        }

        public async Task<Result> ResetUserPassword(ResetPasswordRequest Request)
        {


            var user = await userManager.FindByEmailAsync(Request.Email);

            if (user is null || !user.EmailConfirmed)
                return Result.Falire(UsersErrors.InvalidCode);

            var code = Request.Code;
            try
            {
                 code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Request.Code));
            }


            catch(FormatException) 
            {

                return Result.Falire(UsersErrors.InvalidCode);
            
            }



          

            var result =  await userManager.ResetPasswordAsync(user, code, Request.NewPassword);



            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Falire(new Error(error.Code, error.Description));


        }
    }
}
