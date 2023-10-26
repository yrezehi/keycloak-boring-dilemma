namespace IdentityMapper.Models
{
    public class Properties
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public Properties(string property, string value) =>
            (Property, Value) = (property, value);

        public Properties New(string proeprty, string value) => 
            new Properties(Property, Value);
    }
}
