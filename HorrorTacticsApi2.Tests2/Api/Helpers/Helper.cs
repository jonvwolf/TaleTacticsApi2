using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests2.Api.Helpers
{
    internal class Helper
    {
        internal static StringContent GetContent<T>(T obj)
        {
            string str = JsonConvert.SerializeObject(obj);
            return new StringContent(str, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        internal async static Task<T> VerifyAndGetAsync<T>(HttpResponseMessage response)
        {
            var str = await response.Content.ReadAsStringAsync();
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(str);

                Assert.NotNull(obj);
                if (obj == null)
                    throw new InvalidOperationException("This will never be executed");
                return obj;
            }
            catch (Exception ex)
            {
                throw new AggregateException(str, ex);
            } 
        }
    }
}
