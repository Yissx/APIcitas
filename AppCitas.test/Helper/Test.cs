using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCitas.test.Helper;

public sealed class Test
{
    private static readonly Lazy<Test> _lazyInstance =
            new Lazy<Test>(() => new Test());

    public static Test Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    public HttpClient Client { get; set; }

    private Test()
    {
        // Place for instance initialization code
        Client = new APIWebApplicationFactory<Startup>().CreateDefaultClient();
    }
}
