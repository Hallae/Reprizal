using myApi.Interfaces;

namespace myApi.Models
{
    public class GuidGenerator : IGuidGenerator
    {
        public Guid GenerateNewId()
        {
            return Guid.NewGuid();
        }

        public Guid GenerateNewAuthor()
        {
            return Guid.NewGuid();
        }
    }
}
