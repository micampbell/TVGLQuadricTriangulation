using System;
using System.Collections.Generic;
using System.IO;
using TVGL;
using WebGPUPresenter;
//using WindowsDesktopPresenter;

namespace TVGLUnitTestsAndBenchmarking
{

    internal class Program
    {
        public static string inputFolder = "TestFiles\\QuadricLayerData";

        [STAThread]
        private static void Main(string[] args)
        {
            var dirInfo = IO.BackoutToFolder(inputFolder);

            OutputServices.Presenter2D = new Presenter2D();
            OutputServices.Presenter3D = new Presenter3D();
            TestCase1(dirInfo);
            TestCase2(dirInfo);
            TestCase3(dirInfo);
            TestCase4(dirInfo);
            TestCase5(dirInfo);
        }

        private static void TestCase1(System.IO.DirectoryInfo dirInfo)
        {
            // ellipse
            var quadric = new GeneralQuadric(0.2, 0.8, 0.1, 0, 0, 0, 0, 0, 0, -5);

            IO.Open(dirInfo.FullName + "\\ellipseLayerSphereModel.json",
                out Polygon3D loop1);
            TestEigen1(loop1);
            (var xmin, var xmax, var ymin, var ymax, var zmin, var zmax) = GetBoundingBoxForLoop(loop1);
            Presenter.ShowAndHang(loop1.Vertices, solids: quadric.Tessellate(xmin, xmax,
                 ymin, ymax, zmin, zmax, 0.25));
            var faceCenter = new Vector3(.3, .3, 3);
            var faceNormal = new Vector3(.40, -.80, 1).Normalize();
            var intersections = quadric.LineIntersection(faceCenter, faceNormal);

        }

        private static (double, double, double, double, double, double) GetBoundingBoxForLoop(Polygon3D loop)
        {
            var minX = double.PositiveInfinity;
            var minY = double.PositiveInfinity;
            var minZ = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var maxY = double.NegativeInfinity;
            var maxZ = double.NegativeInfinity;
            foreach (var pt in loop.Vertices)
            {
                if (pt.X < minX)
                    minX = pt.X;
                if (pt.Y < minY)
                    minY = pt.Y;
                if (pt.Z < minZ)
                    minZ = pt.Z;
                if (pt.X > maxX)
                    maxX = pt.X;
                if (pt.Y > maxY)
                    maxY = pt.Y;
                if (pt.Z > maxZ)
                    maxZ = pt.Z;
            }
            return (minX, maxX, minY, maxY, minZ, maxZ);
        }

        private static void TestCase2(DirectoryInfo dirInfo)
        {
            // ellipse
            var quadric = new GeneralQuadric(0.2, 0.8, 0.1, 0, 0, 0, 0, 0, 0, -5);
            IO.Open(dirInfo.FullName + "\\ellipseLayerFlatZModel.json",
                out Polygon3D loop1);
            Presenter.ShowAndHang(loop1.Vertices, solids: quadric.Tessellate(-10, 10, -10, 10, -10, 10, 0.5));
        }
        private static void TestCase3(DirectoryInfo dirInfo)
        {
            // ellipse
            var quadric = new GeneralQuadric(0.2, 0.8, 0.1, 0, 0, 0, 0, 0, 0, -5);
            IO.Open(dirInfo.FullName + "\\ellipseLayerFlatZModelTwoLoops1.json",
                out Polygon3D loop1);
            IO.Open(dirInfo.FullName + "\\ellipseLayerFlatZModelTwoLoops2.json",
                out Polygon3D loop2);
            Presenter.ShowAndHang([loop1.Vertices, loop2.Vertices], solids: quadric.Tessellate(-10, 10, -10, 10, -10, 10, 0.5));
        }
        private static void TestCase4(DirectoryInfo dirInfo)
        {
            // hyperboloid
            var quadric = new GeneralQuadric(-0.6, 1, 0.4, 0, 0, 0, 0, 0, 0, -5);

            IO.Open(dirInfo.FullName + "\\hyperboloidLayerSphereModel.json",
                out Polygon3D loop1);
            Presenter.ShowAndHang(loop1.Vertices, solids: quadric.Tessellate(-10, 10, -10, 10, -10, 10, 0.5));
        }
        private static void TestCase5(DirectoryInfo dirInfo)
        {
            // hyperboloid
            var quadric = new GeneralQuadric(-0.6, 1, 0.4, 0, 0, 0, 0, 0, 0, -5);
            IO.Open(dirInfo.FullName + "\\hyperboloidLayerThreeLoopModel1.json",
                out Polygon3D loop1);
            IO.Open(dirInfo.FullName + "\\hyperboloidLayerThreeLoopModel2.json",
                out Polygon3D loop2);
            IO.Open(dirInfo.FullName + "\\hyperboloidLayerThreeLoopModel3.json",
                out Polygon3D loop3);
            Presenter.ShowAndHang([loop1.Vertices, loop2.Vertices, loop3.Vertices], solids: quadric.Tessellate(-10, 10, -10, 10, -10, 10, 0.5));
        }



        private static void TestEigen1(Polygon3D loop1)
        {
            // directions of the ellipsoid
            var direction1 = Vector3.UnitY;   // new Vector3(0, 1, 0);
            var direction2 = Vector3.UnitZ;   // new Vector3(0, 0, 1);
            var points2D = new List<Vector2>();
            foreach (var point in loop1.Vertices)
            {
                double x = point.X;
                double y = point.Y;
                double z = point.Z;

                double newX = point.Dot(direction1);  // x * direction1.X
                                                      //+ y * direction1.Y
                                                      //+ z * direction1.Z;

                double newY = point.Dot(direction2);
                //x * direction2.X
                //            + y * direction2.Y
                //            + z * direction2.Z;
                points2D.Add(new Vector2(newX, newY));
                Console.WriteLine(newX + ", " + newY);
            }
            Presenter.ShowAndHang(points2D);
        }

    }
}




