using System;
using System.IO;
using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using PgsKanban.BusinessLogic.Config;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Import.Dtos;

namespace PgsKanban.Import.Tests
{
    [TestFixture]
    public class ImportServiceTests
    {
        private const string USER_ID = "0a9d633055a748edbcf871b244e80a3a";
        private const string CUSTOM_BOARD_NAME = "My new board";
        private const string EMPTY_BOARD_FILENAME = "emptyBoard.json";
        private const string BOARD_WITH_LISTS_FILENAME = "boardWithListsWithoutCards.json";
        private const string BOARD_WITH_LISTS_AND_CARDS_FILENAME = "boardWithListsAndCards.json";
        private const string BOARD_WITH_LISTS_CARDS_AND_COMMENTS_FILENAME = "boardWithListsCardsAndComments.json";
        private const string INVALID_FILENAME = "invalidFile.json";
        private const int EXPECTED_NUMBER_OF_ITEMS = 3;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUserBoardRepository> _userBoardRepositoryMock;
        private Mock<IObfuscator> _obfuscatorMock;
        private StreamReader _streamReader;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = AutoMapperConfig.Initialize();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userBoardRepositoryMock = new Mock<IUserBoardRepository>();
            _obfuscatorMock = new Mock<IObfuscator>();
            SetUpMocks();
        }

        [TearDown]
        public void TearDown()
        {
            _streamReader.Dispose();
        }

        [Test]
        public void ImportBoard_ShouldReturnProperStatisctics_IfBoardWithoutListsInFile()
        {
            var importService = new ImportService(_userRepositoryMock.Object, _userBoardRepositoryMock.Object, _mapper, _obfuscatorMock.Object);
            
            var result = importService.ImportBoard(CreateFormFileDto(EMPTY_BOARD_FILENAME), USER_ID);

            Assert.AreEqual(0, result.ListsCount);
            Assert.AreEqual(0, result.CardsCount);
            Assert.AreEqual(0, result.CommentsCount);
            Assert.AreEqual(CUSTOM_BOARD_NAME, result.UserBoard.Board.Name);
            Assert.AreEqual(USER_ID, result.UserBoard.UserId);
        }

        [Test]
        public void ImportBoard_ShouldReturnProperStatistics_IfBoardWithListsAndWithoutCardsInFile()
        {
            var importService = new ImportService(_userRepositoryMock.Object, _userBoardRepositoryMock.Object, _mapper, _obfuscatorMock.Object);

            var result = importService.ImportBoard(CreateFormFileDto(BOARD_WITH_LISTS_FILENAME), USER_ID);

            Assert.AreEqual(EXPECTED_NUMBER_OF_ITEMS, result.ListsCount);
            Assert.AreEqual(0, result.CardsCount);
            Assert.AreEqual(0, result.CommentsCount);
            Assert.AreEqual(CUSTOM_BOARD_NAME, result.UserBoard.Board.Name);
            Assert.AreEqual(USER_ID, result.UserBoard.UserId);
        }

        [Test]
        public void ImportBoard_ShouldReturnProperStatistics_IfBoardWithListsAndCards()
        {
            var importService = new ImportService(_userRepositoryMock.Object, _userBoardRepositoryMock.Object, _mapper, _obfuscatorMock.Object);

            var result = importService.ImportBoard(CreateFormFileDto(BOARD_WITH_LISTS_AND_CARDS_FILENAME), USER_ID);

            Assert.AreEqual(EXPECTED_NUMBER_OF_ITEMS, result.ListsCount);
            Assert.AreEqual(EXPECTED_NUMBER_OF_ITEMS, result.CardsCount);
            Assert.AreEqual(0, result.CommentsCount);
            Assert.AreEqual(CUSTOM_BOARD_NAME, result.UserBoard.Board.Name);
            Assert.AreEqual(USER_ID, result.UserBoard.UserId);
        }

        [Test]
        public void ImportBoard_ShouldReturnProperStatistics_IfBoardWithListsCardsAndComments()
        {
            var importService = new ImportService(_userRepositoryMock.Object, _userBoardRepositoryMock.Object, _mapper, _obfuscatorMock.Object);

            var result = importService.ImportBoard(CreateFormFileDto(BOARD_WITH_LISTS_CARDS_AND_COMMENTS_FILENAME), USER_ID);

            Assert.AreEqual(EXPECTED_NUMBER_OF_ITEMS, result.ListsCount);
            Assert.AreEqual(EXPECTED_NUMBER_OF_ITEMS, result.CardsCount);
            Assert.AreEqual(EXPECTED_NUMBER_OF_ITEMS, result.CommentsCount);
            Assert.AreEqual(CUSTOM_BOARD_NAME, result.UserBoard.Board.Name);
            Assert.AreEqual(USER_ID, result.UserBoard.UserId);
        }

        [Test]
        public void ImportBoard_ShouldReturnNull_IfInvalidFile()
        {
            var importService = new ImportService(_userRepositoryMock.Object, _userBoardRepositoryMock.Object, _mapper, _obfuscatorMock.Object);

            var result = importService.ImportBoard(CreateFormFileDto(INVALID_FILENAME), USER_ID);

            Assert.IsNull(result);
        }

        private FileFormDto CreateFormFileDto(string fileName)
        {
            return new FileFormDto
            {
                BoardName = CUSTOM_BOARD_NAME,
                File = GetFile(fileName)
            };
        }

        private FormFile GetFile(string fileName)
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"PgsKanban.Import.Tests.TestFiles.{fileName}");
            _streamReader = new StreamReader(resourceStream, Encoding.UTF8);
            var stream = _streamReader.BaseStream;
            return new FormFile(stream, stream.Position, stream.Length, fileName, fileName);
        }

        private void SetUpMocks()
        {
            SetUpUserBoardRepositoryMock();
            SetUpUserRepositoryMock();
            SetUpObfuscatorMock();
        }

        private void SetUpUserBoardRepositoryMock()
        {
            _userBoardRepositoryMock.Setup(x => x.CreateUserBoard(It.IsAny<UserBoard>()))
                .Returns<UserBoard>(userBoard => userBoard);
        }

        private void SetUpUserRepositoryMock()
        {
            _userRepositoryMock.Setup(x => x.AddExternalUser(It.IsAny<ExternalUser>())).Returns<ExternalUser>(
                externalUser =>
                {
                    externalUser.Id = Guid.NewGuid().ToString("D");
                    return externalUser;
                });
            _userRepositoryMock.Setup(x => x.GetUserByFullName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<User>(null);
            _userRepositoryMock.Setup(x => x.GetExternalUserByFullName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<ExternalUser>(null);
        }

        private void SetUpObfuscatorMock()
        {
            _obfuscatorMock.Setup(x => x.Obfuscate(It.IsAny<int>())).Returns<int>(id => id.ToString());
            _obfuscatorMock.Setup(x => x.Deobfuscate(It.IsAny<string>())).Returns<string>(int.Parse);
        }
    }
}
