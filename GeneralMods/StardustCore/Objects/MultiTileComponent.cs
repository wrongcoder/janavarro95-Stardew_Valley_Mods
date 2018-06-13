using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardustCore.Objects
{
    public class MultiTileComponent : CoreObject
    {
        public MultiTileObject containerObject;
        public CoreObject objectPart;

        public MultiTileComponent(CoreObject part)
        {
            this.objectPart = part;
            this.name = this.objectPart.name;
            this.defaultBoundingBox = new Rectangle(0, 0, 16, 16);
            this.boundingBox.Value = new Rectangle((int)0 * Game1.tileSize, (int)0* Game1.tileSize, 1 * Game1.tileSize, 1 * Game1.tileSize);
        }

        public override bool clicked(Farmer who)
        {
           containerObject.RemoveAllObjects();
            return true;
        }

        public override Item getOne()
        {
            var obj= new MultiTileComponent((CoreObject)objectPart.getOne());
            obj.containerObject = this.containerObject;
            return obj;
        }

        public override bool RightClicked(Farmer who)
        {
            return this.objectPart.RightClicked(who);
        }

        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);

            this.TileLocation = new Vector2((float)point.X, (float)point.Y);
            this.boundingBox.Value = new Rectangle((int)TileLocation.X * Game1.tileSize, (int)TileLocation.Y * Game1.tileSize, 1 * Game1.tileSize, 1 * Game1.tileSize);
            using (List<StardewValley.Farmer>.Enumerator enumerator3 = location.getFarmers().GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    if (enumerator3.Current.GetBoundingBox().Intersects(this.boundingBox.Value))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
            }
            this.updateDrawPosition();

            bool f = Utilities.placementAction(this, location, x, y,StardustCore.ModCore.SerializationManager ,who);
            this.thisLocation = Game1.player.currentLocation;
            return f;
            //  Game1.showRedMessage("Can only be placed in House");
            //  return false;
        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            // Game1.showRedMessage("Why3?");
            try
            {
                this.heldObject.Value.performRemoveAction(this.TileLocation, this.thisLocation);
                this.heldObject.Value = null;
            }
            catch(Exception err)
            {

            }
            StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(this);
            this.thisLocation.objects.Remove(this.TileLocation);
            this.thisLocation = null;
            this.locationsName = "";
            base.performRemoveAction(tileLocation, environment);
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (x == -1)
            {
                spriteBatch.Draw(this.objectPart.TextureSheet.getTexture(), Game1.GlobalToLocal(Game1.viewport, this.objectPart.drawPosition), new Rectangle?(this.objectPart.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.objectPart.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.objectPart.Decoration_type == 12) ? 0f : ((float)(this.objectPart.boundingBox.Bottom - 8) / 10000f));
            }
            else
            {
                //The actual planter box being drawn.
                if (animationManager == null)
                {
                    spriteBatch.Draw(this.objectPart.TextureSheet.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.objectPart.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.objectPart.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
                }

                else
                {
                    //Log.AsyncC("Animation Manager is working!");
                    this.animationManager.draw(spriteBatch, this.objectPart.animationManager.objectTexture.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.objectPart.animationManager.currentAnimation.sourceRectangle), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.objectPart.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    try
                    {
                        this.animationManager.tickAnimation();
                        // Log.AsyncC("Tick animation");
                    }
                    catch (Exception err)
                    {
                        ModCore.ModMonitor.Log(err.ToString());
                    }
                }

                // spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((double)tileLocation.X * (double)Game1.tileSize + (((double)tileLocation.X * 11.0 + (double)tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2), (float)((double)tileLocation.Y * (double)Game1.tileSize + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)((double)tileLocation.X * 51.0 + (double)tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(((double)tileLocation.Y * (double)Game1.tileSize + (double)(Game1.tileSize / 2) + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));



            }
        }




    }
}
