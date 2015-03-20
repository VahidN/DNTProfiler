using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace NHibernateConsoleTests.Core
{
    public static class ManyToManyConventions
    {
        public static void AddAllManyToManyRelations(this ModelMapper mapper, IEnumerable<Type> entities)
        {
            var mappedItemsCache = new List<string>();
            foreach (var entity in entities)
            {
                var properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    var inverseProperty = GetInverseProperty(property);

                    if (inverseProperty == null) continue;
                    if (mappedItemsCache.Contains(inverseProperty.PropertyType.FullName) ||
                        mappedItemsCache.Contains(property.PropertyType.FullName)) continue;

                    CallGenericMapManytoManyMethod(mapper, property, inverseProperty);

                    mappedItemsCache.Add(inverseProperty.PropertyType.FullName);
                    mappedItemsCache.Add(property.PropertyType.FullName);
                }
            }
        }

        public static void CallGenericMapManytoManyMethod(this ModelMapper mapper, PropertyInfo property, PropertyInfo inverseProperty)
        {
            var method = typeof(ManyToManyConventions).GetMethod("MapManyToMany", BindingFlags.Public | BindingFlags.Static)
                                                      .MakeGenericMethod(property.DeclaringType, inverseProperty.DeclaringType);
            method.Invoke(null, new object[] { mapper, property, inverseProperty });
        }

        public static PropertyInfo GetInverseProperty(this PropertyInfo property)
        {
            var type = property.PropertyType;
            var to = type.DetermineCollectionElementOrDictionaryValueType();
            if (to == null)
            {
                // no generic collection or simple property
                return null;
            }

            var expectedInversePropertyType = type.GetGenericTypeDefinition()
                .MakeGenericType(property.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties().FirstOrDefault(x => x.PropertyType == expectedInversePropertyType && x != property);
        }

        public static PropertyInfo GetInverseProperty(this MemberInfo member)
        {
            var type = member.GetPropertyOrFieldType();
            var to = type.DetermineCollectionElementOrDictionaryValueType();
            if (to == null)
            {
                // no generic collection or simple property
                return null;
            }

            var expectedInversePropertyType = type.GetGenericTypeDefinition()
                                                  .MakeGenericType(member.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties().FirstOrDefault(x => x.PropertyType == expectedInversePropertyType);
        }

        public static void ManyToMany<TControllingEntity, TInverseEntity>(
            this ModelMapper mapper,
            Expression<Func<TControllingEntity, IEnumerable<TInverseEntity>>> controllingProperty,
            Expression<Func<TInverseEntity, IEnumerable<TControllingEntity>>> inverseProperty
            )
            where TControllingEntity : class
            where TInverseEntity : class
        {
            var controllingPropertyName = ((MemberExpression)controllingProperty.Body).Member.Name;
            var controllingColumnName = string.Format("{0}Id", controllingPropertyName);
            var inverseColumnName = string.Format("{0}Id", typeof(TControllingEntity).Name);
            var tableName = string.Format("{0}To{1}", typeof(TControllingEntity).Name, controllingPropertyName);
            mapper.Class<TControllingEntity>(map => map.Bag(controllingProperty,
                                                            cm =>
                                                            {
                                                                cm.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                                                cm.Table(tableName);
                                                                cm.Key(km => km.Column(controllingColumnName));
                                                            },
                                                            em => em.ManyToMany(m => m.Column(inverseColumnName))));
            mapper.Class<TInverseEntity>(map => map.Bag(inverseProperty,
                                                        cm =>
                                                        {
                                                            cm.Table(tableName);
                                                            cm.Inverse(true);
                                                            cm.Key(km => km.Column(inverseColumnName));
                                                        },
                                                        em => em.ManyToMany(m => m.Column(controllingColumnName))));
        }

        public static void ManyToMany<TControllingEntity, TInverseEntity>(
            this ModelMapper mapper,
            Expression<Func<TControllingEntity, IEnumerable<TInverseEntity>>> controllingProperty
            )
            where TControllingEntity : class
            where TInverseEntity : class
        {
            var controllingPropertyName = ((MemberExpression)controllingProperty.Body).Member.Name;
            var controllingColumnName = string.Format("{0}Id", controllingPropertyName);
            var inverseColumnName = string.Format("{0}Id", typeof(TControllingEntity).Name);
            var tableName = string.Format("{0}To{1}", typeof(TControllingEntity).Name, controllingPropertyName);
            mapper.Class<TControllingEntity>(map => map.Bag(controllingProperty,
                                                            cm =>
                                                            {
                                                                cm.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                                                cm.Table(tableName);
                                                                cm.Key(km => km.Column(controllingColumnName));
                                                            },
                                                            em => em.ManyToMany(m => m.Column(inverseColumnName))));
        }

        public static void MapManyToMany<TControllingEntity, TInverseEntity>(
             this ModelMapper mapper, PropertyInfo property, PropertyInfo inverseProperty
            )
            where TControllingEntity : class
            where TInverseEntity : class
        {
            var controllingPropertyName = property.DeclaringType.Name;
            var controllingColumnName = string.Format("{0}Id", controllingPropertyName);
            var inverseColumnName = string.Format("{0}Id", inverseProperty.DeclaringType.Name);
            var tableName = string.Format("{0}To{1}", inverseProperty.Name, property.Name);

            mapper.Class<TControllingEntity>(map => map.Bag<TInverseEntity>(property.Name,
                                                            cm =>
                                                            {
                                                                cm.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                                                cm.Table(tableName);
                                                                cm.Key(km => km.Column(controllingColumnName));
                                                                cm.Lazy(CollectionLazy.NoLazy);
                                                            },
                                                            em => em.ManyToMany(m => m.Column(inverseColumnName))));
            mapper.Class<TInverseEntity>(map => map.Bag<TControllingEntity>(inverseProperty.Name,
                                                        cm =>
                                                        {
                                                            cm.Table(tableName);
                                                            cm.Inverse(true);
                                                            cm.Key(km => km.Column(inverseColumnName));
                                                            cm.Lazy(CollectionLazy.NoLazy);
                                                        },
                                                        em => em.ManyToMany(m => m.Column(controllingColumnName))));

        }
    }
}