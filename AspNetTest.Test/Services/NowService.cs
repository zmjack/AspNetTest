using System;

namespace AspNetTest.Test.Services
{
    public class NowService : INowService
    {
        public DateTime Now { get; }
        public NowService()
        {
            Now = DateTime.Now;
        }
    }
}
