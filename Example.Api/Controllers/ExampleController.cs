using Example.DomainModel.Example;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Controllers
{
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly IMediator mediator;

        public ExampleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("api/v1/example/getExamples")]
        public async Task<IActionResult> GetExamples()
        {
            var result = await mediator.Send(new GetExamplesRequest()).ConfigureAwait(false);

            return Ok(result);
        }
    }
}
