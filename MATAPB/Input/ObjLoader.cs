using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using System.IO;

namespace MATAPB.Input
{
    public class ObjLoader
    {
        public static VertexData[] FromFile(string path, out int[] indexBuffer)
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<Indices> indices = new List<Indices>();

            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(' ');

                    switch (line[0])
                    {
                        case "v":
                            positions.Add(ToVector3(line[1], line[2], line[3]));
                            break;

                        case "vn":
                            normals.Add(ToVector3(line[1], line[2], line[3]));
                            break;

                        case "vt":
                            texCoords.Add(ToVector2(line[1], line[2]));
                            break;

                        case "f":
                            Indices i1 = Indices.FromText(line[1]);
                            Indices i2 = Indices.FromText(line[2]);
                            Indices i3 = Indices.FromText(line[3]);
                            if (i1 == null || i2 == null || i3 == null)
                                break;

                            indices.Add(i1);
                            indices.Add(i2);
                            indices.Add(i3);
                            break;

                        default:
                            break;
                    }
                }
            }

            VertexData[] result = null;
            indexBuffer = new int[indices.Count];

            result = Build(indices, positions, normals, texCoords, ref indexBuffer);

            return result;
        }

        private static VertexData[] Build(List<Indices> indices, List<Vector3> positions, List<Vector3> normals, List<Vector2> texCoords, ref int[] indexBuffer)
        {
            VertexData[] result = new VertexData[indices.Count];
            int count = 0;

            foreach (Indices index in indices)
            {
                result[count] = new VertexData()
                {
                    position = new MatVector3Float(positions[index.position].X, positions[index.position].Y, positions[index.position].Z),
                    normal = new MatVector3Float(normals[index.normal].X, normals[index.normal].Y, normals[index.normal].Z),
                    texCoord = new MatVector2Float(texCoords[index.texCoord].X, 1.0f - texCoords[index.texCoord].Y)
                };

                indexBuffer[count] = count;
                count++;
            }

            return result;
        }

        private static Vector2 ToVector2(string x, string y)
        {
            if (float.TryParse(x, out float X) && float.TryParse(y, out float Y))
                return new Vector2(X, Y);
            else
                return new Vector2(0.0f);
        }

        private static Vector3 ToVector3(string x, string y, string z)
        {
            if (float.TryParse(x, out float X) && float.TryParse(y, out float Y) && float.TryParse(z, out float Z))
                return new Vector3(X, Y, Z);
            else
                return new Vector3(0.0f);
        }

        private enum IndexType
        {
            Position,
            Normal,
            TexCoord
        }

        private class Indices
        {
            public Indices()
            {

            }

            public static Indices FromText(string indices)
            {
                string[] index = indices.Split('/');

                if (index.Length != 3)
                    return null;

                Indices result = new Indices();

                if (int.TryParse(index[0], out result.position) && int.TryParse(index[1], out result.texCoord) && int.TryParse(index[2], out result.normal))
                {
                    result.position--;
                    result.normal--;
                    result.texCoord--;
                }
                else
                {
                    result.position = 0;
                    result.normal = 0;
                    result.texCoord = 0;
                }

                return result;
            }

            public int position;
            public int normal;
            public int texCoord;
        }
    }
}
