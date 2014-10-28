using System;
using System.Collections.Generic;
using System.Drawing;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.Drawable;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.BaseObjects.Interfaces {

    public interface IGameObject : IDrawableObject, ICanControlledByHumanPlayer {

        Guid Guid { get; }

        Point PositionCenter { get; set; }

        Rectangle RectangleForCollisionDetection { get; set; }

        IAiMission CurrentMission { get; set; }

        int Health { get; }
        int MaxHealth { get; }
        bool Visible { get; }
        bool Vulnerable { get; set; }
        bool CanInteractWithOthers { get; }
        
        bool Dead { get; }
        DateTime? TimeOfDeath { get; }
        void Die(IGameObject killer);
        void MarkAsToBeDeleted();
        bool ShouldBeDeleted { get; }

        string GetVideoName();
        void SetSurface(string surfaceFileName);
        Surface GetCurrentSurface();
        IEnumerable<Sprite> GetCurrentSprites();
        bool IsInteractingWith(IGameObject obj);
    }

}