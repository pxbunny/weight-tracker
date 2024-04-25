namespace WeightTracker.Api.Models;

/// <summary>
/// Represents the weight data group.
/// </summary>
/// <remarks>
/// This class is used to group weight data together and calculate the average, maximum, and minimum weight values.
/// </remarks>
internal sealed class WeightDataGroup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeightDataGroup"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor is private to prevent external classes from creating instances of this class.
    /// It's due to the fact that it needs to calculate values,
    /// so the only way to create an instance is through the <see cref="Create"/> method.
    /// </remarks>
    private WeightDataGroup()
    {
    }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    /// <value>The user ID.</value>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the average weight.
    /// </summary>
    /// <value>The average weight.</value>
    public decimal? AverageWeight { get; set; }

    /// <summary>
    /// Gets or sets the maximum weight.
    /// </summary>
    /// <value>The maximum weight.</value>
    public decimal? MaxWeight { get; set; }

    /// <summary>
    /// Gets or sets the minimum weight.
    /// </summary>
    /// <value>The minimum weight.</value>
    public decimal? MinWeight { get; set; }

    /// <summary>
    /// Gets or sets the weight data.
    /// </summary>
    /// <remarks>
    /// The weight data is a collection of <see cref="WeightData"/> instances.
    /// </remarks>
    /// <value>The weight data.</value>
    /// <seealso cref="WeightData"/>
    public IEnumerable<WeightData> Data { get; set; } = Enumerable.Empty<WeightData>();

    /// <summary>
    /// Creates a new instance of the <see cref="WeightDataGroup"/> class.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="data">The weight data.</param>
    /// <returns>The new instance of the <see cref="WeightDataGroup"/> class.</returns>
    public static WeightDataGroup Create(string userId, IEnumerable<WeightData> data)
    {
        var dataList = data.ToList();

        var dataGroup = new WeightDataGroup
        {
            UserId = userId,
            Data = dataList
        };

        if (dataList.Count == 0)
        {
            return dataGroup;
        }

        dataGroup.AverageWeight = Math.Round(dataList.Average(x => x.Weight), 2);
        dataGroup.MaxWeight = dataList.Max(x => x.Weight);
        dataGroup.MinWeight = dataList.Min(x => x.Weight);

        return dataGroup;
    }
}
