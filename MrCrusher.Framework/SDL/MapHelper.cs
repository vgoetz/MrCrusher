using System;
using System.Collections.Generic;
using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.SDL
{
    public static class MapHelper {
        private const int MaxTryNumberForRecursiveCalls = 3;


        public static bool IsWithinBounds(this Surface surface, Point point) {
            if (surface.Width == 0 || surface.Height == 0) {
                return false;
            }

            if (point.X < 0 || point.Y < 0) {
                return false;
            }

            if (point.X-1 > surface.Width || point.Y-1 > surface.Height) {
                return false;
            }

            return true;
        }

        public static bool IsWithinBounds(this Surface surface, Point point, int widthOfObject, int heightOfObject) {

            bool isWithinBoundsTopLeftOfObject = IsWithinBounds(surface, new Point(point.X - widthOfObject / 2, point.Y - heightOfObject / 2));
            bool isWithinBoundsBottomRightOfObject = IsWithinBounds(surface, new Point(point.X + widthOfObject / 2, point.Y + heightOfObject / 2));

            return isWithinBoundsTopLeftOfObject && isWithinBoundsBottomRightOfObject;
        }

        public static IEnumerable<IGameObject> GetGameObjectsInNearAreaOfPoint(Point point, int maxDistance) {
            foreach (var gameObject in GameEnv.GameObjectsToDrawAndMove) {
                var distanceToPoint = Calculator.CalculateDistance(point, gameObject.PositionCenter);

                if (distanceToPoint <= maxDistance) {
                    yield return gameObject;
                }
            }
        }

        public static ICanBeEntered GetNearestGameObject(IEnumerable<ICanBeEntered> objects, Point dependsOnPoint) {
            var distanceDictionary = new Dictionary<int, ICanBeEntered>();
            int minValue = 0;

            foreach (var gameObject in objects) {
                var distance = (int) Calculator.CalculateDistance(dependsOnPoint, gameObject.PositionCenter);

                if (distanceDictionary.ContainsKey(distance) == false) {
                    distanceDictionary.Add(distance, gameObject);
                    if (minValue == 0 || distance < minValue) {
                        minValue = distance;
                    }
                }
            }

            return distanceDictionary[minValue];
        }


        public static Point? GetRandomColisionFreePointWithinRectangle(Size collisionSize, Rectangle limationRectangleInMap, Random randomizer, int tryNumber = 0) {

            if (tryNumber > MaxTryNumberForRecursiveCalls) {
                return null;
            }
            Point newPosition = GetRandomPointWithinRectangle(limationRectangleInMap, randomizer);

            bool isInterseptingWithOtherObject = CollisionDetection.CollisionDetectionForRectangle_ForAddingNewObjects(newPosition, collisionSize);

            if (isInterseptingWithOtherObject == false) {
                return newPosition;
            } else {
                return GetRandomColisionFreePointWithinRectangle(collisionSize, limationRectangleInMap, randomizer, tryNumber + 1);
            }
        }

        private static Point GetRandomPointWithinRectangle(Rectangle limationRectangle, Random randomizer) {

            return new Point(randomizer.Next(limationRectangle.X, limationRectangle.X + limationRectangle.Width),
                randomizer.Next(limationRectangle.Y, limationRectangle.Y + limationRectangle.Height));
        }
    }
}
