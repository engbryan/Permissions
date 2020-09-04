
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.Restier.Core.Model;
using Microsoft.Restier.Core.Query;
using Microsoft.Restier.EntityFramework;
using Northwind.API;
using Restier.DataContext;
using System;
using System.Linq;
using System.Web.Http;

namespace Restier
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

#if !PROD
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
#endif

            config.Filter().Expand().Select().OrderBy().MaxTop(100).Count().SetTimeZoneInfo(TimeZoneInfo.Utc);

            config.UseRestier<NorthwindApi>((services) =>
            {
                //services.AddTransient<IModelBuilder, CustomizedModelBuilder>();
                // This delegate is executed after OData is added to the container.
                // Add you replacement services here.
                services.AddEF6ProviderServices<NorthwindEntitiesContainer>();

                //services.AddSingleton<ODataSerializerProvider, CustomizedSerializerProvider>();

                services.AddSingleton<IQueryExpressionAuthorizer, CustomizedQueryAuthorizer>();
                services.AddSingleton<ODataPayloadValueConverter, CustomizedPayloadValueConverter>();

                //services.AddSingleton<IQueryExpressionInspector, CustomizedInspector>();

                services.AddSingleton(new ODataValidationSettings
                {
                    MaxTop = 5,
                    MaxAnyAllExpressionDepth = 3,
                    MaxExpansionDepth = 3,
                });
            });

            config.MapHttpAttributeRoutes();

            config.MapRestier<NorthwindApi>("ApiV1", "", true);
        }
    }
}
