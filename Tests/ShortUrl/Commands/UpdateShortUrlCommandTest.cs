using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.ShortUrl.Commands;
using Xunit;

namespace Tests.ShortUrl.Commands
{
    public class UpdateShortUrlCommandTest
    {
        private ApplicationDbContext GetContext()
        {
            return ApplicationDbContextFactory.Create();
        }
        [Fact]
        public async Task ShouldUpdateUrlAndSaveInDatabaseAsync()
        {
            Fixture fixture = new Fixture();
            var command = fixture.Build<UpdateShortUrlCommand>()
                .With(x => x.UrlId, 1)
                .Create();

            var context = GetContext();

            fixture.Customize<UpdateShortUrlCommandHandler>(c =>
                c.FromFactory(() =>
                new UpdateShortUrlCommandHandler(context))
            );

            var sut = fixture.Create<UpdateShortUrlCommandHandler>();
            ShortUrlModel result = await sut.Handle(command, CancellationToken.None);
            var entity = context.ShortUrlModels.Find(result.Id);

            Assert.NotNull(entity);
        }

        [Fact]
        public async Task GivenInvalidIdAndShouldThrowException()
        {
            Fixture fixture = new Fixture();
            var command = fixture.Build<UpdateShortUrlCommand>()
                .With(x => x.UrlId, 33)
                .Create();

            var context = GetContext();

            fixture.Customize<UpdateShortUrlCommandHandler>(c =>
                c.FromFactory(() =>
                new UpdateShortUrlCommandHandler(context))
            );

            var sut = fixture.Create<UpdateShortUrlCommandHandler>();

            await Assert.ThrowsAnyAsync<Exception>(() => 
            sut.Handle(command, CancellationToken.None));
        }
    }
}
