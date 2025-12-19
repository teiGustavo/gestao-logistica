using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace GestaoLogisticaAPI.Infrastructure.Routing;

public class ApiRoutePrefixConvention(string prefix = "api") : IApplicationModelConvention
{
    private readonly AttributeRouteModel _prefix = new(new RouteAttribute(prefix));

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (!typeof(Controllers.AbstractController).IsAssignableFrom(controller.ControllerType))
            {
                continue;
            }

            var hasAttributeRoute = controller.Selectors.Any(s => s.AttributeRouteModel != null);

            if (hasAttributeRoute)
            {
                foreach (var selector in controller.Selectors.Where(s => s.AttributeRouteModel != null))
                {
                    selector.AttributeRouteModel =
                        AttributeRouteModel.CombineAttributeRouteModel(_prefix, selector.AttributeRouteModel);
                }
                
                continue;
            }
           
            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel =
                    AttributeRouteModel.CombineAttributeRouteModel(
                        _prefix,
                        new AttributeRouteModel(new RouteAttribute("")));
            }
        }
    }
}