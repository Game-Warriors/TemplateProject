using GameWarriors.AnalyticDomain.Abstraction;



namespace AnalyticDomain.Core
{
#if METRIX
using MetrixSDK;
    public class MetrixAnalyticHandler : IAnalyticHandler
    {
        private const string METRIX_API_KEY = "enhfqtzvsbnsifu";

        public MetrixAnalyticHandler()
        {
             Metrix.Initialize(METRIX_API_KEY);
        }

        void IAnalyticHandler.SetABTestTag(string abTestTag)
        {

        }
    }
#endif
}
