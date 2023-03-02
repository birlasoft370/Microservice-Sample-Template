using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.DataTransfer.Examples
{
    [Table("Example")]
    public class ExampleDto : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExampleId { get; set; }
        public string ExampleName { get; set; }
    }
}
