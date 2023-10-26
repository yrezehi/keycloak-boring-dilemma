namespace IdentityMapper.Models
{
    public class Property
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public Property(string property, string value) =>
            (Property, Value) = (property, value);

        public Property New(string proeprty, string value) => 
            new Property(Property, Value);
    }
}
