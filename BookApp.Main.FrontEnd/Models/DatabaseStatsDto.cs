﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using BookApp.Books.Domain;
using BookApp.Books.Persistence;

namespace BookApp.Main.FrontEnd.Models
{
    public class DatabaseStatsDto
    {
        public DatabaseStatsDto(BookDbContext context)
        {
            NumBooks = context.Books.Count();
            NumReviews = context.Set<Review>().Count();
            NumBookAuthors = context.Set<BookAuthor>().Count();
            NumAuthors = context.Authors.Count();
            NumBookTags = context.Set<BookTag>().Count();
            NumTags = context.Tags.Count();
        }

        public int NumBooks { get; }
        public int NumReviews { get; }
        public int NumBookAuthors { get; }
        public int NumAuthors { get; }
        public int NumBookTags { get; }
        public int NumTags { get; }
    }
}