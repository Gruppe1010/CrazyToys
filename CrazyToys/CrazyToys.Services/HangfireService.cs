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
        public IProductDataService ProductDataService { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
