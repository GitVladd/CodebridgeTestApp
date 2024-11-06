namespace CodebridgeTestApp.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string str)  : base(str) { } 
    }
}
