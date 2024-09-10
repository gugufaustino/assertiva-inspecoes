
using Differencial.Domain.Contracts.Entities;

namespace Differencial.Domain.Filters
{
    public class Pagination : IPagination
    {
        public Pagination()
        {
            Skip = 0;
            Take = int.MaxValue;
            TotalRecords = 0;
            Order = Order.Ascending;
            OrderField = "1";
        }

        public int Skip { get; set; }
        public int Take { get; set; }
        public int TotalRecords { get; set; }
        public bool EnablePaging => Take != int.MaxValue;
        
        public Order Order { get; protected set; }
        public string OrderField { get ; protected set; }

    }

    public enum Order
    {
        Ascending,
        Descending
    }

}
