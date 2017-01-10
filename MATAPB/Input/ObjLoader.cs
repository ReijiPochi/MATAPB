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
                            indices.Add(new Indices(line[1]));
                            indices.Add(new Indices(line[2]));
                            indices.Add(new Indices(line[3]));
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
            public Indices(string indices)
            {
                string[] index = indices.Split('/');

                if (int.TryParse(index[0], out position) && int.TryParse(index[1], out texCoord) && int.TryParse(index[2], out normal))
                {
                    position--;
                    normal--;
                    texCoord--;
                }
                else
                {
                    position = 0;
                    normal = 0;
                    texCoord = 0;
                }
            }

            public int position;
            public int normal;
            public int texCoord;
        }
    }
}
