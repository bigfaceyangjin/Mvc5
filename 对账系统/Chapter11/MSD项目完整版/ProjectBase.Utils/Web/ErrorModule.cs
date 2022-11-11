using System;
using System.Web;
using log4net;

namespace ProjectBase.Utils
{
    /// <summary>
    /// Global error handler for logging web exceptions.
    /// Source from Karl Seguin @ http://codebetter.com/blogs/karlseguin/archive/2006/04/05/142355.aspx
    /// </summary>
    public class ErrorModule : IHttpModule
    {
        #region Fields and Properties

        private static readonly ILog logger = LogManager.GetLogger(typeof(ErrorModule));

        #endregion

        #region IHttpModule Members

        public void Init(HttpApplication context) {
            if (context == null)
            {
                throw new ArgumentException("context is not null");
            }
            context.Error += new EventHandler(application_Error);
        }

        public void Dispose() { }

        #endregion

        void application_Error(object sender, EventArgs e) {
            HttpContext ctx = HttpContext.Current;
            
            //get the inner most exception
            Exception exception;

            for (exception = ctx.Server.GetLastError(); exception.InnerException != null; exception = exception.InnerException) { }
            var httpException = exception as HttpException;
            if (httpException==null && httpException.ErrorCode == 404)
            {
                logger.Warn("A 404 occurred", exception);
            }
            else {
                logger.Error("ErrorModule caught an unhandled exception", exception);
            }
        }
    }
}
