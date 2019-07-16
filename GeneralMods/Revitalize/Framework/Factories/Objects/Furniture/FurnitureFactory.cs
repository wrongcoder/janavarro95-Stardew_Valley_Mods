using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Factories.Objects.Furniture;
using Revitalize.Framework.Graphics;
using Revitalize.Framework.Graphics.Animations;
using Revitalize.Framework.Illuminate;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Objects.Furniture;
using Revitalize.Framework.Objects.InformationFiles.Furniture;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Factories.Objects
{
    //TODO: Add Rugs
    //Add Benches
    //Add dressers for storage/appearance change (create this)
    //Create portable beds???
    public class FurnitureFactory
    {
        public static string ChairFolder = Path.Combine("Data", "Furniture", "Chairs");
        public static string TablesFolder = Path.Combine("Data", "Furniture", "Tables");
        public static string LampsFolder = Path.Combine("Data", "Furniture", "Lamps");

        public static void LoadFurnitureFiles()
        {
            LoadChairFiles();
            LoadTableFiles();
            LoadLampFiles();
        }

        private static void LoadChairFiles()
        {
            SerializeChairs();
            DeserializeChairs();
        }

        private static void LoadTableFiles()
        {
            SerializeTableFiles();
            DeserializeTableFiles();
        }

        private static void LoadLampFiles()
        {
            SerializeLamps();
            DeserializeLamps();
        }
        /// <summary>
        /// Serialize a lamp to a .json file for easier creation of like objects.
        /// </summary>
        private static void SerializeLamps()
        {
            LampTileComponent lampTop = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Lamps.OakLamp", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), typeof(LampTileComponent), Color.White), new BasicItemInformation("Oak Lamp", "Omegasis.Revitalize.Furniture.Lamps.OakLamp", "A basic wooden light.", "Lamps", Color.Brown, -300, 0, true, 100, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new Animation(new Rectangle(0, 0, 16, 16))), Color.White,true, new InventoryManager(), new LightManager()));

            LampTileComponent lampMiddle = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Lamps.OakLamp", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), typeof(LampTileComponent), Color.White), new BasicItemInformation("Oak Lamp", "Omegasis.Revitalize.Furniture.Lamps.OakLamp", "A basic wooden light.", "Lamps", Color.Brown, -300, 0, true, 100, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new Animation(new Rectangle(0, 16, 16, 16))), Color.White, true, new InventoryManager(), new LightManager()));
            LampTileComponent lampBottom = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Lamps.OakLamp", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), typeof(LampTileComponent), Color.White), new BasicItemInformation("Oak Lamp", "Omegasis.Revitalize.Furniture.Lamps.OakLamp", "A basic wooden light.", "Lamps", Color.Brown, -300, 0, true, 100, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new Animation(new Rectangle(0, 32, 16, 16))), Color.White, false, new InventoryManager(), new LightManager()));

            lampMiddle.lights.addLight(new Vector2(Game1.tileSize), new LightSource(4, new Vector2(0, 0), 2.5f, Color.Orange.Invert()), lampMiddle);

            LampMultiTiledObject lamp = new LampMultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Lamps.OakLamp", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), typeof(LampMultiTiledObject), Color.White), new BasicItemInformation("Oak Lamp", "Omegasis.Revitalize.Furniture.Lamps.OakLamp", "A basic wooden light", "Lamps", Color.Brown, -300, 0, true, 300, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Lamp"), new AnimationManager(), Color.White, false, new InventoryManager(), new LightManager()));

            lamp.addComponent(new Vector2(0, -2), lampTop);
            lamp.addComponent(new Vector2(0, -1), lampMiddle);
            lamp.addComponent(new Vector2(0, 0), lampBottom);

            FactoryInfo lT = new FactoryInfo(lampTop);
            FactoryInfo lM = new FactoryInfo(lampMiddle);
            FactoryInfo lB = new FactoryInfo(lampBottom);

            FactoryInfo lO = new FactoryInfo(lamp);

            ModCore.Serializer.SerializeContentFile("OakLamp_0_-2", lT,Path.Combine(LampsFolder,"OakLamp"));
            ModCore.Serializer.SerializeContentFile("OakLamp_0_-1", lM, Path.Combine(LampsFolder, "OakLamp"));
            ModCore.Serializer.SerializeContentFile("OakLamp_0_0", lB, Path.Combine(LampsFolder, "OakLamp"));
            ModCore.Serializer.SerializeContentFile("OakLamp", lO, Path.Combine(LampsFolder, "OakLamp"));

            //ModCore.customObjects.Add(lamp.info.id, lamp);
        }

        private static void DeserializeLamps()
        {
            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", LampsFolder))) Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", LampsFolder));
            string[] directories = Directory.GetDirectories(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", LampsFolder));

            foreach(string directory in directories)
            {
                string[] files = Directory.GetFiles(directory);

                Dictionary<string, LampMultiTiledObject> objs = new Dictionary<string, LampMultiTiledObject>();

                //Deserialize container.
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == true) continue;
                    else
                    {
                        FactoryInfo factoryInfo = ModCore.Serializer.DeserializeContentFile<FactoryInfo>(file);
                        objs.Add(Path.GetFileNameWithoutExtension(file), new LampMultiTiledObject(factoryInfo.PyTkData,factoryInfo.info));
                    }
                }
                //Deseralize components
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == false) continue;
                    else
                    {

                        string[] splits = Path.GetFileNameWithoutExtension(file).Split('_');
                        string name = splits[0];
                        Vector2 offset = new Vector2(Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
                        FactoryInfo info = ModCore.Serializer.DeserializeContentFile<FactoryInfo>(file);

                        LampTileComponent lampPiece = new LampTileComponent(info.PyTkData,info.info);
                        //Recreate the lights info.
                        if (lampPiece.lights != null)
                        {
                            //ModCore.log("Info for file"+Path.GetFileNameWithoutExtension(file)+" has this many lights: " + info.info.lightManager.fakeLights.Count);
                            lampPiece.lights.lights.Clear();
                            foreach (KeyValuePair<Vector2, FakeLightSource> light in info.info.lightManager.fakeLights)
                            {
                                lampPiece.lights.addLight(new Vector2(Game1.tileSize), new LightSource(light.Value.id, new Vector2(0, 0), light.Value.radius, light.Value.color.Invert()), lampPiece);
                            }
                        }


                        objs[name].addComponent(offset, lampPiece);
                    }
                }
                foreach (var v in objs)
                {
                    ModCore.customObjects.Add(v.Value.info.id, v.Value);
                }
            }


        }

        /// <summary>
        /// Serialize all chair basic information to a file to have as a reference for making other like objects.
        /// </summary>
        private static void SerializeChairs()
        {
            Framework.Objects.Furniture.ChairTileComponent chairTop = new ChairTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Chairs.OakChair", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Chair"), typeof(ChairTileComponent), Color.White), new BasicItemInformation("Oak Chair", "Omegasis.Revitalize.Furniture.Chairs.OakChair", "A basic wooden chair made out of oak.", "Chairs", Color.Brown, -300, 0, false, 250, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Chair"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest,"Furniture","Oak Chair"), new Animation(new Rectangle(0, 0, 16, 16)), new Dictionary<string, List<Animation>>() {
                { "Default_" + (int)Framework.Enums.Direction.Down , new List<Animation>()
                    {
                        new Animation(new Rectangle(0,0,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Down , new List<Animation>()
                    {
                        new Animation(new Rectangle(0,0,16,16))
                    }
                },
                { "Default_" + (int)Framework.Enums.Direction.Right , new List<Animation>()
                    {
                        new Animation(new Rectangle(16,0,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Right , new List<Animation>()
                    {
                        new Animation(new Rectangle(16,0,16,16))
                    }
                },
                { "Default_" + (int)Framework.Enums.Direction.Up , new List<Animation>()
                    {
                        new Animation(new Rectangle(32,0,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Up , new List<Animation>()
                    {
                        new Animation(new Rectangle(32,32,16,32))
                    }
                },
                { "Default_" + (int)Framework.Enums.Direction.Left , new List<Animation>()
                    {
                        new Animation(new Rectangle(48,0,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Left , new List<Animation>()
                    {
                        new Animation(new Rectangle(48,0,16,16))
                    }
                }
            }, "Default_" + (int)Framework.Enums.Direction.Down), Color.White, true, null, null),new ChairInformation(false));


            Framework.Objects.Furniture.ChairTileComponent chairBottom = new ChairTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Chairs.OakChair", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Chair"), typeof(ChairTileComponent), Color.White), new BasicItemInformation("Oak Chair", "Omegasis.Revitalize.Furniture.Chairs.OakChair", "A basic wooden chair.", "Chairs", Color.Brown, -300, 0, false, 250, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Chair"), new AnimationManager(TextureManager.TextureManagers[ModCore.Manifest.UniqueID]["Furniture"].getTexture("Oak Chair"), new Animation(new Rectangle(0, 16, 16, 16)), new Dictionary<string, List<Animation>>() {
                { "Default_" + (int)Framework.Enums.Direction.Down , new List<Animation>()
                    {
                        new Animation(new Rectangle(0,16,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Down , new List<Animation>()
                    {
                        new Animation(new Rectangle(0,16,16,16))
                    }
                },
                { "Default_" + (int)Framework.Enums.Direction.Right , new List<Animation>()
                    {
                        new Animation(new Rectangle(16,16,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Right , new List<Animation>()
                    {
                        new Animation(new Rectangle(16,16,16,16))
                    }
                },
                { "Default_" + (int)Framework.Enums.Direction.Up , new List<Animation>()
                    {
                        new Animation(new Rectangle(32,16,16,16))
                    }
                },
                { "Sitting_" + (int)Framework.Enums.Direction.Up , new List<Animation>()
                    {
                        new Animation(new Rectangle(48,32,16,32))
                    }
                },
                { "Default_" + (int)Framework.Enums.Direction.Left , new List<Animation>()
                    {
                        new Animation(new Rectangle(48,16,16,16))
                    }
                },
                { "Sitting" + (int)Framework.Enums.Direction.Left , new List<Animation>()
                    {
                        new Animation(new Rectangle(48,16,16,16))
                    }
                }
            }, "Default_" + (int)Framework.Enums.Direction.Down), Color.White, false, null, null), new ChairInformation(true));

            Framework.Objects.Furniture.ChairMultiTiledObject oakChair = new ChairMultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Chairs.OakChair", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Chair"), typeof(ChairMultiTiledObject), Color.White), new BasicItemInformation("Oak Chair", "Omegasis.Revitalize.Furniture.Chairs.OakChair", "A basic wooden chair.", "Chairs", Color.White, -300, 0, false, 250, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Chair"), new AnimationManager(), Color.White, false, null, null));


            ChairFactoryInfo top = new ChairFactoryInfo(chairTop);
            ChairFactoryInfo bottom = new ChairFactoryInfo(chairBottom);
            ChairFactoryInfo obj = new ChairFactoryInfo(oakChair);


            ModCore.Serializer.SerializeContentFile("OakChair_0_-1", top, Path.Combine(ChairFolder, "OakChair"));
            ModCore.Serializer.SerializeContentFile("OakChair_0_0", bottom, Path.Combine(ChairFolder, "OakChair"));
            ModCore.Serializer.SerializeContentFile("OakChair", obj, Path.Combine(ChairFolder, "OakChair"));
        }
        private static void DeserializeChairs()
        {
            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", ChairFolder))) Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", ChairFolder));
            string[] directories = Directory.GetDirectories(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", ChairFolder));

            foreach (string directory in directories)
            {
                string[] files = Directory.GetFiles(directory);

                Dictionary<string, ChairMultiTiledObject> chairObjects = new Dictionary<string, ChairMultiTiledObject>();

                //Deserialize container.
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == true) continue;
                    else
                    {
                        ChairFactoryInfo factoryInfo = ModCore.Serializer.DeserializeContentFile<ChairFactoryInfo>(file);
                        chairObjects.Add(Path.GetFileNameWithoutExtension(file), new ChairMultiTiledObject(factoryInfo.PyTkData,factoryInfo.info));
                    }
                }
                //Deseralize components
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == false) continue;
                    else
                    {

                        string[] splits = Path.GetFileNameWithoutExtension(file).Split('_');
                        string name = splits[0];
                        Vector2 offset = new Vector2(Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
                        ChairFactoryInfo info = ModCore.Serializer.DeserializeContentFile<ChairFactoryInfo>(file);
                        chairObjects[name].addComponent(offset, new ChairTileComponent(info.PyTkData,info.info, info.chairInfo));
                    }
                }
                foreach (var v in chairObjects)
                {
                    ModCore.customObjects.Add(v.Value.info.id, v.Value);
                }
            }
        }

        private static void SerializeTableFiles()
        {
            TableTileComponent upperLeft = new TableTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableTileComponent), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple wooden table to place objects on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Table"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), new TableInformation(true));
            TableTileComponent upperRight = new TableTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableTileComponent), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple wooden table to place objects on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Table"), new Animation(16, 0, 16, 16)), Color.White, true, null, null), new TableInformation(true));
            TableTileComponent centerLeft = new TableTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableTileComponent), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple wooden table to place objects on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Table"), new Animation(0, 16, 16, 16)), Color.White, false, null, null), new TableInformation(true));
            TableTileComponent centerRight = new TableTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableTileComponent), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple wooden table to place objects on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Table"), new Animation(16, 16, 16, 16)), Color.White, false, null, null), new TableInformation(true));
            TableTileComponent bottomLeft = new TableTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableTileComponent), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple wooden table to place objects on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Table"), new Animation(0, 32, 16, 16)), Color.White, false, null, null), new TableInformation(true));
            TableTileComponent bottomRight = new TableTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableTileComponent), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple wooden table to place objects on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Oak Table"), new Animation(16, 32, 16, 16)), Color.White, false, null, null), new TableInformation(true));

            TableMultiTiledObject obj = new TableMultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Tables.OakTable", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), typeof(TableMultiTiledObject), Color.White), new BasicItemInformation("Oak Table", "Omegasis.Revitalize.Furniture.Tables.OakTable", "A simple oak table to place things on.", "Tables", Color.Brown, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Oak Table"), new AnimationManager(), Color.White, false, null, null));

            TableFactoryInfo uL = new TableFactoryInfo(upperLeft);
            TableFactoryInfo uR = new TableFactoryInfo(upperRight);
            TableFactoryInfo cL = new TableFactoryInfo(centerLeft);
            TableFactoryInfo cR = new TableFactoryInfo(centerRight);
            TableFactoryInfo bL = new TableFactoryInfo(bottomLeft);
            TableFactoryInfo bR = new TableFactoryInfo(bottomRight);

            TableFactoryInfo table = new TableFactoryInfo(obj);


            ModCore.Serializer.SerializeContentFile("OakTable_0_0", uL, Path.Combine(TablesFolder, "OakTable"));
            ModCore.Serializer.SerializeContentFile("OakTable_1_0", uR, Path.Combine(TablesFolder, "OakTable"));
            ModCore.Serializer.SerializeContentFile("OakTable_0_1", cL, Path.Combine(TablesFolder, "OakTable"));
            ModCore.Serializer.SerializeContentFile("OakTable_1_1", cR, Path.Combine(TablesFolder, "OakTable"));
            ModCore.Serializer.SerializeContentFile("OakTable_0_2", bL, Path.Combine(TablesFolder, "OakTable"));
            ModCore.Serializer.SerializeContentFile("OakTable_1_2", bR, Path.Combine(TablesFolder, "OakTable"));

            ModCore.Serializer.SerializeContentFile("OakTable", table, Path.Combine(TablesFolder, "OakTable"));

        }

        private static void DeserializeTableFiles()
        {
            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", TablesFolder))) Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", TablesFolder));

            string[] directories = Directory.GetDirectories(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", TablesFolder));
            foreach (string directory in directories)
            {

                string[] files = Directory.GetFiles(directory);

                Dictionary<string, TableMultiTiledObject> chairObjects = new Dictionary<string, TableMultiTiledObject>();

                //Deserialize container.
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == true) continue;
                    else
                    {
                        TableFactoryInfo factoryInfo = ModCore.Serializer.DeserializeContentFile<TableFactoryInfo>(file);
                        chairObjects.Add(Path.GetFileNameWithoutExtension(file), new TableMultiTiledObject(factoryInfo.PyTkData,factoryInfo.info));
                    }
                }
                //Deseralize components
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == false) continue;
                    else
                    {

                        string[] splits = Path.GetFileNameWithoutExtension(file).Split('_');
                        string name = splits[0];
                        Vector2 offset = new Vector2(Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
                        TableFactoryInfo info = ModCore.Serializer.DeserializeContentFile<TableFactoryInfo>(file);
                        chairObjects[name].addComponent(offset, new TableTileComponent(info.PyTkData,info.info, info.tableInfo));
                    }
                }
                foreach (var v in chairObjects)
                {
                    ModCore.customObjects.Add(v.Value.info.id, v.Value);
                }
            }
        }


        public static ChairMultiTiledObject GetChair(string name)
        {
            return (ChairMultiTiledObject)ModCore.GetObjectFromPool(name);
        }
        public static TableMultiTiledObject GetTable(string name)
        {
            return (TableMultiTiledObject)ModCore.GetObjectFromPool(name);
        }
        public static LampMultiTiledObject GetLamp(string name)
        {
            return (LampMultiTiledObject)ModCore.GetObjectFromPool(name);
        }



    }
}
