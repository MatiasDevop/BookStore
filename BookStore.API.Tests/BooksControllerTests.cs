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
            var dtoExpected = MapModelToBookResultListDto();

            _bookServiceMock.Setup(c => c.GetAll()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookResultDto>>(It.IsAny<List<Book>>())).Returns(dtoExpected);

            await _booksController.GetAll();

            _bookServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnOk_WhenBookExist()
        {
            var book = CreateBook();
            var dtoExpected = new MapModelToBookResultDto(book);

            _bookServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookResultDto>(It.IsAny<Book>())).Returns(dtoExpected);

            var result = await _booksController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }


    }
}
