namespace HorrorTacticsApi2.Data
{
    /// <summary>
    /// The reason there is an interface to validate in Data and not Domain it is because:
    /// - Validation in here (IValidatableEntity) should be like attributes, not too complex
    /// - Only validation that can't do the database goes here, anything more complex should go in Domain
    /// - There are multiple Models (Create, Read, Update) and makes more sense to have validation in one class, like Converter
    /// </summary>
    public interface IValidatableEntity
    {
        void Validate();
    }
}
