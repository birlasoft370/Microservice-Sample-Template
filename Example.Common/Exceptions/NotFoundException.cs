namespace Example.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg)
           : base(msg)
        {
        }
        public NotFoundException(string entity, int id)
            : base($"{entity} {id} not found.")
        {
        }

        public NotFoundException(string entity, Guid id)
            : base($"{entity} {id} not found.")
        {
        }

        public NotFoundException(string entity, string id)
            : base($"{entity} {id} not found.")
        {
        }
    }
}
