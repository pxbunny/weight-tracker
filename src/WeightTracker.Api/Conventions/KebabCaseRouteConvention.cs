using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WeightTracker.Api.Conventions;

public sealed partial class KebabCaseRouteConvention : RouteTokenTransformerConvention
{
    private static readonly KebabCaseParameterTransformer Transformer = new();
    
    public KebabCaseRouteConvention() : base(Transformer)
    {
    
    }
    
    private sealed partial class KebabCaseParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value is not string valueStr || string.IsNullOrWhiteSpace(valueStr))
                return string.Empty;

            return MyRegex().Replace(valueStr, "-$1")
                .Trim()
                .ToLower();
        }

        [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}
