using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PgsKanban.Hubs.Interfaces
{
    public interface ISampleClientHandler
    {
        Task SayHello(string message);
    }
}
