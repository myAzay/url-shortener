using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.ShortUrl.Commands;
using Xunit;

namespace Tests.ShortUrl.Commands
{
    public class DeleteUrlCommandTest : TestBase
    {
        [Fact]
        public async void ShouldDeleteItemInDatabase()
        {
            Fixture fixture = new Fixture();
            var command = fixture.Build<DeleteUrlCommand>()
                .With(x => x.Id, 1)
                .Create();

            fixture.Customize<DeleteUrlCommandHandler>(c =>
                c.FromFactory(() =>
                new DeleteUrlCommandHandler(Context))
            );

            var sut = fixture.Create<DeleteUrlCommandHandler>();
            await sut.Handle(command, CancellationToken.None);
            var entity = Context.ShortUrlModels.Find(1);

            Assert.Null(entity);
        }
    }
}
