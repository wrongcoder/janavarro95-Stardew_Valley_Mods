using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib.Utils;

namespace Revitalize.Framework.Networking
{
    public class NetClass: INetSerializable, IComparable
    {
        /// <summary>
        /// Strictly used for value types that haven't been casted to NetSerializableObjects
        /// </summary>
        public static Dictionary<Type, Type> CastingTypes = new Dictionary<Type, Type>()
        {
            { typeof(string), typeof(NetString) },
            {typeof(int),typeof(NetInt)},
            { typeof(char), typeof(NetChar) },
            { typeof(bool), typeof(NetBool) },
            { typeof(float), typeof(NetFloat) },
            { typeof(double), typeof(NetDouble) },
            { typeof(byte), typeof(NetByte) },
            { typeof(sbyte), typeof(NetSByte) },
            { typeof(Color32), typeof(NetColor32) },
            { typeof(Color), typeof(NetColor) },
            { typeof(Vector3), typeof(NetVector3) },
            { typeof(Vector2), typeof(NetVector2) },
            { typeof(Rect), typeof(NetRectangle) },
            { typeof(short), typeof(NetShort) },
            { typeof(ushort), typeof(NetUShort) },
            { typeof(long), typeof(NetLong) },
            { typeof(ulong), typeof(NetULong) },
            { typeof(uint), typeof(NetUInt) },
        };


        public NetDictionary<NetString, NetString> extraVariables;

        private Guid _guid;

        public virtual Guid GUID
        {
            get
            {
                if (this._guid == Guid.Empty)
                {
                    this._guid = Guid.NewGuid();
                }
                return this._guid;
            }
            set
            {
                this._guid = value;
            }
        }

        public NetClass()
        {
            this._guid = Guid.NewGuid();
        }


        public virtual void Deserialize(NetDataReader reader)
        {
            if (extraVariables == null) this.extraVariables = new NetDictionary<NetString, NetString>();
            this.extraVariables.Deserialize(reader);
            this.GUID = reader.GetGuid();
        }

        public virtual void Serialize(NetDataWriter writer)
        {
            if (extraVariables == null) this.extraVariables = new NetDictionary<NetString, NetString>();
            this.extraVariables.Serialize(writer);
            writer.Put(this.GUID);
        }

        /// <summary>
        /// Adds an extra variable to the net class.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="obj"></param>
        public virtual void addExtraVariable(string Key, object obj)
        {
            NetString str = GameManager.Self.serializer.ToJSONString(obj);
            if (this.extraVariables == null)
            {
                this.extraVariables = new NetDictionary<NetString, NetString>();
            }
            this.extraVariables.Add(Key, str);
        }


        /// <summary>
        /// Gets the extra variable from the net class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <returns></returns>
        public virtual T getExtraVariable<T>(string Key)
        {
            if (this.extraVariables.ContainsKey(Key))
            {
                T obj = GameManager.Self.serializer.DeserializeFromJSONString<T>(this.extraVariables[Key]);
                return obj;
            }
            else return default(T);
        }

        /// <summary>
        /// Removes an extra variable added to this net class.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public virtual bool removeExtraVariable(string Key)
        {
            return this.extraVariables.Remove(Key);
        }

        /// <summary>
        /// Attemps to cast data that does not inherit from NetSerializableObject to their respective NetSerializable types. Should really only be used for C# value types and some Unity Engine Types.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static NetClass CastNonNetData(object Data)
        {
            Type nonNetType = Data.GetType();
            if (CastingTypes.ContainsKey(nonNetType))
            {
                Type netType = CastingTypes[Data.GetType()];
                object v = Activator.CreateInstance(netType, new object[] { Data });
                return (NetClass)v;
            }
            return null;
        }

        public static T Deserialize<T>(NetDataReader Reader) where T : NetClass, new()
        {
            Type nonNetType = typeof(T);
            if (CastingTypes.ContainsKey(nonNetType))
            {
                object v = Activator.CreateInstance(nonNetType, new object[] { });
                return (T)v;
            }
            return null;
        }

        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1;

            NetClass other = obj as NetClass;
            if (other != null)
                return this.GUID.CompareTo(other.GUID);
            else
                throw new ArgumentException("Object is not a valid Net matching class!");
        }
    }
}
