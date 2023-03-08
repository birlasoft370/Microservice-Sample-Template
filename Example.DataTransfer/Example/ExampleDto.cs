using Example.Common.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.DataTransfer.Examples
{
    [Table("Example")]
    public class ExampleDto : BaseEntity
    {
        [Key]
        public int ExampleId { get; set; }
        public string? ExampleName { get; set; }
        public char? IsActive { get; set; } = (char)AppConstants.IsActive.Yes;
    }
}