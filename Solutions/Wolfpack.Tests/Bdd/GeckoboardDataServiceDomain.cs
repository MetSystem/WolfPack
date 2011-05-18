using Wolfpack.Core.Geckoboard;
using Wolfpack.Core.Geckoboard.DataProvider;

namespace Wolfpack.Tests.Bdd
{
    public abstract class GeckoboardDataServiceDomain : BddTestDomain
    {
        protected IGeckoboardDataServiceImpl myActivity;
        protected IGeckoboardDataProvider myDataProvider;
        protected IColourPicker myColourPicker;

        protected GeckoboardDataServiceDomain()
        {
            myColourPicker = new DefaultColourPicker();
        }
    }
}