using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Models;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature, FeatureResource>();

            // API Resource to Domain
            CreateMap<VehicleResource, Vehicle>()
                // for VehicleResource map we need to supply additional configuration because
                // the shape of our domain class is different from shape of our api resource
                // v as vehicle goes to v.ContactName, the second argument tells where we can find the value of this expression
                // opt is operation object. vr as vehicleResource to vr.Contact.Name
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                // add another configuration for mapping the features, here we need to get the collection of featureId from vehicleResource
                // so for each integer we need to create a new vehicleFeature object
                // here we select all featureId and for each id we create new VehicleFeature then initialize it with {FeatureId = id}
                .ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(id => new VehicleFeature { FeatureId = id })));
        }
    }
}