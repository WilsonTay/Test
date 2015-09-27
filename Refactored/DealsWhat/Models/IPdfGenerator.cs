using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Models
{
    using System.ServiceModel;

    [ServiceContract(Namespace = "urn:ps")]
    interface IPdfGenerator
    {
        [OperationContract]
        byte[] GeneratePdfWithHtml(string html);
    }

    interface IPdfGeneratorChannel : IPdfGenerator, IClientChannel { }
}
