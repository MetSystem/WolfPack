using Wolfpack.Contrib.Geckoboard.DataProvider;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Contrib.Geckoboard.Tests
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