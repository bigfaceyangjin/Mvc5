using System.Configuration;

namespace ProjectBase.Data
{
    /// <summary>
    /// 在 Web/App.config 中创建Section节来读取配置信息.
    /// </summary>
    public class OpenSessionInViewSection : ConfigurationSection
    {
        [ConfigurationProperty("sessionFactories", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SessionFactoriesCollection), AddItemName = "sessionFactory",
            ClearItemsName = "clearFactories")]
        public SessionFactoriesCollection SessionFactories {
            get {
                SessionFactoriesCollection sessionFactoriesCollection =
                    (SessionFactoriesCollection)base["sessionFactories"];
                return sessionFactoriesCollection;
            }
        }
    }
}
