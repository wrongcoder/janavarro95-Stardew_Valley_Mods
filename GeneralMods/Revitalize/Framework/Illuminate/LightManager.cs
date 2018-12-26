using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Illuminate
{
    public class LightManager
    {
        public Dictionary<Vector2,LightSource> lights;
        public bool lightsOn;

        public LightManager()
        {
            this.lights = new Dictionary<Vector2, LightSource>();
            lightsOn = false;
        }

        /// <summary>
        /// Add a light to the list of tracked lights.
        /// </summary>
        /// <param name="IdKey"></param>
        /// <param name="light"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool addLight(Vector2 IdKey,LightSource light,StardewValley.Object gameObject)
        {
            Vector2 initialPosition = gameObject.TileLocation*Game1.tileSize;
            initialPosition += IdKey;

            if (this.lights.ContainsKey(IdKey))
            {
                return false;
            }
            else
            {
                light.position.Value = initialPosition;
                this.lights.Add(IdKey, light);
                return true;
            }
        }

        /// <summary>
        /// Turn off a single light.
        /// </summary>
        /// <param name="IdKey"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool turnOffLight(Vector2 IdKey, GameLocation location)
        {
            if (!lights.ContainsKey(IdKey))
            {
                return false;
            }
            else
            {
                LightSource light = new LightSource();
                this.lights.TryGetValue(IdKey, out light);
                Game1.currentLightSources.Remove(light);
                location.sharedLights.Remove(light);
                return true;
            }
        }

        /// <summary>
        /// Turn on a single light.
        /// </summary>
        /// <param name="IdKey"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool turnOnLight(Vector2 IdKey, GameLocation location,StardewValley.Object gameObject)
        {
            if (!lights.ContainsKey(IdKey))
            {
                return false;
            }
            else
            {
                LightSource light = new LightSource();
                this.lights.TryGetValue(IdKey, out light);
                if (light == null)
                {
                    throw new Exception("Light is null????");
                }
                Game1.currentLightSources.Add(light);
                if (location == null)
                {
                    throw new Exception("WHY IS LOC NULL???");
                }
                if (location.sharedLights == null)
                {
                    throw new Exception("Locational lights is null!");
                    
                }
                Game1.showRedMessage("TURN ON!");
                Game1.currentLightSources.Add(light);
                location.sharedLights.Add(light);
                repositionLight(light,IdKey,gameObject);
                return true;
            }
        }




        /// <summary>
        /// Add a light source to this location.
        /// </summary>
        /// <param name="environment">The game location to add the light source in.</param>
        /// <param name="c">The color of the light to be added</param>
        public virtual void turnOnLights(GameLocation environment,StardewValley.Object gameObject)
        {
            foreach(KeyValuePair<Vector2,LightSource> pair in this.lights)
            {
                turnOnLight(pair.Key, environment,gameObject);
            }
            repositionLights(gameObject);
        }

        /// <summary>
        /// Removes a lightsource from the game location.
        /// </summary>
        /// <param name="environment">The game location to remove the light source from.</param>
        public void turnOffLights(GameLocation environment)
        {
            foreach (KeyValuePair<Vector2, LightSource> pair in this.lights)
            {
                turnOffLight(pair.Key, environment);
            }

        }

        public void repositionLights(StardewValley.Object gameObject)
        {
            foreach (KeyValuePair<Vector2, LightSource> pair in this.lights)
            {
                repositionLight(pair.Value, pair.Key, gameObject);
            }
        }

        public void repositionLight(LightSource light,Vector2 offset,StardewValley.Object gameObject)
        {
            Vector2 initialPosition = gameObject.TileLocation * Game1.tileSize;
            light.position.Value = initialPosition + offset;
        }

        public virtual void toggleLights(GameLocation location,StardewValley.Object gameObject)
        {

            if (lightsOn == false)
            {
                this.turnOnLights(location,gameObject);
                lightsOn = true;
            }
            else if (lightsOn == true)
            {
                
                this.turnOffLights(Game1.player.currentLocation);
                lightsOn = false;
            }
        }

    }
}
