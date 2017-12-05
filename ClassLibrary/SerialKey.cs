using System;
using System.Collections.Generic;


namespace ClassLibrary
{
    public class SerialKey
    {

        //private List<String> keys;

        public SerialKey()
        {
            //Keys = new List<String>();
        }

        //public List<string> Keys { get => keys; set => keys = value; }

        //public void AddKey(string key)
        //{
        //    Keys.Add(key);
        //}


        /*
         * Generates a Globally Unique Identifier (GUID)
         * which is a 128-bit number and returns it as a string
         * https://en.wikipedia.org/wiki/Universally_unique_identifier
         */
        public string GenerateSingleKey()
        {
            return Guid.NewGuid().ToString();
        }


        public void GenerateMultipleKeys(List<String> list, int amountOfKeys)
        {
            for (int i = 0; i < amountOfKeys; i++)
            {
                list.Add(GenerateSingleKey());
            }
        }

    }
}
