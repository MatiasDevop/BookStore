using AutoMapper;
using BookStore.API.Controllers;
using BookStore.API.Dtos.Book;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookStore.API.Tests
{
    public class BooksControllerTests
    {
        private readonly BooksController _booksController;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public BooksControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _mapperMock = new Mock<IMapper>();
            _booksController = new BooksController(_mapperMock.Object, _bookServiceMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistBooks()
        {
            var books = CreateBookList();
            var dtoExpected = MapModelToBookResultListDto(books);

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            var result = await _booksController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenDoesNotExistAnyBook()
        {
            var books = CreateBookList();
            var dtoExpected = MapModelToBookResultListDto(books);

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            var result = await _booksController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromService_OnlyOnce()
        {
            var books = CreateBookList();
            var dtoExpected = MapModelToBookResultListDto(books);

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            await _booksController.GetAll();

            _bookServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnOk_WhenBookExist()
        {
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultDto(book); 

            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            var result = await _booksController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync((Book)null);

            var result = await _booksController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromService_OnlyOnce()
        {
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultDto(book);

            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            await _booksController.GetById(2);

            _bookServiceMock.Verify(mock => mock.GetById(2), Times.Once);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnOk_WhenBookWithSearchedCategoryExist()
        {
            var bookList = CreateBookList();
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultListDto(bookList);

            _bookServiceMock.Setup(c => c.GetBooksByCategory(book.CategoryId)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<IEnumerable<Book>>())).Returns(dtoExpected);

            var result = await _booksController.GetBooksByCategory(book.CategoryId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnNotFound_WhenBookWithSearchedCategoryDoesNotExist()
        {
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultDto(book);

            _bookServiceMock.Setup(c => c.GetBooksByCategory(book.CategoryId)).ReturnsAsync(new List<Book>());
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            var result = await _booksController.GetBooksByCategory(book.CategoryId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldCallGetBooksByCategoryFromService_OnlyOnce()
        {
            var bookList = CreateBookList();
            var book = CreateBook();
            var dtoExpected = MapModelToBookResultListDto(bookList);

            _bookServiceMock.Setup(c => c.GetBooksByCategory(book.CategoryId)).ReturnsAsync(bookList);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<IEnumerable<Book>>())).Returns(dtoExpected);

            await _booksController.GetBooksByCategory(book.CategoryId);

            _bookServiceMock.Verify(mock => mock.GetBooksByCategory(book.CategoryId), Times.Once);
        }

        private Book CreateBook()
        {
            return new Book()
            {
                Id = 2,
                Name = "Book Test",
                Author = "Author Test",
                Description = "Description Test",
                Value = 10,
                CategoryId = 1,
                PublishDate = DateTime.MinValue.AddYears(40),
                Category = new Category()
                {
                    Id = 1,
                    Name = "Category Test"
                }
            };
        }

        private BookResultDto MapModelToBookResultDto(Book book)
        {
            var bookDto = new BookResultDto()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Description = book.Description,
                PublishDate = book.PublishDate,
                Value = book.Value,
                CategoryId = book.CategoryId
            };
            return bookDto;
        }

        private List<Book> CreateBookList()
        {
            return new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Name = "Book Test 1",
                    Author = "Author Test 1",
                    Description = "Description Test 1",
                    Value = 10,
                    CategoryId = 1
                },
                new Book()
                {
                    Id = 1,
                    Name = "Book Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    Value = 20,
                    CategoryId = 1
                },
                new Book()
                {
                    Id = 1,
                    Name = "Book Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    Value = 30,
                    CategoryId = 2
                }
            };
        }

        private List<BookResultDto> MapModelToBookResultListDto(List<Book> books)
        {
            var listBooks = new List<BookResultDto>();

            foreach (var item in books)
            {
                var book = new BookResultDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Author = item.Author,
                    Description = item.Description,
                    PublishDate = item.PublishDate,
                    Value = item.Value,
                    CategoryId = item.CategoryId
                };
                listBooks.Add(book);
            }
            return listBooks;
        }
    }
}
