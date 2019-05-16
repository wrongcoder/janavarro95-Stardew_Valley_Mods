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

namespace Revitalize.Framework.Factories.Objects
{
    public class FurnitureFactory
    {
        public static string ChairFolder = Path.Combine("Objects", "Furniture", "Chairs");
        public static string TablesFolder = Path.Combine("Objects", "Furniture", "Tables");

        public static void LoadFurnitureFiles()
        {
            LoadChairFiles();
            LoadTableFiles();
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



        private static void SerializeChairs()
        {
            Framework.Objects.Furniture.ChairTileComponent chairTop = new Framework.Objects.Furniture.ChairTileComponent(new BasicItemInformation("Oak Chair", "A basic wooden chair", "Chairs", Color.Brown, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Chairs.OakChair", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Chair").texture, Color.White, 0, true, typeof(Framework.Objects.Furniture.ChairTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Chair"), new Animation(new Rectangle(0, 0, 16, 16)), new Dictionary<string, List<Animation>>() {
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
            }, "Default_" + (int)Framework.Enums.Direction.Down), Color.White, true, new Framework.Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.ChairInformation(false));
            Framework.Objects.Furniture.ChairTileComponent chairBottom = new Framework.Objects.Furniture.ChairTileComponent(new BasicItemInformation("Oak Chair", "A basic wooden chair", "Chairs", Color.Brown, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Chairs.OakChair", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Framework.Graphics.TextureManager.TextureManagers["Furniture"].getTexture("Oak Chair").texture, Color.White, 0, true, typeof(Framework.Objects.Furniture.ChairTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Chair"), new Animation(new Rectangle(0, 16, 16, 16)), new Dictionary<string, List<Animation>>() {
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
            }, "Default_" + (int)Framework.Enums.Direction.Down), Color.White, false, new Framework.Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.ChairInformation(true));
            Framework.Objects.Furniture.ChairMultiTiledObject oakChair = new Framework.Objects.Furniture.ChairMultiTiledObject(new BasicItemInformation("Oak Chair", "A wood chair you can place anywhere.", "Chair", Color.White, -300, 0, true, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Chairs.OakChair", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Chair").texture, Color.White, 0, true, typeof(Revitalize.Framework.Objects.Furniture.ChairMultiTiledObject), null, new AnimationManager(), Color.White, false, new Framework.Utilities.InventoryManager(), new LightManager()));

            ChairFactoryInfo top = new ChairFactoryInfo(chairTop);
            ChairFactoryInfo bottom = new ChairFactoryInfo(chairBottom);
            ChairFactoryInfo obj = new ChairFactoryInfo(oakChair);


            ModCore.Serializer.SerializeContentFile("OakChair_0_-1", top, ChairFolder);
            ModCore.Serializer.SerializeContentFile("OakChair_0_0", bottom, ChairFolder);
            ModCore.Serializer.SerializeContentFile("OakChair", obj, ChairFolder);
        }
        private static void DeserializeChairs()
        {
            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", ChairFolder))) Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", ChairFolder));
            string[] files = Directory.GetFiles(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", ChairFolder));

            Dictionary<string, ChairMultiTiledObject> chairObjects = new Dictionary<string, ChairMultiTiledObject>();

            //Deserialize container.
            foreach (string file in files)
            {
                if ((Path.GetFileName(file)).Contains("_")==true) continue;
                else
                {
                    chairObjects.Add(Path.GetFileNameWithoutExtension(file), new ChairMultiTiledObject(ModCore.Serializer.DeserializeContentFile<ChairFactoryInfo>(file).itemInfo));
                }
            }
            //Deseralize components
            foreach (string file in files)
            {
                if ((Path.GetFileName(file)).Contains("_")==false) continue;
                else
                {
                    
                    string[] splits = Path.GetFileNameWithoutExtension(file).Split('_');
                    string name = splits[0];
                    Vector2 offset = new Vector2(Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
                    ChairFactoryInfo info = ModCore.Serializer.DeserializeContentFile<ChairFactoryInfo>(file);
                    chairObjects[name].addComponent(offset, new ChairTileComponent(info.itemInfo, info.chairInfo));
                }
            }
            foreach (var v in chairObjects)
            {
                ModCore.customObjects.Add(v.Value.info.id, v.Value);
            }
        }

        private static void SerializeTableFiles()
        {
            TableTileComponent upperLeft = new TableTileComponent(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Table"),new Animation(new Rectangle(0,0,16,16),-1)),Color.White,true, new Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.TableInformation(true));
            TableTileComponent upperRight = new TableTileComponent(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Table"), new Animation(new Rectangle(16, 0, 16, 16), -1)), Color.White, true, new Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.TableInformation(true));
            TableTileComponent centerLeft = new TableTileComponent(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Table"), new Animation(new Rectangle(0, 16, 16, 16), -1)), Color.White, false, new Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.TableInformation(true));
            TableTileComponent centerRight = new TableTileComponent(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Table"), new Animation(new Rectangle(16, 16, 16, 16), -1)), Color.White, false, new Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.TableInformation(true));
            TableTileComponent bottomLeft = new TableTileComponent(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Table"), new Animation(new Rectangle(0, 32, 16, 16), -1)), Color.White, false, new Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.TableInformation(true));
            TableTileComponent bottomRight = new TableTileComponent(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableTileComponent), null, new AnimationManager(TextureManager.TextureManagers["Furniture"].getTexture("Oak Table"), new Animation(new Rectangle(16, 32, 16, 16), -1)), Color.White, false, new Utilities.InventoryManager(), new LightManager()), new Framework.Objects.InformationFiles.Furniture.TableInformation(true));

            TableMultiTiledObject obj = new TableMultiTiledObject(new BasicItemInformation("Oak Table", "A simple oak table to hold things.", "Tables", Color.White, -300, 0, false, 350, Vector2.Zero, true, true, "Omegasis.Revitalize.Furniture.Tables.OakTable", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", TextureManager.TextureManagers["Furniture"].getTexture("Oak Table").texture, Color.White, 0, true, typeof(TableMultiTiledObject), null, new AnimationManager(), Color.White, false, new Utilities.InventoryManager(), new LightManager()));

            TableFactoryInfo uL = new TableFactoryInfo(upperLeft);
            TableFactoryInfo uR = new TableFactoryInfo(upperRight);
            TableFactoryInfo cL = new TableFactoryInfo(centerLeft);
            TableFactoryInfo cR = new TableFactoryInfo(centerRight);
            TableFactoryInfo bL = new TableFactoryInfo(bottomLeft);
            TableFactoryInfo bR = new TableFactoryInfo(bottomRight);

            TableFactoryInfo table = new TableFactoryInfo(obj);


            ModCore.Serializer.SerializeContentFile("OakTable_0_0", uL, TablesFolder);
            ModCore.Serializer.SerializeContentFile("OakTable_1_0", uR, TablesFolder);
            ModCore.Serializer.SerializeContentFile("OakTable_0_1", cL, TablesFolder);
            ModCore.Serializer.SerializeContentFile("OakTable_1_1", cR, TablesFolder);
            ModCore.Serializer.SerializeContentFile("OakTable_0_2", bL, TablesFolder);
            ModCore.Serializer.SerializeContentFile("OakTable_1_2", bR, TablesFolder);

            ModCore.Serializer.SerializeContentFile("OakTable", table, TablesFolder);

        }

        private static void DeserializeTableFiles()
        {
            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", TablesFolder))) Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", TablesFolder));
            string[] files = Directory.GetFiles(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", TablesFolder));

            Dictionary<string, TableMultiTiledObject> chairObjects = new Dictionary<string, TableMultiTiledObject>();

            //Deserialize container.
            foreach (string file in files)
            {
                if ((Path.GetFileName(file)).Contains("_") == true) continue;
                else
                {
                    chairObjects.Add(Path.GetFileNameWithoutExtension(file), new TableMultiTiledObject(ModCore.Serializer.DeserializeContentFile<TableFactoryInfo>(file).info));
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
                    chairObjects[name].addComponent(offset, new TableTileComponent(info.info, info.tableInfo));
                }
            }
            foreach (var v in chairObjects)
            {
                ModCore.customObjects.Add(v.Value.info.id, v.Value);
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



    }
}
