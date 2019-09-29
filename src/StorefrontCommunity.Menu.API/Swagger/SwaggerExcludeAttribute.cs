using System;
using System.Diagnostics.CodeAnalysis;

namespace StorefrontCommunity.Menu.API.Swagger
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SwaggerExcludeAttribute : Attribute { }
}
