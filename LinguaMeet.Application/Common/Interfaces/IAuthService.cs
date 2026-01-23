using LinguaMeet.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Common.Interfaces
{
    public interface IAuthService
    {
            Task<IdentityResult> RegisterAsync(RegisterVM model);
            Task<SignInResult> LoginAsync(LoginVM model);
            Task LogoutAsync();
        

    }
}
