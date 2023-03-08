// Copyright © CompanyName. All Rights Reserved.
using System.ComponentModel.DataAnnotations;

namespace Example.DomainModel.Example
{
    public class ExampleModel : DomainBaseEntity
    {
        public int ExampleId { get; set; }

        [Required(ErrorMessage = "NotEmpty")]
        public string ExampleName { get; set; }
        public char? IsActive { get; set; }
    }
}
