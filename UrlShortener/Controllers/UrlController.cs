using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Models;
using UrlShortener.ShortUrl.Commands;

namespace UrlShortener.Controllers
{
    public class UrlController : ApiControllerBase
    {
        [HttpPost("create-url")]
        public async Task<ActionResult<ShortUrlModel>> CreateShortUrl(CreateShortUrlCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("get-url")]
        public async Task<ActionResult<string>> GetUrl([FromQuery] GetUrlByShortUrlQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpDelete("delete-url")]
        public async Task<ActionResult> DeleteUrl(DeleteUrlCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpGet("redirect")]
        public async Task<IActionResult> RedirectToUrl([FromQuery] GetUrlByShortUrlQuery query)
        {
            var page = await Mediator.Send(query);
            var result = Redirect(page);
            return result;
        }

        [HttpPut("update-url")]
        public async Task<ActionResult<ShortUrlModel>> ChangeShortUrl(UpdateShortUrlCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
