
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurvayBasket.Abstractions.Consts.cs;
using SurvayBasket.Helper.cs;
using System.Text;

namespace SurvayBasket.services

{
    public class AuthService(UserManager<AppUser> userManager, IJwtProvider jwtProvider, 
                            ILogger<AuthService> logger , IHttpContextAccessor httpContextAccessor,
                            IEmailSender emailService  ,
                            AppDbContext context,
                            SignInManager<AppUser> signInManager) : IAuthService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> logger = logger;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender emailService = emailService;
        private readonly AppDbContext context = context;
        private readonly SignInManager<AppUser> signInManager = signInManager;

        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
        {

            var user = await _userManager.FindByEmailAsync(Email);

            if (user is null) 
                return Result.Falire<AuthResponse>(UsersErrors.IvalidCredential) ;


            if(user.IsDeleted)
                return Result.Falire<AuthResponse>(UsersErrors.DisabledUser);


            //var IsValidPassword = await _userManager.CheckPasswordAsync(user, Password);

            //if (!IsValidPassword) 
            //    return Result.Falire<AuthResponse>(UsersErrors.IvalidCredential);



            //if (!user.EmailConfirmed)
            //    return Result.Falire<AuthResponse>(UsersErrors.NotConfirmedEamil);

            var result = await signInManager.PasswordSignInAsync(user, Password , false ,true ); // check --> 1- check password  2- check Email confirmation 3- check lock 


            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var userPermissions = (from r in context.Roles
                                       join p in context.RoleClaims
                                       on r.Id equals p.RoleId
                                       where userRoles.Contains(r.Name!)
                                       select p.ClaimValue)
                                       .Distinct()
                                       .ToList();

                (string token, int expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);
                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);

                return Result.Success(response);

            }

            var error = result.IsNotAllowed ? UsersErrors.NotConfirmedEamil : result.IsLockedOut ? UsersErrors.LockedUser : UsersErrors.IvalidCredential;

            return Result.Falire<AuthResponse>(error);


        }

        public async Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
        {
            var EmailIsExist = await _userManager.FindByEmailAsync(registerRequest.Email);

            if (EmailIsExist is not null )
            {
                return Result.Falire(UsersErrors.DublicatedEmail) ;

            }

            var user = registerRequest.Adapt<AppUser>() ;
         
            var result = await _userManager.CreateAsync(user,registerRequest.Password) ;
              
            // problem in creation
            if (!result.Succeeded)
            { 
                var error = result.Errors.First();

                return Result.Falire(new Error(error.Code, error.Description));

            }
         
   
            // Generate verification code

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user) ;

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)) ;

            logger.LogInformation("Confirmation code : {code} ", code) ;

            // Send email 

            var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailStyle", templateModel:
                new Dictionary<string, string>
                {

                    {"{{name}}",$"{user.FirstName} {user.LastName}" },
                    {"{{action_url}}", $"{origin}/Auth/Confirm-Email?userId={user.Id},code={code}" }

                });

            await emailService.SendEmailAsync(registerRequest.Email, "Survay Basket Team", emailBody);


            return Result.Success() ;

        }

        public async Task<Result> ConfirmEmailAsync(ConfirmationEmailRequest ConfirmEmailRequest, CancellationToken cancellationToken = default)
        {

            var user = await _userManager.FindByIdAsync(ConfirmEmailRequest.Id);

            if (user is null)
            {
                return Result.Falire(UsersErrors.InvalidCode);

            }

            if (user.EmailConfirmed)
            {
                return Result.Falire(UsersErrors.EmailIsConfirmBefore);
               
            }

            var code = ConfirmEmailRequest.ConfirmationToken;

            try
            { 
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)  
            { 
                return Result.Falire(UsersErrors.InvalidCode);
            }



            var result = await _userManager.ConfirmEmailAsync(user, code);


            if (!result.Succeeded)
            {

                var error = result.Errors.First();

                return Result.Falire(new Error (error.Code,error.Description));

            }


            await _userManager.AddToRoleAsync(user, DefaultRole.Member);

            return Result.Success() ;

        }

        public async Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmail ResendConfirmationEmailRequest, CancellationToken cancellationToken = default)
        {


            var user = await _userManager.FindByEmailAsync(ResendConfirmationEmailRequest.Email) ;

            if (user is null)
            {
                return Result.Success(); // 

            }

            if (user.EmailConfirmed)
            {
                return Result.Falire(UsersErrors.EmailIsConfirmBefore);

            }

            // Generate verification code

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            logger.LogInformation("Confirmation code : {code} ", code);

            // Send email 
            var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailStyle", templateModel:
                new Dictionary<string, string>
                {

                    {"{{name}}",$"{user.FirstName} {user.LastName}" },
                    {"{{action_url}}", $"{origin}/Auth/Confirm-Email?userId={user.Id},code={code}" }

                });

            await emailService.SendEmailAsync(ResendConfirmationEmailRequest.Email, "Survay Basket Team", emailBody);

            return Result.Success();

        }
    }


}
