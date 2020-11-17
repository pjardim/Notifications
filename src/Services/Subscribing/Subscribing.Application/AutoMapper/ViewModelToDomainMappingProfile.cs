using AutoMapper;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain;

namespace Subscribing.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //ViewModel to Domain should not use Automapper use model's constructor instead.

        }
    }
}