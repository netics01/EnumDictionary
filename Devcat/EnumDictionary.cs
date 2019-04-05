using System;
using System.Collections;
using System.Collections.Generic;

namespace Devcat
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // EnumDictionary
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // author: netics@nexon.co.kr
    public class EnumDictionary<TKey, TValue> :
        IEnumerable<KeyValuePair<TKey, TValue>>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
        IEnumerable,
        ICollection,
        IDictionary
        where TKey : struct
    {
        //================================================================================================================================
        // 속성
        //================================================================================================================================
        public readonly Dictionary<int, TValue> Raw;

        //================================================================================================================================
        // 생성
        //================================================================================================================================
        public EnumDictionary()
        {
            Raw = new Dictionary<int, TValue>();
        }

        //================================================================================================================================
        // Dictionary 구현
        //================================================================================================================================
        public int Count
        {
            get { return Raw.Count; }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public IEqualityComparer<TKey> Comparer
        {
            get { throw new NotImplementedException(); }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public KeyCollection Keys
        {
            get { return new KeyCollection(Raw.Keys); }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public ValueCollection Values
        {
            get { return new ValueCollection(Raw.Values); }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public void Add(TKey key, TValue value)
        {
            Raw.Add(ToInternalKey(key), value);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public void Clear()
        {
            Raw.Clear();
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public bool ContainsKey(TKey key)
        {
            return Raw.ContainsKey(ToInternalKey(key));
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public bool ContainsValue(TValue value)
        {
            return Raw.ContainsValue(value);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public Enumerator GetEnumerator()
        {
            return new Enumerator(Raw.GetEnumerator());
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public bool Remove(TKey key)
        {
            return Raw.Remove(ToInternalKey(key));
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public bool TryGetValue(TKey key, out TValue value)
        {
            return Raw.TryGetValue(ToInternalKey(key), out value);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public TValue this[TKey key]
        {
            get { return Raw[ToInternalKey(key)]; }
            set { Raw[ToInternalKey(key)] = value; }
        }

        //================================================================================================================================
        // IDictionary<TKey, TValue> 구현
        //================================================================================================================================
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get { return new KeyCollection(Raw.Keys); }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get { return new ValueCollection(Raw.Values); }
        }

        //================================================================================================================================
        // IDictionary 구현
        //================================================================================================================================
        ICollection IDictionary.Keys
        {
            get { return new KeyCollection(Raw.Keys); }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        ICollection IDictionary.Values
        {
            get { return new ValueCollection(Raw.Values); }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary)Raw).IsFixedSize; }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        bool IDictionary.IsReadOnly
        {
            get { return ((IDictionary)Raw).IsReadOnly; }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new Enumerator(Raw.GetEnumerator());
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        void IDictionary.Add(object key, object value)
        {
            ((IDictionary)Raw).Add(ToInternalKey((TKey)key), value);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        bool IDictionary.Contains(object key)
        {
            var k = ToInternalKey((TKey)key);
            return ((IDictionary)Raw).Contains(k);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        void IDictionary.Remove(object key)
        {
            var k = ToInternalKey((TKey)key);
            ((IDictionary)Raw).Remove(k);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        object IDictionary.this[object key]
        {
            get { return ((IDictionary)Raw)[ToInternalKey((TKey)key)]; }
            set { ((IDictionary)Raw)[ToInternalKey((TKey)key)] = value; }
        }

        //================================================================================================================================
        // ICollection<KeyValuePair<TKey, TValue>> 구현
        //================================================================================================================================
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return RawTypedCollection.IsReadOnly; }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
        {
            RawTypedCollection.Add(ToInternalPair(pair));
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            return RawTypedCollection.Contains(ToInternalPair(pair));
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
        {
            return RawTypedCollection.Remove(ToInternalPair(pair));
        }

        //================================================================================================================================
        // ICollection 구현
        //================================================================================================================================
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)Raw).IsSynchronized; }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        object ICollection.SyncRoot
        {
            get { return ((ICollection)Raw).SyncRoot; }
        }

        //================================================================================================================================
        // IEnumerable<KeyValuePair<TKey, TValue> 구현
        //================================================================================================================================
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(Raw.GetEnumerator());
        }

        //================================================================================================================================
        // IEnumerable 구현
        //================================================================================================================================
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(Raw.GetEnumerator());
        }

        //================================================================================================================================
        // object 구현
        //================================================================================================================================
        public override bool Equals(object rhs)
        {
            if( rhs == null ) { return false; }
            if( rhs.GetType() != GetType() ) { return false; }
            return Equals((EnumDictionary<TKey, TValue>)rhs);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public bool Equals(EnumDictionary<TKey, TValue> rhs)
        {
            return Raw.Equals(rhs.Raw);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return Raw.GetHashCode();
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            //Dictionary<,>.ToString()도 아래처럼 구현되어 있다.
            return GetType().ToString();
        }

        //================================================================================================================================
        // Enumerator 타입
        //================================================================================================================================
        public struct Enumerator : IEnumerator, IDisposable, IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            Dictionary<int, TValue>.Enumerator enumerator;

            internal Enumerator(Dictionary<int, TValue>.Enumerator enumerator_)
            {
                enumerator = enumerator_;
            }

            //================================================================================================================================
            // Dictionary.Enumerator 구현
            //================================================================================================================================
            public KeyValuePair<TKey, TValue> Current
            {
                get { return ToExternalPair(enumerator.Current); }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            public void Dispose()
            {
                enumerator.Dispose();
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            //================================================================================================================================
            // IEnumerator 구현
            //================================================================================================================================
            object IEnumerator.Current
            {
                get { return ToExternalPair(enumerator.Current); }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void IEnumerator.Reset()
            {
                ((IEnumerator)enumerator).Reset();
            }

            //================================================================================================================================
            // IDictionaryEnumerator 구현
            //================================================================================================================================
            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    var p = ((IDictionaryEnumerator)enumerator).Entry;
                    p.Key = ToExternalKey((int)p.Key);
                    return p;
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            object IDictionaryEnumerator.Key
            {
                get
                {
                    var k = ((IDictionaryEnumerator)enumerator).Key;
                    return ToExternalKey((int)k);
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            object IDictionaryEnumerator.Value
            {
                get { return ((IDictionaryEnumerator)enumerator).Value; }
            }
        }

        //================================================================================================================================
        // KeyCollection 타입
        //================================================================================================================================
        public class KeyCollection : IEnumerable, ICollection, ICollection<TKey>, IEnumerable<TKey>
        {
            Dictionary<int, TValue>.KeyCollection keyCollection;

            internal KeyCollection(Dictionary<int, TValue>.KeyCollection keyCollection_)
            {
                keyCollection = keyCollection_;
            }

            //================================================================================================================================
            // Dictionary.KeyCollection 구현
            //================================================================================================================================
            public int Count
            {
                get { return keyCollection.Count; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            public void CopyTo(TKey[] array, int index)
            {
                throw new NotImplementedException();
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            public Enumerator GetEnumerator()
            {
                return new Enumerator(keyCollection.GetEnumerator());
            }

            //================================================================================================================================
            // ICollection<TKey> 구현
            //================================================================================================================================
            bool ICollection<TKey>.IsReadOnly
            {
                get { return ((ICollection<int>)keyCollection).IsReadOnly; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void ICollection<TKey>.Add(TKey item)
            {
                ((ICollection<int>)keyCollection).Add(ToInternalKey(item));
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void ICollection<TKey>.Clear()
            {
                ((ICollection<int>)keyCollection).Clear();
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            bool ICollection<TKey>.Contains(TKey item)
            {
                return ((ICollection<int>)keyCollection).Contains(ToInternalKey(item));
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            bool ICollection<TKey>.Remove(TKey item)
            {
                return ((ICollection<int>)keyCollection).Remove(ToInternalKey(item));
            }

            //================================================================================================================================
            // ICollection 구현
            //================================================================================================================================
            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)keyCollection).IsSynchronized; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            object ICollection.SyncRoot
            {
                get { return ((ICollection)keyCollection).SyncRoot; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            //================================================================================================================================
            // IEnumerable<TKey> 구현
            //================================================================================================================================
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return new Enumerator(keyCollection.GetEnumerator());
            }

            //================================================================================================================================
            // IEnumerable 구현
            //================================================================================================================================
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(keyCollection.GetEnumerator());
            }

            //================================================================================================================================
            // KeyCollection.Enumerator 타입
            //================================================================================================================================
            public struct Enumerator : IEnumerator, IDisposable, IEnumerator<TKey>
            {
                Dictionary<int, TValue>.KeyCollection.Enumerator enumerator;

                internal Enumerator(Dictionary<int, TValue>.KeyCollection.Enumerator enumerator_)
                {
                    enumerator = enumerator_;
                }

                //================================================================================================================================
                // Dictionary.KeyCollection.Enumerator 구현
                //================================================================================================================================
                public TKey Current
                {
                    get { return ToExternalKey(enumerator.Current); }
                }
                //--------------------------------------------------------------------------------------------------------------------------------
                public void Dispose()
                {
                    enumerator.Dispose();
                }
                //--------------------------------------------------------------------------------------------------------------------------------
                public bool MoveNext()
                {
                    return enumerator.MoveNext();
                }

                //================================================================================================================================
                // IEnumerator 구현
                //================================================================================================================================
                void IEnumerator.Reset()
                {
                    ((IEnumerator)enumerator).Reset();
                }
                //--------------------------------------------------------------------------------------------------------------------------------
                object IEnumerator.Current
                {
                    get { return Current; }
                }
            }
        }

        //================================================================================================================================
        // ValueCollection 타입
        //================================================================================================================================
        public class ValueCollection : IEnumerable, ICollection, ICollection<TValue>, IEnumerable<TValue>
        {
            Dictionary<int, TValue>.ValueCollection valueCollection;

            internal ValueCollection(Dictionary<int, TValue>.ValueCollection valueCollection_)
            {
                valueCollection = valueCollection_;
            }

            //================================================================================================================================
            // Dictionary.ValueCollection 구현
            //================================================================================================================================
            public int Count
            {
                get { return valueCollection.Count; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            public void CopyTo(TValue[] array, int index)
            {
                throw new NotImplementedException();
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            public Enumerator GetEnumerator()
            {
                return new Enumerator(valueCollection.GetEnumerator());
            }

            //================================================================================================================================
            // ICollection<TValue> 구현
            //================================================================================================================================
            bool ICollection<TValue>.IsReadOnly
            {
                get { return ((ICollection<TValue>)valueCollection).IsReadOnly; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void ICollection<TValue>.Add(TValue item)
            {
                ((ICollection<TValue>)valueCollection).Add(item);
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void ICollection<TValue>.Clear()
            {
                ((ICollection<TValue>)valueCollection).Clear();
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            bool ICollection<TValue>.Contains(TValue item)
            {
                return ((ICollection<TValue>)valueCollection).Contains(item);
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            bool ICollection<TValue>.Remove(TValue item)
            {
                return ((ICollection<TValue>)valueCollection).Remove(item);
            }

            //================================================================================================================================
            // ICollection 구현
            //================================================================================================================================
            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)valueCollection).IsSynchronized; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            object ICollection.SyncRoot
            {
                get { return ((ICollection)valueCollection).SyncRoot; }
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            //================================================================================================================================
            // IEnumerable<TValue> 구현
            //================================================================================================================================
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return new Enumerator(valueCollection.GetEnumerator());
            }

            //================================================================================================================================
            // IEnumerable 구현
            //================================================================================================================================
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(valueCollection.GetEnumerator());
            }

            //================================================================================================================================
            // ValueCollection.Enumerator 타입
            //================================================================================================================================
            public struct Enumerator : IEnumerator, IDisposable, IEnumerator<TValue>
            {
                Dictionary<int, TValue>.ValueCollection.Enumerator enumerator;

                internal Enumerator(Dictionary<int, TValue>.ValueCollection.Enumerator enumerator_)
                {
                    enumerator = enumerator_;
                }

                //================================================================================================================================
                // Dictionary.KeyCollection.Enumerator 구현
                //================================================================================================================================
                public TValue Current
                {
                    get { return enumerator.Current; }
                }
                //--------------------------------------------------------------------------------------------------------------------------------
                public void Dispose()
                {
                    enumerator.Dispose();
                }
                //--------------------------------------------------------------------------------------------------------------------------------
                public bool MoveNext()
                {
                    return enumerator.MoveNext();
                }

                //================================================================================================================================
                // IEnumerator 구현
                //================================================================================================================================
                void IEnumerator.Reset()
                {
                    ((IEnumerator)enumerator).Reset();
                }
                //--------------------------------------------------------------------------------------------------------------------------------
                object IEnumerator.Current
                {
                    get { return ((IEnumerator)enumerator).Current; }
                }
            }
        }

        //================================================================================================================================
        // 전용
        //================================================================================================================================
        static KeyValuePair<int, TValue> ToInternalPair(KeyValuePair<TKey, TValue> pair)
        {
            return new KeyValuePair<int, TValue>(BitConvert.Enum32ToInt(pair.Key), pair.Value);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        static KeyValuePair<TKey, TValue> ToExternalPair(KeyValuePair<int, TValue> pair)
        {
            return new KeyValuePair<TKey, TValue>(BitConvert.IntToEnum32<TKey>(pair.Key), pair.Value);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        static int ToInternalKey(TKey key)
        {
            return BitConvert.Enum32ToInt(key);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        static TKey ToExternalKey(int key)
        {
            return BitConvert.IntToEnum32<TKey>(key);
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        ICollection<KeyValuePair<int, TValue>> RawTypedCollection
        {
            get { return Raw; }
        }
    }
}