using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServerSightAPI.Responses;

namespace ServerSightAPI.Configurations.Services
{
    public static class ModelStateHandler
    {
        public static void ConfigureModelStateHandler(this IServiceCollection services)
        {
            services.AddMvc()
                .ConfigureApiBehaviorOptions(options =>
                {
                    //options.SuppressModelStateInvalidFilter = true;
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var modelState = actionContext.ModelState;

                        var fieldErrorsList = new List<FieldError>();
                        foreach (var model in modelState)
                        foreach (var error in model.Value.Errors)
                            fieldErrorsList.Add(new FieldError(model.Key, error.ErrorMessage));
                        var fieldErrors = new FieldErrors(fieldErrorsList);
                        return new BadRequestObjectResult(fieldErrors);
                    };
                });
        }
    }
}