using System.Configuration;

namespace ProjectBase.Data
{
    /// <summary>
    /// �� Web/App.config �д���Section������ȡ������Ϣ.
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
