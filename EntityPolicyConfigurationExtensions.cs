using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Bcc.EntityApi
{
    public static class EntityPolicyConfigurationExtensions
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            //First we find all the defined policies
            var policies = System.Reflection.Assembly.GetCallingAssembly().GetTypes()
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.FullName.Contains(typeof(EntityPolicy<>).FullName))
                .Select(type => (EntityType: type.BaseType.GetGenericArguments().First(), PolicyType: type))
                .ToList();

            // Then we register them in the service provider
            foreach (var (entityType, policyType) in policies)
            {
                services.AddScoped(typeof (EntityPolicy<>).MakeGenericType(entityType), policyType);
            }

            // Then we find all entities that extend the BaseEntity and that do not have a defined policy
            var entities = System.Reflection.Assembly.GetCallingAssembly().GetTypes()
                .Where(type => type.BaseType == typeof(BaseEntity) && ! policies.Any(item => item.EntityType == type))
                .ToList();

            // Then we create a default policy for them
            foreach (Type type in entities)
            {
                services.AddScoped(typeof(EntityPolicy<>), typeof(EntityPolicy<>));
            }
        }
    }
}