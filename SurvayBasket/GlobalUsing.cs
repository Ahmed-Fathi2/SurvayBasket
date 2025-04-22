global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

global using SurvayBasket.Entities;
global using SurvayBasket.services;
global using SurvayBasket.Contracts.Polls;

 
global using FluentValidation;
global using FluentValidation.AspNetCore;

global using Mapster;
global using MapsterMapper;
global using System.Reflection;

global using Microsoft.AspNetCore.Identity;
global using SurvayBasket.Contracts.Authentication;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using SurvayBasket.Abstractions;

global using SurvayBasket.Contracts.Answers;
global using SurvayBasket.Contracts.Questions;
global using SurvayBasket.Contracts.Results;
global using SurvayBasket.Contracts.Results.NumOfSelectionPerEachAnswer;
global using SurvayBasket.Contracts.Results.VoteResults;
global using SurvayBasket.Contracts.Results.VoteResultsPerDay.cs;
global using SurvayBasket.Data;
global using SurvayBasket.Errors;
global using SurvayBasket.UsreErrors;
global using System.Collections.Generic;
global using System.Linq;

global using SurvayBasket.Abstractions;
global using SurvayBasket.Authentication;
global using SurvayBasket.UsreErrors;

global using SurvayBasket.Abstractions;
global using SurvayBasket.Authentication;
global using SurvayBasket.UsreErrors;



global using SurvayBasket.Filter.cs;