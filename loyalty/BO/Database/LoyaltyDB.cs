using System.Configuration;

namespace Palmary.Loyalty.BO.Database
{
    partial class LoyaltyDBDataContext
    {
        public LoyaltyDBDataContext()
            : base(ConfigurationManager.ConnectionStrings["loyaltydbConnectionString"].ConnectionString)
    {
        OnCreated();
    }
    }
}

