using System.Reflection.Metadata;

namespace SurvayBasket.Abstractions.Consts.cs
{
    public static class DefaultRole
    {
        public const string Admin  = nameof(Admin);
        public const string AdminRoleId = "01954439-8011-7cca-9a77-c5bf8fae0bae"; 
        public const  string AdminConcurrencyStamp  = "01954439-8011-7cca-9a77-c5c08d1d3c39";


        public const string Member = nameof(Member);
        public const string MemberRoleId  = "01954439-8011-7cca-9a77-c5c32b24d682";
        public const string MemberConcurrencyStamp  = "01954439-8011-7cca-9a77-c5c4f014795f";
    }
}
