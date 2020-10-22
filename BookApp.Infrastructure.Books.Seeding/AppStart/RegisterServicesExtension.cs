﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace BookApp.Infrastructure.Books.Seeding.AppStart
{
    public static class RegisterServicesExtension
    {
        public static void RegisterBooksSeeding(this IServiceCollection services, IConfiguration configuration)
        {
             var debug = services.RegisterAssemblyPublicNonGenericClasses()
                .AsPublicImplementedInterfaces();
        }
    }
}