﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Books.Domain.SupportTypes;
using BookApp.Books.Persistence;
using SoftDeleteServices.Configuration;

namespace BookApp.Main.FrontEnd.SoftDeleteConfig
{
    public class ConfigSoftDelete : SingleSoftDeleteConfiguration<ISoftDelete>
    {
        public ConfigSoftDelete(BookDbContext context)
            : base(context)
        {
            GetSoftDeleteValue = entity => entity.SoftDeleted;
            SetSoftDeleteValue = (entity, value) => entity.AlterSoftDelete(value);

        }
    }
}