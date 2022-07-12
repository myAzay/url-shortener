using System;
using Xunit;
using AutoFixture;
using UrlShortener.ShortUrl.Commands;
using System.Threading;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using UrlShortener.Data;
using Moq;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using System.Threading.Tasks;

namespace Tests.ShortUrl.Commands
{
    public class CreateShortUrlCommandTest : TestBase
    {
        [Fact]
        public async Task ShouldCreateUrlAndSaveInDatabaseAsync()
        {
            Fixture fixture = new Fixture();
            var command = fixture.Build<CreateShortUrlCommand>()
                .With(x => x.LongUrl, "https://www.google.ru/")
                .Create();

            fixture.Customize<CreateShortUrlCommandHandler>(c => 
                c.FromFactory(() => 
                new CreateShortUrlCommandHandler(Context))
            );

            var sut = fixture.Create<CreateShortUrlCommandHandler>();
            ShortUrlModel result = await sut.Handle(command, CancellationToken.None);
            var entity = Context.ShortUrlModels.Find(result.Id);
 
            Assert.NotNull(entity);
            Assert.Equal(command.LongUrl, entity.OriginalUrl + "sdasdsadasd");
        }
    }
}
