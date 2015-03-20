using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using NHibernate.Mapping.ByCode;
//Needs a ref. to System.Data.Entity.Design.dll

namespace NHibernateConsoleTests.Core
{
    public static class NamingConventions
    {
        private static readonly PluralizationService _service = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        public static void ApplyNamingConventions(this ModelMapper mapper)
        {
            mapper.BeforeMapClass += (modelInspector, type, map) =>
            {
                PluralizeEntityName(type, map);
                PrimaryKeyConvention(type, map);
            };
            mapper.BeforeMapManyToOne += (modelInspector, member, map) =>
            {
                ReferenceConvention(modelInspector, member, map);
            };
            mapper.BeforeMapBag += (modelInspector, member, map) =>
            {
                OneToManyConvention(modelInspector, member, map);
            };
            mapper.BeforeMapManyToMany += (modelInspector, member, map) =>
            {
                ManyToManyConvention(modelInspector, member, map);
            };
        }

        public static void ManyToManyConvention(IModelInspector modelInspector, PropertyPath member, IManyToManyMapper map)
        {
            map.ForeignKey(
                string.Format("fk_{0}_{1}",
                       member.LocalMember.Name,
                       member.GetContainerEntity(modelInspector).Name));
        }

        public static void OneToManyConvention(IModelInspector modelInspector, PropertyPath member, IBagPropertiesMapper map)
        {
            var inv = member.LocalMember.GetInverseProperty();
            if (inv == null)
            {
                map.Key(x => x.Column(member.GetContainerEntity(modelInspector).Name + "Id"));
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                map.BatchSize(20);
                map.Inverse(true);
            }
        }

        public static void PluralizeEntityName(Type type, IClassAttributesMapper map)
        {
            map.Table(_service.Pluralize(type.Name));
        }

        public static void PrimaryKeyConvention(Type type, IClassAttributesMapper map)
        {
            map.Id(k =>
            {
                k.Generator(Generators.Native);
                k.Column(type.Name + "Id");
            });
        }

        public static void ReferenceConvention(IModelInspector modelInspector, PropertyPath member, IManyToOneMapper map)
        {
            map.Column(k => k.Name(member.LocalMember.GetPropertyOrFieldType().Name + "Id"));
            map.ForeignKey(
                string.Format("fk_{0}_{1}",
                       member.LocalMember.Name,
                       member.GetContainerEntity(modelInspector).Name));
            map.Cascade(Cascade.All | Cascade.DeleteOrphans);
        }
    }
}
