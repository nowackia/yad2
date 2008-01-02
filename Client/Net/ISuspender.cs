using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Client
{
    public interface ISuspender
    {
        void Suspend();
        void Resume();
    }
}
