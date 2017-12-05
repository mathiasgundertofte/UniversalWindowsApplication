using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Submission
    {
        private UserData data;
        private string name;

        public Submission(string name, UserData data)
        {
            this.name = name;
            this.data = data;
        }

        public string Name { get => name; set => name = value; }
    }
}
