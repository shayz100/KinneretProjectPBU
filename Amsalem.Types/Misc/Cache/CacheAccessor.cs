using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;
using System.Web;
namespace Amsalem.Types.Misc.Cache
{
    public class CacheAccessor
    {
        public static MemoryCache CurrentCaching
        {
            get;
            set;
        }

        static CacheAccessor()
        {
            CurrentCaching = MemoryCache.Default;
        }

        public List<KeyValuePair<string, object>> GetCacheItems(string prefix)
        {
            var res = CurrentCaching.Where(item => item.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
            return res;
        }

        public TItem GetItem<TItem>(string keyName)
        {
            var obj = CurrentCaching[keyName];
            TItem result = (TItem)obj;
            return result;
        }
        public TItem GetItem<TItem>(string keyName, Func<TItem> GetItem, TimeSpan expiration)
        {
            var obj = CurrentCaching[keyName];
            if (obj == null)
            {
                obj = GetItem();
                var expirationTime = DateTime.Now.Add(expiration);
                CurrentCaching.Add(keyName, obj, new DateTimeOffset(expirationTime));
            }
            TItem result = (TItem)obj;
            return result;
        }

        public void AddToCache(object itemToCache, string itemSignature, TimeSpan expiration)
        {
            var expirationTime = DateTime.Now.Add(expiration);
            CurrentCaching.Add(itemSignature, itemToCache, new DateTimeOffset(expirationTime));
        }
 
        public  TItem GetFromSession<TItem>(string keyName, Func<TItem> AcquireItem)
        {
            var session = HttpContext.Current.Session;
            if (session[keyName] == null)
            {
                TItem item = AcquireItem();
                session.Add(keyName, item);
            }
            return (TItem)session[keyName];
        }
    }
}

