using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

//
// adapted from :
// http://www.codeproject.com/KB/recipes/ClassKey.aspx
// 
// using in order to have a hash with a multiple-value key
//


namespace Benchmark_Instant_Reports_2.Infrastructure
{
    /// <summary>
    /// Defines a common set of operations and functionality for creating concrete key classes which allow us to lookup items in a collection
    /// using one or more of the properties in that collection.
    /// </summary>
    public abstract class ClassKey<T> where T : class
    {
        private T _CollectionItem = null;

        /// <summary>
        /// The collection item referenced by this key
        /// </summary>
        public T ClassReference
        {
            get { return _CollectionItem; }
            set { _CollectionItem = value; }
        }

        /// <summary>
        /// Init empty if needed
        /// </summary>
        public ClassKey() { }

        /// <summary>
        /// Init with specific collection item
        /// </summary>
        /// <param name="CollectionItem"></param>
        public ClassKey(T CollectionItem)
        {
            this.ClassReference = CollectionItem;
        }

        /// <summary>
        /// Compare based on hash code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ClassKey<T>)
            {
                return (obj as ClassKey<T>).GetHashCode() == this.GetHashCode();
            }
            else
                return false; //definitely not equal
        }

        public static bool operator ==(ClassKey<T> p1, ClassKey<T> p2)
        {
            //if both null, then equal
            if ((object)p1 == null && (object)p2 == null) return true;

            //if one or other null, then not since above we guaranteed if here one is not null
            if ((object)p1 == null || (object)p2 == null) return false;

            //compare on fields
            return (p1.Equals(p2));
        }

        public static bool operator !=(ClassKey<T> p1, ClassKey<T> p2)
        {
            return !(p1 == p2);
        }

        //must override to get list of key values
        public abstract object[] GetKeyValues();

        /// <summary>
        /// Implement hash code function to specify which columns will be used for the key without using reflection which may be a bit slow.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            object[] keyValues = GetKeyValues();

            //use co-prime numbers to salt the hashcode so same values in different order will not return as equal - see TestClassKeyXOROrderProblem to reproduce problem 
            //http://www.msnewsgroups.net/group/microsoft.public.dotnet.languages.csharp/topic36405.aspx
            //http://directxinfo.blogspot.com/2007/06/gethashcode-in-net.html
            int FinalHashCode = 17; //first co-prime number
            int OtherCoPrimeNumber = 37; //other co-prime number - ask me if I know what co-prime means, go ahead, ask me.

            //get total hashcode to return
            if (keyValues != null)
                foreach (object keyValue in keyValues)
                {
                    //can't get hash code if null
                    if (keyValue != null)
                    {
                        //if hashcodes are continually summed they will overflow
                        //this is ok by default since VS doesn't use the checked flag when compiling
                        //do unchecked here in case this is compiled with checked option
                        //in hash code calculation - overflow is ok for hashcode computation
                        unchecked
                        {
                            FinalHashCode = FinalHashCode * OtherCoPrimeNumber + keyValue.GetHashCode();
                        }
                    }
                }

            return FinalHashCode;
        }
    }
}