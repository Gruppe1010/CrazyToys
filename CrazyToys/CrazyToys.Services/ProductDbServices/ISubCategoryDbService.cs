namespace CrazyToys.Services.ProductDbServices
{
    public interface ISubCategoryDbService
    {

        Task<SubCategory> Create(SubCategory subCategory);

    }
}