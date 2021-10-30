using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Linq;
using System.Reflection;

namespace LoyaltyProgramUnitTests
{
    public class FixedControllerProvider : ControllerFeatureProvider
    {
        private readonly Type[] _controllerTypes;
        public FixedControllerProvider(params Type[] controllerTypes)
        {
            _controllerTypes = controllerTypes;
        }
        protected override bool IsController(TypeInfo typeInfo) => _controllerTypes.Contains(typeInfo);
    }
}
