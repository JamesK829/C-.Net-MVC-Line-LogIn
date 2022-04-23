using AutoMapper;
using Asha.Model;
using PagedList;

namespace AshaAdmin
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(c =>
            {
                c.AddProfile<CategoryProfile>();
                c.AddProfile<ProductProfile>();
                c.AddProfile<TimeProfile>();
                c.AddProfile<ReservedProfile>();
            });
        }
        //Category
        public class CategoryProfile : Profile
        {
            public override string ProfileName
            {
                get
                {
                    return "CategoryProfile";
                }
            }

            public CategoryProfile()
            {
                ConfigureMappings();
            }

            protected void ConfigureMappings()
            {
                CreateMap<PRODUCT_CATEGORY, CategoryForm>();
                CreateMap<CategoryForm, PRODUCT_CATEGORY>();
            }
        }
        //Product
        public class ProductProfile : Profile
        {
            public override string ProfileName
            {
                get
                {
                    return "ProductProfile";
                }
            }

            public ProductProfile()
            {
                ConfigureMappings();
            }

            protected void ConfigureMappings()
            {
                CreateMap<PRODUCT, ProductForm>();
                CreateMap<ProductForm, PRODUCT>();
            }
        }
        //Time
        public class TimeProfile : Profile
        {
            public override string ProfileName
            {
                get
                {
                    return "TimeProfile";
                }
            }

            public TimeProfile()
            {
                ConfigureMappings();
            }

            protected void ConfigureMappings()
            {
                CreateMap<SERVE_TIME, TimeForm>();
                CreateMap<TimeForm, SERVE_TIME>();
            }
        }
        //Reserved
        public class ReservedProfile : Profile
        {
            public override string ProfileName
            {
                get
                {
                    return "ReservedProfile";
                }
            }

            public ReservedProfile()
            {
                ConfigureMappings();
            }

            protected void ConfigureMappings()
            {
                CreateMap<IPagedList<ORDER>, IPagedList<ReservedForm>>();
                CreateMap<IPagedList<ReservedForm>, IPagedList<ORDER>>();
                CreateMap<ORDER, ReservedForm>();
                CreateMap<ReservedForm, ORDER>();

                CreateMap<CUSTOMER, CustomerForm>();
                CreateMap<CustomerForm, CUSTOMER>();
            }
        }
    }
}