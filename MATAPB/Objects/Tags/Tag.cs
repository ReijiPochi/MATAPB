using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX.Direct3D11;
using System.Reflection;
using System.IO;

namespace MATAPB.Objects.Tags
{
    public abstract class Tag : AutoDisposeObject
    {
        protected bool valueChanged;
        public abstract string GetShaderText();
        public abstract void SetVariables(Effect effect);
        public abstract void Download(RenderingContext context);

        protected static string LoadShaderText(string name)
        {
            string shader;
            Assembly a = Assembly.GetExecutingAssembly();
            string[] resources = a.GetManifestResourceNames();
            using (StreamReader sr = new StreamReader(a.GetManifestResourceStream("MATAPB.Objects.Tags." + name)))
            {
                shader = sr.ReadToEnd();
            }

            return shader;
        }

        protected static Dictionary<string, string> Disassemble(string shader)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] lines = shader.Split('\r', '\n');
            bool loading = false;
            string key = null, data = null;

            foreach(string line in lines)
            {
                if (line.Length > 2)
                {
                    if (!loading && line[0] == '#' && line[1] == '#')
                    {
                        loading = true;

                        foreach (char c in line)
                        {
                            if (c != '#')
                                key += c;
                        }

                        continue;
                    }
                    else if (line == "##end")
                    {
                        result.Add(key, data);

                        loading = false;
                        key = null;
                        data = null;

                        continue;
                    }
                    else if (loading)
                    {
                        data += line + "\r\n";
                    }
                }
            }

            return result;
        }
    }
}
