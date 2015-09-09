namespace Axial.Lang.Internationalization
{
    public abstract class Country
    {
        public abstract CountryCode CountryCode { get; }
        public abstract Languages Language { get; }
        public abstract Currency Currency { get; }

        public abstract StateCodes StateCodes { get; }
    }

    public interface ICountry
    {
        enum States { get; }
    }
}
