using SurvayBasket.Contracts.Questions;
using SurvayBasket.Contracts.Role.cs;
using SurvayBasket.Contracts.User.cs;

namespace SurvayBasket.Mapping

{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>().Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));

            config.NewConfig<RegisterRequest, AppUser>()
                .Map(dest => dest.UserName, src => src.Email);


            config.NewConfig<UserRequest, AppUser>()
              .Map(dest => dest.UserName, src => src.Email)
              .Map(dest => dest.EmailConfirmed, src => true);

            config.NewConfig<UserUpdate, AppUser>()
             .Map(dest => dest.UserName, src => src.Email)
             .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper());





        }
    }
}
