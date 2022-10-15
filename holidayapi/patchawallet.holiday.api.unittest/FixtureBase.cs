using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace patchawallet.holiday.api.unittest
{
    public class FixtureBase
    {
        protected T LoadJson<T>(string name)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var file = path + "\\Mocks\\" + name + ".json";
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                T item = JsonConvert.DeserializeObject<T>(json);

                return item;
            }
        }
    }
}
