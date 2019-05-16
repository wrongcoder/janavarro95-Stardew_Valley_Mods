using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Objects;
using StardewValley;

namespace Revitalize.Framework.Utilities.Serialization
{
    public class SpriteBatchUtilities
    {

        public static void Draw(SpriteBatch spriteBatch, CustomObject obj, StardewValley.Item itemToDraw,float alpha,float addedDepth)
        {
            if (itemToDraw.GetType()==typeof(StardewValley.Object))
            {
                Rectangle rectangle;
                SpriteBatch spriteBatch1 = spriteBatch;
                Texture2D shadowTexture = Game1.shadowTexture;
                Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(obj.TileLocation.X), (float)(obj.TileLocation.Y)));
                Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
                Color color = Color.White * alpha;
                rectangle = Game1.shadowTexture.Bounds;
                double x1 = (double)rectangle.Center.X;
                rectangle = Game1.shadowTexture.Bounds;
                double y1 = (double)rectangle.Center.Y;
                Vector2 origin = new Vector2((float)x1, (float)y1);
                double num = (double)obj.boundingBox.Bottom / 10000.0;
                spriteBatch1.Draw(shadowTexture, position, sourceRectangle, color, 0.0f, origin, 4f, SpriteEffects.None, (float)num);
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(obj.boundingBox.Center.X - 32), (float)(obj.boundingBox.Center.Y))), obj.animationManager.currentAnimation.sourceRectangle, Color.White * alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(obj.boundingBox.Bottom + 1) / 10000f)+ addedDepth);
            }
            if (ModCore.Serializer.IsSameOrSubclass(typeof(CustomObject),itemToDraw.GetType()))
            {
                (itemToDraw as CustomObject).draw(spriteBatch,(int)obj.TileLocation.X, (int)obj.TileLocation.Y);
            }
        }

    }
}
