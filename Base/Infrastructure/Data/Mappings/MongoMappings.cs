using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Collections;
using Core.Entities;

namespace Infrastructure.Data.Mappings
{
    public class MongoDbMapping
    {
        public static void RegisterMapping()
        {
            AddConventionPack();
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserEntity)))
            {
                BsonClassMap.RegisterClassMap<UserEntity>(cm =>
                {
                    cm.SetIdMember(cm.GetMemberMap(a => a.UserId));
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

        }

        static void AddConventionPack()
        {
            var conventionPacks = new ConventionPack {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true) };

            conventionPacks.AddMemberMapConvention("IgnoreEmptyList", m =>
            {
                if (m.MemberType.IsGenericType && m.MemberType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    m.SetShouldSerializeMethod(instance =>
                    {
                        var value = (ICollection)m.Getter(instance);
                        return value != null && value.Count > 0;
                    });
                }
            });
            ConventionRegistry.Register("Convension", conventionPacks, type => true);
        }
    }
}
