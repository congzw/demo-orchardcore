//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Microsoft.Extensions.Configuration;

//namespace Common
//{
//    public class ConnectionStringSettings
//    {
//        public String Name { get; set; }
//        public String ConnectionString { get; set; }
//        public String ProviderName { get; set; }

//        public ConnectionStringSettings()
//        {
//        }

//        public ConnectionStringSettings(String name, String connectionString)
//            : this(name, connectionString, null)
//        {
//        }

//        public ConnectionStringSettings(String name, String connectionString, String providerName)
//        {
//            this.Name = name;
//            this.ConnectionString = connectionString;
//            this.ProviderName = providerName;
//        }

//        protected bool Equals(ConnectionStringSettings other)
//        {
//            return String.Equals(Name, other.Name) && String.Equals(ConnectionString, other.ConnectionString) && String.Equals(ProviderName, other.ProviderName);
//        }

//        public override bool Equals(Object obj)
//        {
//            if (ReferenceEquals(null, obj)) return false;
//            if (ReferenceEquals(this, obj)) return true;
//            if (obj.GetType() != this.GetType()) return false;
//            return Equals((ConnectionStringSettings)obj);
//        }

//        public override int GetHashCode()
//        {
//            unchecked
//            {
//                int hashCode = (Name != null ? Name.GetHashCode() : 0);
//                hashCode = (hashCode * 397) ^ (ConnectionString != null ? ConnectionString.GetHashCode() : 0);
//                hashCode = (hashCode * 397) ^ (ProviderName != null ? ProviderName.GetHashCode() : 0);
//                return hashCode;
//            }
//        }

//        public static bool operator ==(ConnectionStringSettings left, ConnectionStringSettings right)
//        {
//            return Equals(left, right);
//        }

//        public static bool operator !=(ConnectionStringSettings left, ConnectionStringSettings right)
//        {
//            return !Equals(left, right);
//        }
//    }

//    public class ConnectionStringSettingsCollection : IDictionary<String, ConnectionStringSettings>
//    {
//        private readonly Dictionary<String, ConnectionStringSettings> m_ConnectionStrings;

//        public ConnectionStringSettingsCollection()
//        {
//            m_ConnectionStrings = new Dictionary<String, ConnectionStringSettings>();
//        }

//        public ConnectionStringSettingsCollection(int capacity)
//        {
//            m_ConnectionStrings = new Dictionary<String, ConnectionStringSettings>(capacity);
//        }

//        #region IEnumerable methods
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return ((IEnumerable)m_ConnectionStrings).GetEnumerator();
//        }
//        #endregion

//        #region IEnumerable<> methods
//        IEnumerator<KeyValuePair<String, ConnectionStringSettings>> IEnumerable<KeyValuePair<String, ConnectionStringSettings>>.GetEnumerator()
//        {
//            return ((IEnumerable<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).GetEnumerator();
//        }
//        #endregion

//        #region ICollection<> methods
//        void ICollection<KeyValuePair<String, ConnectionStringSettings>>.Add(KeyValuePair<String, ConnectionStringSettings> item)
//        {
//            ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).Add(item);
//        }

//        void ICollection<KeyValuePair<String, ConnectionStringSettings>>.Clear()
//        {
//            ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).Clear();
//        }

//        Boolean ICollection<KeyValuePair<String, ConnectionStringSettings>>.Contains(KeyValuePair<String, ConnectionStringSettings> item)
//        {
//            return ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).Contains(item);
//        }

//        void ICollection<KeyValuePair<String, ConnectionStringSettings>>.CopyTo(KeyValuePair<String, ConnectionStringSettings>[] array, Int32 arrayIndex)
//        {
//            ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).CopyTo(array, arrayIndex);
//        }

//        Boolean ICollection<KeyValuePair<String, ConnectionStringSettings>>.Remove(KeyValuePair<String, ConnectionStringSettings> item)
//        {
//            return ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).Remove(item);
//        }

//        public Int32 Count => ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).Count;
//        public Boolean IsReadOnly => ((ICollection<KeyValuePair<String, ConnectionStringSettings>>)m_ConnectionStrings).IsReadOnly;
//        #endregion

//        #region IDictionary<> methods
//        public void Add(String key, ConnectionStringSettings value)
//        {
//            // NOTE only slight modification, we add back in the Name of connectionString here (since it is the key)
//            value.Name = key;
//            m_ConnectionStrings.Add(key, value);
//        }

//        public Boolean ContainsKey(String key)
//        {
//            return m_ConnectionStrings.ContainsKey(key);
//        }

//        public Boolean Remove(String key)
//        {
//            return m_ConnectionStrings.Remove(key);
//        }

//        public Boolean TryGetValue(String key, out ConnectionStringSettings value)
//        {
//            return m_ConnectionStrings.TryGetValue(key, out value);
//        }

//        public ConnectionStringSettings this[String key]
//        {
//            get => m_ConnectionStrings[key];
//            set => Add(key, value);
//        }

//        public ICollection<String> Keys => m_ConnectionStrings.Keys;
//        public ICollection<ConnectionStringSettings> Values => m_ConnectionStrings.Values;
//        #endregion
//    }

//    public static class ConnectionStringSettingsExtensions
//    {
//        public static ConnectionStringSettingsCollection ConnectionStrings(this IConfigurationRoot configuration, string section = "ConnectionStrings")
//        {
//            var connectionStringCollection = configuration.GetSection(section).Get<ConnectionStringSettingsCollection>();
//            if (connectionStringCollection == null)
//            {
//                return new ConnectionStringSettingsCollection();
//            }

//            return connectionStringCollection;
//        }

//        public static ConnectionStringSettings ConnectionString(this IConfigurationRoot configuration, String name, String section = "ConnectionStrings")
//        {
//            ConnectionStringSettings connectionStringSettings;

//            var connectionStringCollection = configuration.GetSection(section).Get<ConnectionStringSettingsCollection>();
//            if (connectionStringCollection == null ||
//                !connectionStringCollection.TryGetValue(name, out connectionStringSettings))
//            {
//                return null;
//            }

//            return connectionStringSettings;
//        }

//        //public static void HowTo(IConfigurationRoot configuration)
//        //{
//        //    var connectionStrings = configuration.ConnectionStrings();

//        //    foreach (var connectionString in connectionStrings.Values)
//        //    {
//        //        Console.WriteLine(connectionString.Name);
//        //        Console.WriteLine(connectionString.ConnectionString);
//        //        Console.WriteLine(connectionString.ProviderName);
//        //    }

//        //    var specificConnStr1 = connectionStrings["Test1"];
//        //    Console.WriteLine(specificConnStr1.Name);
//        //    Console.WriteLine(specificConnStr1.ConnectionString);
//        //    Console.WriteLine(specificConnStr1.ProviderName);

//        //    var specificConnStr2 = configuration.ConnectionString("Test2");
//        //    Console.WriteLine(specificConnStr2.Name);
//        //    Console.WriteLine(specificConnStr2.ConnectionString);
//        //    Console.WriteLine(specificConnStr2.ProviderName);
//        //}
//    }


//}
