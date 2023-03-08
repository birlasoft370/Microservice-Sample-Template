// Copyright © CompanyName. All Rights Reserved.
using FileHelpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Example.DomainModel.Example
{
    public class BulkInsertExampleRequest : IRequest<BulkInsertExampleResponse>
    {
        public BulkInsertExampleRequest()
        {
            BulkInsertExampleModel = new BulkInsertExampleModel();

        }
        public BulkInsertExampleModel BulkInsertExampleModel { get; set; }
    }

    public class BulkInsertExampleModel
    {
        [Required(ErrorMessage = "NotEmpty")]
        [AllowedExtensions(new string[] { ".csv" }, ErrorMessage = "uploaded filetype is not valid.")]
        public IFormFile UploadedFile { get; set; }
        [Required(ErrorMessage = "NotEmpty")]
        public string UserID { get; set; }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName);

            if (!_extensions.Contains(extension.ToLower()))
                return false;

            return true;
        }
    }

    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class BulkInsertExampleCsvSampleFormatModel
    {
        public string ExampleName { get; set; }
    }
}
