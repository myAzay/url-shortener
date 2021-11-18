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
    public class DeleteUrlCommandTest
    {
        [Fact]
        public async void ShouldDeleteItemInDatabase()
        {
            Fixture fixture = new Fixture();
            var command = fixture.Build<DeleteUrlCommand>()
                .With(x => x.Id, 1)
                .Create();

            var context = ApplicationDbContextFactory.Create();

            fixture.Customize<DeleteUrlCommandHandler>(c =>
                c.FromFactory(() =>
                new DeleteUrlCommandHandler(context))
            );

            var sut = fixture.Create<DeleteUrlCommandHandler>();
            await sut.Handle(command, CancellationToken.None);
            var entity = context.ShortUrlModels.Find(1);

            Assert.Null(entity);
        }
    }
}
