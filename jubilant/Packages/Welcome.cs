using System;
using System.Collections.Generic;
using System.Text;

namespace jubilant.Packages
{
    class Welcome : Package
    {
        public Welcome() : base(PackageType.Welcome) { }

        public override string GetContent()
        {
            return Manager.username;
        }
    }
}
