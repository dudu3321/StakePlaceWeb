namespace stake_place_web.Entities
{
    public abstract class BaseLine
    {
        public string Id { get; protected set; }
        public string Description { get; protected set; }

        protected BaseLine (string description)
        {
            Description = description;
        }
    }

    public abstract class BaseLine<T> : BaseLine
    {
        public T Value { get; protected set; }

        protected BaseLine (T value, string description) : base (description)
        {
            Value = value;
            Id = $"{Value}";
        }

    }
}