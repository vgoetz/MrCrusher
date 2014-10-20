using System;
using System.Collections.Generic;
using System.Drawing;

namespace MrCrusher.Test
{
    class TestHelper
    {
        public static void WriteRoute(Queue<Point> route)
        {
            Console.Write("Route: \n");
            foreach (var point in route) {
                Console.WriteLine("x={0},y={1} ", point.X, point.Y);
            }

            if (route.Count == 0)
                Console.WriteLine("---");
        }

        public static void WriteRoutes(Queue<Point> expectedRoute, Queue<Point> actualRoute)
        {
            Console.Write("Expected ");
            WriteRoute(expectedRoute);

            Console.WriteLine();
            Console.Write("Actual ");
            WriteRoute(expectedRoute);
        }
    }
}
