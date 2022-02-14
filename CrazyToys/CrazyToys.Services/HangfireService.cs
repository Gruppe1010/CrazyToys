using CrazyToys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IProductDataService _productDataService;

        private HangfireService(IProductDataService productDataService)
        {

        }

        public void GetDaily()
        {
            throw new NotImplementedException();
        }

        public void GetIndex()
        {
            throw new NotImplementedException();
        }
    }
}
