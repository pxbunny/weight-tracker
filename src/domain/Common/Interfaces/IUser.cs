namespace WeightTracker.Domain.Common.Interfaces;

/// <summary>
/// Represents the currently logged-in user data.
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets the ID of the currently logged-in user.
    /// </summary>
    /// <value>The user ID.</value>
    public string? Id { get; }
}
