using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;

using MATAPB.Objects;

namespace MATAPB.Gaming
{
    public class MapJudgment2_5D
    {
        public static Object3D Map { get; set; }

        public static MapJudgment2_5DResult Judge(MapJudgment2_5DInput inputData)
        {
            int pointCount = Map.indices.Length;
            Line l1 = new Line(), l2 = new Line();
            Vector3 centor;

            Vector3 proposalPoint1 = new Vector3()
            {
                X = inputData.currentPosition.X + inputData.speedVector.X,
                Z = inputData.currentPosition.Z + inputData.speedVector.Z,
                Y = inputData.currentPosition.Y
            };

            MapJudgment2_5DResult result = new MapJudgment2_5DResult()
            {
                mapOK = false,
                result = inputData.currentPosition,
                floorHeight = 0
            };

            Vector3 resultSpeedVector = new Vector3(0);

            for (int i = 0; i < pointCount; i += 3)
            {
                int i1 = Map.indices[i], i2 = Map.indices[i + 1], i3 = Map.indices[i + 2];

                double height = CalcHeight(Map.vertices[i1].position, Map.vertices[i2].position, Map.vertices[i3].position, proposalPoint1);

                if (height > proposalPoint1.Y - 0.5) continue;

                double cX = (Map.vertices[i1].position.X + Map.vertices[i2].position.X + Map.vertices[i3].position.X) / 3.0;
                double cZ = (Map.vertices[i1].position.Z + Map.vertices[i2].position.Z + Map.vertices[i3].position.Z) / 3.0;

                centor = new Vector3((float)cX, 0.0f, (float)cZ);

                Vector3 p1 = Map.vertices[i1].position;
                Vector3 p2 = Map.vertices[i2].position;
                Vector3 p3 = Map.vertices[i3].position;

                l1.a = p1;
                l1.b = p2;
                l2.a = proposalPoint1;
                l2.b = centor;
                if (HitTestLine(l1, l2))
                {
                    if (!result.mapOK)
                    {
                        l2.b = inputData.currentPosition;
                        if (HitTestLine(l1, l2))
                            resultSpeedVector = CalcResultVector(l1, l2);
                    }

                    continue;
                }



                l1.a = p1;
                l1.b = p3;
                l2.a = proposalPoint1;
                l2.b = centor;
                if (HitTestLine(l1, l2))
                {
                    if (!result.mapOK)
                    {
                        l2.b = inputData.currentPosition;
                        if (HitTestLine(l1, l2))
                            resultSpeedVector = CalcResultVector(l1, l2);
                    }

                    continue;
                }


                l1.a = p2;
                l1.b = p3;
                l2.a = proposalPoint1;
                l2.b = centor;
                if (HitTestLine(l1, l2))
                {
                    if (!result.mapOK)
                    {
                        l2.b = inputData.currentPosition;
                        if (HitTestLine(l1, l2))
                            resultSpeedVector = CalcResultVector(l1, l2);
                    }

                    continue;
                }


                result.mapOK = true;

                result.result = proposalPoint1;

                if (height > result.floorHeight)
                    result.floorHeight = height;
            }

            if (result.mapOK)
            {
                return result;
            }
            else
            {
                Vector3 proposalPoint2 = new Vector3()
                {
                    X = inputData.currentPosition.X + resultSpeedVector.X,
                    Z = inputData.currentPosition.Z + resultSpeedVector.Z,
                    Y = result.result.Y
                };

                for (int i = 0; i < pointCount; i += 3)
                {
                    int i1 = Map.indices[i], i2 = Map.indices[i + 1], i3 = Map.indices[i + 2];

                    double height = CalcHeight(Map.vertices[i1].position, Map.vertices[i2].position, Map.vertices[i3].position, proposalPoint2);

                    if (height > proposalPoint2.Y - 0.5) continue;

                    double cX = (Map.vertices[i1].position.X + Map.vertices[i2].position.X + Map.vertices[i3].position.X) / 3.0;
                    double cZ = (Map.vertices[i1].position.Z + Map.vertices[i2].position.Z + Map.vertices[i3].position.Z) / 3.0;

                    centor = new Vector3((float)cX, 0.0f, (float)cZ);

                    Vector3 p1 = Map.vertices[i1].position;
                    Vector3 p2 = Map.vertices[i2].position;
                    Vector3 p3 = Map.vertices[i3].position;

                    l1.a = p1;
                    l1.b = p2;
                    l2.a = proposalPoint2;
                    l2.b = centor;
                    if (HitTestLine(l1, l2))
                        continue;


                    l1.a = p1;
                    l1.b = p3;
                    l2.a = proposalPoint2;
                    l2.b = centor;
                    if (HitTestLine(l1, l2))
                        continue;


                    l1.a = p2;
                    l1.b = p3;
                    l2.a = proposalPoint2;
                    l2.b = centor;
                    if (HitTestLine(l1, l2))
                        continue;


                    result.result = proposalPoint2;
                    return result;
                }

                return result;
            }
        }

        private static bool HitTestLine(Line line1, Line line2)
        {
            double a = (line2.aX - line2.bX) * (line1.aZ - line2.aZ) + (line2.aZ - line2.bZ) * (line2.aX - line1.aX);
            double b = (line2.aX - line2.bX) * (line1.bZ - line2.aZ) + (line2.aZ - line2.bZ) * (line2.aX - line1.bX);
            double c = (line1.aX - line1.bX) * (line2.aZ - line1.aZ) + (line1.aZ - line1.bZ) * (line1.aX - line2.aX);
            double d = (line1.aX - line1.bX) * (line2.bZ - line1.aZ) + (line1.aZ - line1.bZ) * (line1.aX - line2.bX);

            return c * d < 0.0 && a * b < 0.0;
        }

        private static double CalcHeight(Vector3 a, Vector3 b, Vector3 c, Vector3 point)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            Vector3 n = Vector3.Cross(ab, ac);

            return -(n.X * (point.X - a.X) - n.Z * (point.Z - a.Z)) / n.Y + a.Y;
        }

        private static Vector3 CalcResultVector(Line wall, Line player)
        {
            Vector2 wallVector = new Vector2(wall.bX - wall.aX, wall.bZ - wall.aZ);
            wallVector = Vector2.Normalize(wallVector);

            Vector2 playerVector = new Vector2(player.bX - player.aX, player.bZ - player.aZ);

            float dot = -Vector2.Dot(wallVector, playerVector);

            if (dot > 0.0f && dot < 0.01f) dot = 0.01f;
            else if (dot < 0.0f && dot > -0.01f) dot = -0.01f;

            return new Vector3(dot * wallVector.X, 0, dot * wallVector.Y);
        }
    }
}
