using System;
using System.Collections.Generic;
using System.Drawing;

namespace MrCrusher.Framework.Core
{
    public class Calculator
    {
        public static string ToFuzzyByteString(long bytes) {
            double s = bytes;
            string[] format = new string[]
                  {
                      "{0} bytes", "{0} kB",  
                      "{0} MB", "{0} GB", "{0} TB", "{0} PB", "{0} EB"
                  };

            int i = 0;

            while (i < format.Length && s >= 1024) {
                s = (long)(100 * s / 1024) / 100.0;
                i++;
            }
            return string.Format(format[i], s);
        }

        private static Point CalculateOffsetPoint(double distance, Degree degrees) {
            if (distance < 0) {
                throw new IndexOutOfRangeException("distance");
            }

            var offset = new Point();
            var angle = Math.PI * degrees / 180.0;
            offset.X = Convert.ToInt32(distance * Math.Cos(angle));
            offset.Y -= Convert.ToInt32(distance * Math.Sin(angle));
            return offset;
        }

        public static Point CalculateDestinationPoint(Point startPoint, double distance, Degree degrees)
        {
            if (distance < 0) {
                throw new IndexOutOfRangeException("distance");
            }

            var destinationPoint = CalculateOffsetPoint(distance, degrees);
            destinationPoint.Offset(startPoint);

            return destinationPoint;
        }

        public static double CalculateDistance(Point startPoint, Point endPoint){
            int a = endPoint.X - startPoint.X;
            int b = endPoint.Y - startPoint.Y;
            return Math.Sqrt(a * a + b * b);
        }

        public static Degree CalculateDegree(Point startPoint, Point endPoint) {
            double a = endPoint.X - startPoint.X;
            double b = endPoint.Y - startPoint.Y;
            double c = CalculateDistance(startPoint, endPoint);

            // Für c == 0 ist Asin nicht definiert
            if(c == 0) {
                return 0;
            }

            // Das Koordinatensystem ist leider an der X-Achse gespiegelt
            if(b <= 0) {
                return Math.Acos(a / c) * 180 / Math.PI;
            }
            
            return 360 - (Math.Acos(a / c) * 180 / Math.PI);
        }

        public static Degree GetNewDegreeForStepwiseRotation(Degree currentDegree, Degree targertDegree, Degree degreeStep) {
            
            double degreeDiff = CalculateDegreeDifferenceBetweenToDegrees(currentDegree, targertDegree);

            if (Math.Abs(degreeDiff) < degreeStep) {
                return currentDegree + degreeDiff;
            } else {
                return currentDegree + (degreeDiff > 0 ? new Degree(degreeStep) : new Degree(-1 * degreeStep));
            }
        }

        public static double CalculateDegreeDifferenceBetweenToDegrees(Degree currentDegree, Degree targertDegree) {
            Degree resultTemp = targertDegree - currentDegree;
            resultTemp        = resultTemp + 180;
            resultTemp        = resultTemp + 360;
            resultTemp        = resultTemp % 360;
            double result     = resultTemp.Value - 180;

            if (result > 180) {
                throw new ArgumentOutOfRangeException(String.Format("Die minimale Graddifferenz zwischen zwei Winkeln darf nie mehr als 180° betragen. Sie beträgt aber {0}°.", result));
            }

            return result;
        }


        public static Queue<Point> CalculateRoute(Point startPoint, double maxDistance, Degree degrees)
        {
            if(maxDistance < 0) {
                throw new IndexOutOfRangeException("maxDistance");
            }

            var queueOfPoints = new Queue<Point>();
            var angle = Math.PI * degrees / 180.0;
            var tempPoint = startPoint;

            for (int i = 1; i <= Math.Round(maxDistance, MidpointRounding.AwayFromZero); i++) {
                var newPoint = new Point();
                newPoint.X = Convert.ToInt32(i * Math.Cos(angle));
                newPoint.Y -= Convert.ToInt32(i * Math.Sin(angle));

                newPoint.Offset(startPoint);
                
                // Add only new waypoints
                if(newPoint.Equals(tempPoint)) {
                    //maxDistance++;
                    continue;
                }
                    
                tempPoint = newPoint;
                queueOfPoints.Enqueue(newPoint);
            }

            return queueOfPoints;
        }
        
        public static Queue<Point> CalculateRoute(Point startPoint, Point endPoint)
        {
            var distance = CalculateDistance(startPoint, endPoint); // Hypothenuse
            var degrees = CalculateDegree(startPoint, endPoint); // Alpha
            
            return CalculateRoute(startPoint, distance, degrees);
        }
        
        public static double GetAccelratedSpeed(double currentSpeed, double accelration, double maxSpeed)
        {
            // Starthilfe
            if (accelration > 1 && currentSpeed == 0) {
                currentSpeed = 0.3;
            }

            var tempSpeed = currentSpeed * accelration;
            return tempSpeed > maxSpeed ? maxSpeed : tempSpeed;
        }
    }
}
