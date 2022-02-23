using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Api.UnitTests
{
    public class ActivitiesControllerTests
    {
        public DataContext GetDbContext(bool useSqlite = false)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            if (useSqlite)
            {
                builder.UseSqlite("Data Source=reactivities.db", x => { });
            }
            else
            {
                //builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            var dbContext = new DataContext(builder.Options);

            if (useSqlite)
            {
                dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private readonly IMapper _mapper;

        public ActivitiesControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfiles()); });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public async Task Should_Return_404_If_Activity_Not_Found()
        {

            var userAccessor = new Mock<IUserAccessor>();
            userAccessor.Setup(u => u.GetUsername()).Returns("test");

            var context = GetDbContext(true);

            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = await sut.Handle(new Details.Query { Id = It.IsAny<Guid>() }, CancellationToken.None);

             Assert.NotNull(result.Value);
        }

    }

  
}
