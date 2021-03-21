using BookStore.Domain.Models;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookStore.Infrastructure.Tests
{
    public class BookRepositoryTests
    {
        private readonly DbContextOptions<BookStoreDbContext> _options;

        public BookRepositoryTests()
        {
            // Use this when using SqlLite inMemory database
            _options = BookRepositoryTests.BookStoreDbContextOptionsSQLiteInMemory();
            BookStoreHelperTests.CreateDataBaseSQLiteInMemory(_options);

            // Use this. When Using a Ef Core InMemory database
            // _Optioon = BookStoreHelperTest.BookStoreDbContextOptionsEfCoreInMemory();
            
            //BookStoreHelperTests.CreateDateBaseEfCoreInMemory(_options);

        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfBook_WhenBooksExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var books = await bookRepository.GetAll();

                Assert.NotNull(books);
                Assert.IsType<List<Book>>(books);
            }
        }

        [Fact]
        public async void GetAll_ShouldRetunAnEmptyList_WhenBooksDoNotExist()
        {
            await BookStoreHelperTests.CleanDataBase(_options);

            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);
                var books = await bookRepository.GetAll();

                Assert.NotNull(books);
                Assert.Empty(books);
                Assert.IsType<List<Book>>(books);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfBookWithCorrectValues_WhenBookExist()
        {
            await using (var context = new BookStoreDbContext(_options))
            {
                var bookRepository = new BookRepository(context);

                var expectedBooks = new BookRepository(context);
                var bookList = await bookRepository.GetAll();



            }
        }
    }
}
