﻿using AutoMapper;
using System.Reflection;

namespace RaportAPI.Core.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(p =>
                    p.GetInterfaces().Any(
                        i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                        )
                    ).ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
