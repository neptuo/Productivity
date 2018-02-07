using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public interface ICursorService
    {
        void Move(string filePath, int position);
    }
}
