using Microsoft.AspNetCore.Mvc;
using PgsKanban.Api.Extensions;
using PgsKanban.Import;
using PgsKanban.Import.Dtos;

namespace PgsKanban.Api.Controllers
{
    [Route("/api/[controller]")]
    public class ImportController: Controller
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost, Route("")]
        public IActionResult ImportBoard([FromForm] FileFormDto data)
        {
            var result = _importService.ImportBoard(data, User.GetUserId());
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
