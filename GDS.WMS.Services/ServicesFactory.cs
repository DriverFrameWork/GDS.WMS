using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace GDS.WMS.Services
{
    public class ServicesFactory
    {
        /// <summary>
        /// 对象字典
        /// </summary>
        public static IDictionary<string, object> ObjectDictionary = new Dictionary<string, object>();

        /// <summary>
        /// 所有服务管理
        /// </summary>
        /// <param name="it">接口类型</param>
        /// <param name="t">接口实现类型</param>
        public static void ObjectActivator(Type it, Type t)
        {
            if (!ObjectDictionary.ContainsKey(it.FullName))
            {
                ServicesFactory.ObjectDictionary.Add(it.FullName, Activator.CreateInstance(t));
            }
        }

        /// <summary>
        /// 获取接口实例
        /// </summary>
        ///// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            if (!ObjectDictionary.ContainsKey(typeof(T).FullName))
            {
                var obj = ServiceLocator.Instance.GetService<T>();
               
                if (obj != null)
                    ObjectDictionary.Add(typeof(T).FullName, obj);
            }
            return (T)ObjectDictionary[typeof(T).FullName];
        }

        public static object GetInstance(Type type)
        {
            if (!ObjectDictionary.ContainsKey(type.FullName))
            {
                object obj = ServiceLocator.Instance.GetService(type);

                ObjectDictionary.Add(type.FullName, obj);
            }
            return ObjectDictionary[type.FullName];
        }
    }
}
