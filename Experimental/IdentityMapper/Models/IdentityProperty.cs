namespace IdentityMapper.Models
{
    public class IdentityProperty
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public IdentityProperty(string property, string value) =>
            (Property, Value) = (property, value);

        public IdentityProperty New(string proeprty, string value) => 
            new IdentityProperty(Property, Value);
    }
}
