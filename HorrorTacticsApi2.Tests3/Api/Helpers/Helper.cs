using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    internal class Helper
    {
        internal static StringContent GetContent<T>(T obj)
        {
            string str = JsonConvert.SerializeObject(obj);
            return new StringContent(str, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        internal async static Task<T> VerifyAndGetAsync<T>(HttpResponseMessage response, int expectedHttpStatusCode)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Content);

            var str = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode != expectedHttpStatusCode)
            {
                throw new InvalidOperationException($"The status code is different from expected. Expected: {expectedHttpStatusCode} Actual: {response.StatusCode}"
                    + Environment.NewLine + "Response string: " + Environment.NewLine + str);
            }

            T? obj;
            try
            {
                obj = JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception ex)
            {
                throw new AggregateException(str, ex);
            }

            if (obj == null)
                throw new InvalidOperationException($"Deserialized object is null: '{str}'");
            return obj;
        }
    }
}
