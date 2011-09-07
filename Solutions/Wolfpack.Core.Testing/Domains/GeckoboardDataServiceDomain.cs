using Wolfpack.Core.Geckoboard;
using Wolfpack.Core.Geckoboard.DataProvider;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Core.Testing.Domains
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