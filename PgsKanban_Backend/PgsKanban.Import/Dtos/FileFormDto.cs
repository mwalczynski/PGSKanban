using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace PgsKanban.Import.Dtos
{
    public class FileFormDto
    {
        public IFormFile File { get; set; }
        public string BoardName { get; set; }
    }
}
