using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.ShortUrl.Commands;
using Xunit;

namespace Tests.ShortUrl.Queries
{
    public class GetUrlByShortUrlQueryTest : TestBase
    {
        [Fact]
        public async Task ShouldReturnsCorrectModel()
        {
            Fixture fixture = new Fixture();
            var query = fixture.Build<GetUrlByShortUrlQuery>()
                .With(x => x.ShortUrl, "http://localhost:5001/qtdJwBun")
                .Create();

            fixture.Customize<GetUrlByShortUrlQueryHandler>(c =>
                c.FromFactory(() =>
                new GetUrlByShortUrlQueryHandler(Context))
            );

            var sut = fixture.Create<GetUrlByShortUrlQueryHandler>();

            string result = await sut.Handle(query, CancellationToken.None);
            var entity = Context.ShortUrlModels
                .Where(x => x.ShortUrl == query.ShortUrl)
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(1,entity.Id);
        }

        [Fact]
        public async Task GivenInvalidStringAndShouldThrowException()
        {
            Fixture fixture = new Fixture();
            var query = fixture.Build<GetUrlByShortUrlQuery>()
                .With(x => x.ShortUrl, "incorrectString")
                .Create();

            fixture.Customize<GetUrlByShortUrlQueryHandler>(c =>
                c.FromFactory(() =>
                new GetUrlByShortUrlQueryHandler(Context))
            );

            var sut = fixture.Create<GetUrlByShortUrlQueryHandler>();

            await Assert.ThrowsAnyAsync<Exception>(() =>
            sut.Handle(query, CancellationToken.None));
        }
    }
}
