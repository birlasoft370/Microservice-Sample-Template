using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.DomainModel.Example
{
    public class ExampleModel : DomainBaseEntity
    {
        public int ExampleId { get; set; }
        public string ExampleName { get; set; }
    }
}
