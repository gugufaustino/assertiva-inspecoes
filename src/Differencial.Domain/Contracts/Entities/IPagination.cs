using System;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IPagination  
    {
        int Skip { get; set; }
        int Take { get; set; }
        int TotalRecords { get; set; }
        bool EnablePaging { get; }

    }
}