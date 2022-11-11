using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Web;
using ProjectBase.Data;
using System.Web.Routing;
using System.Web.Mvc;
using ProjectBase.Utils.Entities;
using System.Security;

namespace ProjectBase.Data
{
    [SecurityCritical]
    public class MockWeb
    {
        protected Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
        protected Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();
        protected Mock<HttpContextBase> context = new Mock<HttpContextBase>();
        protected RouteCollection routes = new RouteCollection();

        public virtual ControllerContext GetContext(Controller controller)
        {
            RegisterRoutes();
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            response.Setup(x => x.ApplyAppPathModifier("/post1")).Returns("http://localhost/post1");


            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            return new ControllerContext(context.Object, new RouteData(), controller);
        }

        public virtual void RegisterRoutes()
        {
        }

        protected UrlHelper GetUrlHelper(Controller controller)
        {
            if (controller.ControllerContext == null)
                controller.ControllerContext = GetContext(controller);

            return new UrlHelper(controller.ControllerContext.RequestContext, routes);
        }

    }
}
