using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            MatVector3 centor;

            MatVector3 proposalPoint = new MatVector3()
            {
                X = inputData.currentPosition.X + inputData.speedVector.X,
                Z = inputData.currentPosition.Z + inputData.speedVector.Z,
                Y = inputData.currentPosition.Y
            };

            MapJudgment2_5DResult result = new MapJudgment2_5DResult()
            {
                mapOK = false,
                resultVector = inputData.speedVector
            };

            for (int i = 0; i < pointCount; i += 3)
            {
                int i1 = Map.indices[i], i2 = Map.indices[i + 1], i3 = Map.indices[i + 2];
                bool cross = false;

                if (Map.vertices[i1].position.Y > proposalPoint.Y - 0.5) continue;

                double cX = (Map.vertices[i1].position.X + Map.vertices[i2].position.X + Map.vertices[i3].position.X) / 3.0;
                double cZ = (Map.vertices[i1].position.Z + Map.vertices[i2].position.Z + Map.vertices[i3].position.Z) / 3.0;

                centor = new MatVector3(cX, 0.0f, cZ);

                MatVector3 p1 = MatVector3Float.ToMatVector3(Map.vertices[i1].position);
                MatVector3 p2 = MatVector3Float.ToMatVector3(Map.vertices[i2].position);
                MatVector3 p3 = MatVector3Float.ToMatVector3(Map.vertices[i3].position);

                l1.a = p1;
                l1.b = p2;
                l2.a = proposalPoint;
                l2.b = centor;
                if (HitTestLine(l1, l2))
                    cross = true;

                if (!result.mapOK)
                {
                    l2.b = inputData.currentPosition;
                    if (HitTestLine(l1, l2))
                        result.resultVector = CalcResultVector(l1, l2);
                }


                l1.a = p1;
                l1.b = p3;
                l2.a = proposalPoint;
                l2.b = centor;
                if (HitTestLine(l1, l2))
                    cross = true;

                if (!result.mapOK)
                {
                    l2.b = inputData.currentPosition;
                    if (HitTestLine(l1, l2))
                        result.resultVector = CalcResultVector(l1, l2);
                }


                l1.a = p2;
                l1.b = p3;
                l2.a = proposalPoint;
                l2.b = centor;
                if (HitTestLine(l1, l2))
                    cross = true;

                if (!result.mapOK)
                {
                    l2.b = inputData.currentPosition;
                    if (HitTestLine(l1, l2))
                        result.resultVector = CalcResultVector(l1, l2);
                }


                if (!cross)
                {
                    result.mapOK = true;

                    result.resultVector = inputData.speedVector;

                    if (Map.vertices[i1].position.Y > result.floorHeight)
                        result.floorHeight = Map.vertices[i1].position.Y;
                }
            }

            return result;
        }

        private static bool HitTestLine(Line line1, Line line2)
        {
            double a = (line2.aX - line2.bX) * (line1.aZ - line2.aZ) + (line2.aZ - line2.bZ) * (line2.aX - line1.aX);
            double b = (line2.aX - line2.bX) * (line1.bZ - line2.aZ) + (line2.aZ - line2.bZ) * (line2.aX - line1.bX);
            double c = (line1.aX - line1.bX) * (line2.aZ - line1.aZ) + (line1.aZ - line1.bZ) * (line1.aX - line2.aX);
            double d = (line1.aX - line1.bX) * (line2.bZ - line1.aZ) + (line1.aZ - line1.bZ) * (line1.aX - line2.bX);

            return c * d < 0.0 && a * b < 0.0;
        }

        private static MatVector3 CalcResultVector(Line wall, Line player)
        {
            MatVector2 wallVector = new MatVector2(wall.bX - wall.aX, wall.bZ - wall.aZ);
            wallVector = MatVector2.Normalize(wallVector);

            MatVector2 playerVector = new MatVector2(player.bX - player.aX, player.bZ - player.aZ);

            double dot = -MatVector2.Dot(wallVector, playerVector);

            if (dot > 0.0 && dot < 0.01) dot = 0.01;
            else if (dot < 0.0 && dot > -0.01) dot = -0.01;

            return new MatVector3(dot * wallVector.X, 0, dot * wallVector.Y);
        }
    }
}
