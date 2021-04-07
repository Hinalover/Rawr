using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    public interface IRotationOptions
    {
        bool UseChainLightning { get; }
        bool UseDpsFireTotem { get; }
        bool UseFireEle { get; }
    }
}
