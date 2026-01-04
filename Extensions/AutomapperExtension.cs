using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.AuthorizationExtension;

public static class AutomapperExtension
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection service)
    {
        service.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<PurchaseDto, Purchase>()
                .ForMember(
                    dest => dest.SerialNumbers,
                    opt => opt.MapFrom(src =>
                        src.SerialNumbers != null
                            ? src.SerialNumbers.Select(s => new SerialNumbers
                            {
                                serial = s,
                                PurchaseId = src.PurchaseId,
                                ProductId = src.ProductId

                            }).ToList()
                            : null
                    )
                );

            cfg.CreateMap<Purchase, PurchaseDto>()
                .ForMember(
               dest => dest.SerialNumbers,
               opt => opt.MapFrom(src =>
                   src.SerialNumbers != null
                       ? src.SerialNumbers.Select(s => s.serial).ToList()
                       : null
               )
           );
        });

        return service;
    }
}