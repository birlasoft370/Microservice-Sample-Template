// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Messages;
using Example.Common.Utility;
using Example.DomainModel.Example;
using MediatR;
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

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(AppConstants.ApiVersion + "/example/TestApi")]
        public async Task<IActionResult> TestApi()
        {
            return Ok(await Task.FromResult("Welcome to Microservice world!"));
        }

        /// <summary>
        /// Get examples
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(AppConstants.ApiVersion + "/example/getExamples")]
        public async Task<IActionResult> GetExamples()
        {
            var result = await mediator.Send(new GetExamplesRequest()).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// Get existing example by Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [HttpGet]
        [Route(AppConstants.ApiVersion + "/example/getExampleById")]
        public async Task<IActionResult> GetExampleById([FromQuery] GetExampleByIdRequest query)
        {
            var result = await mediator.Send(query).ConfigureAwait(false);
            return Ok(CustomSuccessResponse.GetSuccessResponse<CommonResponse, ExampleModel, string>(result.ExampleModel, AppConstants.SuccessMessage.List.ToString()));
        }

        /// <summary>
        /// add example
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [HttpPost]
        [Route(AppConstants.ApiVersion + "/example/addExample")]
        public async Task<IActionResult> AddExample([FromBody] AddExampleRequest request)
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Ok(CustomSuccessResponse.GetSuccessResponse<CommonResponse, ExampleModel, string>(result.ExampleModel, AppConstants.SuccessMessage.Inserted.ToString()));
        }


        /// <summary>
        /// update example
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [HttpPut(AppConstants.ApiVersion + "/example/updateExample")]
        public async Task<IActionResult> UpdateExample([FromBody] UpdateExampleRequest request)
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Ok(CustomSuccessResponse.GetSuccessResponse<CommonResponse, ExampleModel, string>(result.ExampleModel, AppConstants.SuccessMessage.Updated.ToString()));
        }

        /// <summary>
        /// Add & Update example
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [HttpPost]
        [Route(AppConstants.ApiVersion + "/example/addUpdateExample")]
        public async Task<IActionResult> AddUpdateExample([FromBody] AddUpdateExampleRequest request)
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Ok(CustomSuccessResponse.GetSuccessResponse<CommonResponse, List<ExampleModel>, string>(result.ExampleModels, AppConstants.SuccessMessage.Updated.ToString()));
        }

        /// <summary>
        /// Delete Example
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [HttpDelete]
        [Route(AppConstants.ApiVersion + "/example/deleteExample")]
        public async Task<IActionResult> DeleteExample([FromBody] DeleteExampleRequest request)
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Ok(CustomSuccessResponse.GetSuccessResponse<CommonResponse, int, string>(result.ExampleId, AppConstants.SuccessMessage.Deleted.ToString()));
        }

        [ProducesResponseType(typeof(CommonResponse), StatusCodes.Status200OK)]
        [HttpPost]
        [Route(AppConstants.ApiVersion + "/example/bulkInsertExample")]
        public async Task<IActionResult> BulkInsertExample([FromForm] BulkInsertExampleRequest request)
        {
            var result = await mediator.Send(request).ConfigureAwait(false);

            return Ok(CustomSuccessResponse.GetSuccessResponse<CommonResponse, List<ExampleModel>, string>(result.ExampleModels, AppConstants.SuccessMessage.Inserted.ToString()));
        }
    }
}
