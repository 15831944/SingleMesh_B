using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace Loader
{
    public class DiskFile : ILoader,IDisposable
    {
        private string[] content;

        public DiskFile(string fileName)
        {
            if (FStream == null)
            {
                FStream = new StreamReader(fileName);
            }
            Debug.Log(fileName);
        }

        public DiskFile(string[] content)
        {
            this.content = content;
        }


        protected StreamReader FStream;

        protected int ReadLine = 0;

        public Property Next()
        {
            if (content.Length > ReadLine)
            {
                var prop = new Property();
                try
                {
                    prop.Code = Convert.ToInt32(content[ReadLine++].Replace(" ", ""));
                    prop.Value = content[ReadLine++];
                }
                catch {
                    Debug.Log(ReadLine);
                }
                
                return prop;
            }
            else {
                return null;
            }

            //if (FStream.Peek() >= 0)
            //{
            //    var prop = new Property()
            //    {
            //        Code = Convert.ToInt32(FStream.ReadLine()),
            //        Value = FStream.ReadLine(),
            //    };
            //    ReadLine += 2;
            //    //Debug.Print(ReadLine.ToString() + "\t" + prop.Code.ToString() + "\t" + prop.Value);
            //    return prop;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public void Dispose()
        {
            if (FStream != null)
                FStream.Close();
        }

       
    }
}