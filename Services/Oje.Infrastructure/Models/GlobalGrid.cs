
namespace Oje.Infrastructure.Models
{
    public class GlobalGrid
    {
        public string sortField { get; set; }
        public bool? sortFieldIsAsc { get; set; }
        int? Skip;
        public int skip { get
            {
                if (Skip == null)
                    return 0;
                if (Skip <= 0)
                    return 0;
                return Skip.Value;
            }
            set
            {
                Skip = value;
            }
        }
        int? Take;
        public int take
        {
            get
            {
                if (Take == null)
                    return 10;
                if (Take <= 0)
                    return 10;
                if (Take > 1000)
                    return 1000;
                return Take.Value;
            }
            set
            {
                Take = value;
            }
        }
    }
}
