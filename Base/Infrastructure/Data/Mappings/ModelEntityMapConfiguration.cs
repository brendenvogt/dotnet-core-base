using AutoMapper;

namespace Infrastructure.Data.Mappings
{
    public class ModelEntityMapConfiguration : Profile
    {
        public ModelEntityMapConfiguration() : this("default")
        {
        }

        protected ModelEntityMapConfiguration(string profileName) : base(profileName)
        {
        }
    }
}