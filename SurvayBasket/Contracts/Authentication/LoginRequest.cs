﻿namespace SurvayBasket.Contracts.Authentication
{
    public record LoginRequest(
        string Email,
        string Password
        );
   
}
